using Forno.Ecs;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Hash128 = Unity.Entities.Hash128;

namespace AvoidML.Nursecare
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class SequentialNursecareDataSpawner : MonoBehaviour, IDeclareReferencedPrefabs
    {
        public GameObject Prefab;
        public string FilePath;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Prefab);
        }
    }

    public struct SequentialSettings
    {
        public Hash128 Hash;
        public int PositionStartIndex;
        public int PositionCount;
    }

    public class SequentialNursecareDataConversionSystem : GameObjectConversionSystem
    {
        public bool HasHeader = true;

        protected override void OnUpdate()
        {
            var processBlobAssets = new NativeList<Hash128>(Constants.positionCount, Allocator.Temp);
            var currentIndex = 0;
            var blobLength = 0;
            var blobFactoryPositions = new NativeList<float3>(Allocator.TempJob);
            var blobFactoryEnableds = new NativeList<bool>(Allocator.TempJob);

            using (var positionContext = new BlobAssetComputationContext<SequentialSettings, SequentialPositionsBlobAsset>(BlobAssetStore, 32, Allocator.Temp))
            using (var enabledContext = new BlobAssetComputationContext<SequentialSettings, SequentialEnabledsBlobAsset>(BlobAssetStore, 32, Allocator.Temp)) {
                Entities.ForEach((SequentialNursecareDataSpawner spawner) =>
                {
                    var filePathHash = (uint)spawner.FilePath.GetHashCode();
                    var isFirst = false;
                    var dataLength = -1;
                    for (var i = 0; i < Constants.positionCount; ++i) {
                        var hash = new Hash128(filePathHash, (uint)i, 0, 0);
                        processBlobAssets.Add(hash);
                        positionContext.AssociateBlobAssetWithUnityObject(hash, spawner.gameObject);
                        enabledContext.AssociateBlobAssetWithUnityObject(hash, spawner.gameObject);
                        var positionNeedToCompute = positionContext.NeedToComputeBlobAsset(hash);
                        var enabledNeedToCompute = enabledContext.NeedToComputeBlobAsset(hash);
                        if (positionNeedToCompute || enabledNeedToCompute) {
                            if (isFirst) {
                                isFirst = false;
                                var readToEnd = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, spawner.FilePath));
                                dataLength = readToEnd.Length - (HasHeader ? 1 : 0);
                                blobLength = blobLength + Constants.positionCount * dataLength;
                                blobFactoryPositions.Resize(blobLength, NativeArrayOptions.UninitializedMemory);
                                blobFactoryEnableds.Resize(blobLength, NativeArrayOptions.UninitializedMemory);
                                fill(blobFactoryPositions, blobFactoryEnableds, readToEnd, HasHeader, currentIndex, Constants.positionCount);
                            }
                            var sequentialSettings = new SequentialSettings
                            {
                                Hash = hash,
                                PositionStartIndex = currentIndex + i * Constants.positionCount,
                                PositionCount = dataLength
                            };
                            if (positionNeedToCompute)
                                positionContext.AddBlobAssetToCompute(hash, sequentialSettings);
                            if (enabledNeedToCompute)
                                enabledContext.AddBlobAssetToCompute(hash, sequentialSettings);
                        }
                    }
                });

                using (var positionSettings = positionContext.GetSettings(Allocator.TempJob))
                using (var enabledSettings = positionContext.GetSettings(Allocator.TempJob)) {
                    var positionJob = new ComputeSequentialPositionAssetJob(positionSettings, blobFactoryPositions);
                    var positionJobHandle = positionJob.Schedule(positionJob.Settings.Length, 1);
                    var enabledJob = new ComputeSequentialEnabledAssetJob(enabledSettings, blobFactoryEnableds);
                    var enabledJobHandle = enabledJob.Schedule(enabledJob.Settings.Length, 1);
                    positionJobHandle.Complete(); enabledJobHandle.Complete();
                    for (var i = 0; i < positionSettings.Length; ++i) {
                        positionContext.AddComputedBlobAsset(positionSettings[i].Hash, positionJob.BlobAssets[i]);
                    }
                    for (var i = 0; i < enabledSettings.Length; ++i) {
                        enabledContext.AddComputedBlobAsset(enabledSettings[i].Hash, enabledJob.BlobAssets[i]);
                    }
                    enabledJob.BlobAssets.Dispose();
                    positionJob.BlobAssets.Dispose();
                }

                var index = 0;
                Entities.ForEach((SequentialNursecareDataSpawner spawner) =>
                {
                    using (var instances = new NativeArray<Entity>(Constants.positionCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory)) {
                        DstEntityManager.Instantiate(GetPrimaryEntity(spawner.Prefab), instances);
                        for (var i = 0; i < Constants.positionCount; ++i) {
                            positionContext.GetBlobAsset(processBlobAssets[index++], out var positionBlob);
                            DstEntityManager.AddComponentData(instances[i], new SequentialPositions { Value = positionBlob });
                            enabledContext.GetBlobAsset(processBlobAssets[index], out var enabledBlob);
                            DstEntityManager.AddComponentData(instances[i], new SequentialEnableds { Value = enabledBlob });
                        }
                    }
                });

                blobFactoryEnableds.Dispose();
                blobFactoryPositions.Dispose();
                processBlobAssets.Dispose();
            }
        }

        private static void fill(NativeArray<float3> dstPosition, NativeArray<bool> dstEnabled, string[] src, bool isIgnoreHead, int curIndex, int sliceSize)
        {
            for (var i = (isIgnoreHead ? 1 : 0); i < src.Length; ++i) {
                var data = Array.ConvertAll<string, float?>(src[i].Split(','), (string s) => { if (float.TryParse(s, out var f)) return f; else return null; });
                for (var j = 0; j < sliceSize; ++j) {
                    // Convert coordinate system
                    var v0 = data[j * 3];
                    var v1 = data[j * 3 + 2];
                    var v2 = data[j * 3 + 1];
                    var isEnabled = v0.HasValue && v1.HasValue && v2.HasValue;
                    dstPosition[curIndex + j * src.Length + i] = (isEnabled ? new float3(v0.Value, v1.Value, v2.Value) : new float3());
                    dstEnabled[curIndex + j * src.Length + i] = isEnabled;
                }
            }
        }
    }

    [BurstCompile]
    public struct ComputeSequentialPositionAssetJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<SequentialSettings> Settings;
        [ReadOnly] public NativeArray<float3> Positions;
        public NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>> BlobAssets;

        public ComputeSequentialPositionAssetJob(NativeArray<SequentialSettings> settings, NativeArray<float3> positions)
        {
            Settings = settings;
            Positions = positions;
            BlobAssets = new NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>>(settings.Length, Allocator.TempJob);
        }

        public void Execute(int index)
        {
            throw new System.NotImplementedException();
        }
    }

    [BurstCompile]
    public struct ComputeSequentialEnabledAssetJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<SequentialSettings> Settings;
        [ReadOnly] public NativeArray<bool> Enableds;
        public NativeArray<BlobAssetReference<SequentialEnabledsBlobAsset>> BlobAssets;

        public ComputeSequentialEnabledAssetJob(NativeArray<SequentialSettings> settings, NativeArray<bool> enableds)
        {
            Settings = settings;
            Enableds = enableds;
            BlobAssets = new NativeArray<BlobAssetReference<SequentialEnabledsBlobAsset>>(settings.Length, Allocator.TempJob);
        }

        public void Execute(int index)
        {
            throw new NotImplementedException();
        }
    }
}
