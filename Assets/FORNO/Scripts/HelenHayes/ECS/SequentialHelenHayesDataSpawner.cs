using Forno.Ecs;
using System.IO;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Hash128 = Unity.Entities.Hash128;
using UnityEngine.Assertions;

namespace Forno.HelenHayes
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class SequentialHelenHayesDataSpawner : MonoBehaviour
    {
        public GameObject Prefab;
        public string FilePath;
        public bool HasHeader = true;
        public float FrequencyOfSequence = 100f;
    }

    [Serializable]
    public struct SequenceSettings
    {
        public Hash128 Hash;
        public int StartIndex;
        public int Count;
    }

    public class SequentialHelenHayesDataConversionSystem : SequentialHelenHayesDataConversionSystemBase<SequentialHelenHayesDataSpawner> { }

    public class SequentialHelenHayesDataConversionSystemBase<T> : GameObjectConversionSystem
        where T : SequentialHelenHayesDataSpawner
    {
        protected virtual void InitHelenHayes(NativeArray<Entity> entities) { }

        protected override void OnUpdate()
        {
            var processBlobAssets = new NativeList<Hash128>(Constants.positionCount, Allocator.Temp);
            var blobFactoryPositions = new NativeList<float3>(Allocator.TempJob);
            var blobLength = 0;
            int currentIndex = 0;

            using (var positionContext = new BlobAssetComputationContext<SequenceSettings, SequentialPositionsBlobAsset>(BlobAssetStore, 32, Allocator.Temp)) {
                Entities.ForEach((T spawner) =>
                {
                    var filePathHash = (uint)spawner.FilePath.GetHashCode();
                    var isFirst = true;
                    var dataLength = -1;
                    for (var i = 0; i < Constants.positionCount; ++i) {
                        var hash = new Hash128(filePathHash, (uint)i, 0, 0);
                        processBlobAssets.Add(hash);
                        positionContext.AssociateBlobAssetWithUnityObject(hash, spawner.gameObject);
                        if (positionContext.NeedToComputeBlobAsset(hash)) {
                            if (isFirst) {
                                isFirst = false;
                                currentIndex = blobLength;
                                var readToEnd = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, spawner.FilePath));
                                dataLength = readToEnd.Length - (spawner.HasHeader ? 1 : 0);
                                blobLength += Constants.positionCount * dataLength;
                                blobFactoryPositions.Resize(blobLength, NativeArrayOptions.UninitializedMemory);
                                Fill(blobFactoryPositions, readToEnd, spawner.HasHeader, currentIndex);
                            }
                            var sequentialSettings = new SequenceSettings
                            {
                                Hash = hash,
                                StartIndex = currentIndex + i * dataLength,
                                Count = dataLength
                            };
                            positionContext.AddBlobAssetToCompute(hash, sequentialSettings);
                        }
                    }
                });

                using (var positionSettings = positionContext.GetSettings(Allocator.TempJob)) {
                    var positionJob = new ComputeSequentialPositionAssetJob(positionSettings, blobFactoryPositions);
                    positionJob.Schedule(positionJob.Settings.Length, 1).Complete();
                    for (var i = 0; i < positionSettings.Length; ++i) {
                        positionContext.AddComputedBlobAsset(positionSettings[i].Hash, positionJob.BlobAssets[i]);
                    }
                    positionJob.BlobAssets.Dispose();
                }

                blobFactoryPositions.Dispose();

                var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, BlobAssetStore);
                var index = 0;
                Entities.ForEach((T spawner) =>
                {
                    var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(spawner.Prefab, settings);
                    DstEntityManager.AddComponent<SequenceIndex>(prefab);
                    DstEntityManager.AddComponent<SequenceTimeFrac>(prefab);
                    DstEntityManager.AddComponentData(prefab, new SequenceFrequency { Value = spawner.FrequencyOfSequence });
                    using (var instances = DstEntityManager.Instantiate(prefab, Constants.positionCount, Allocator.Temp)) {
                        for (var i = 0; i < instances.Length; ++i) {
                            positionContext.GetBlobAsset(processBlobAssets[index], out var positionBlob);
                            DstEntityManager.AddComponentData(instances[i], new SequentialPositions {
                                BlobData = positionBlob,
                                Parent = GetPrimaryEntity(spawner)
                            });
                            ++index;
                        }
                        InitHelenHayes(instances);
                    }
                });

                processBlobAssets.Dispose();
            }
        }

        private static void Fill(NativeArray<float3> dstPosition, string[] src, bool isIgnoreHead, int curIndex)
        {
            var headSkip = isIgnoreHead ? 1 : 0;
            var forLimit = src.Length - headSkip;
            var sliceSize = dstPosition.Length / forLimit;
            Assert.AreEqual(sliceSize * forLimit, dstPosition.Length);
            for (var i = 0; i < forLimit; ++i) {
                var data = Array.ConvertAll(src[i + headSkip].Split(','), (string s) => { if (float.TryParse(s, out var f)) return f; else return float.NaN; });
                for (var j = 0; j < sliceSize; ++j) {
                    // Convert coordinate system
                    var position = new float3(data[j * 3], data[j * 3 + 2], data[j * 3 + 1]);
                    // 0.001f means milli metre to metre
                    dstPosition[curIndex + j * forLimit + i] = position * 0.001f;
                }
            }
        }
    }

    [BurstCompile]
    public struct ComputeSequentialPositionAssetJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<SequenceSettings> Settings;
        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>> BlobAssets;

        public ComputeSequentialPositionAssetJob(NativeArray<SequenceSettings> settings, NativeArray<float3> positions)
        {
            Settings = settings;
            Positions = positions;
            BlobAssets = new NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>>(settings.Length, Allocator.TempJob);
        }

        public void Execute(int index)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            var settings = Settings[index];
            ref var root = ref builder.ConstructRoot<SequentialPositionsBlobAsset>();
            var array = builder.Allocate(ref root.Positions, settings.Count);

            for (var i = 0; i < array.Length; ++i) {
                array[i] = Positions[settings.StartIndex + i];
            }

            BlobAssets[index] = builder.CreateBlobAssetReference<SequentialPositionsBlobAsset>(Allocator.Persistent);
            builder.Dispose();
        }
    }
}
