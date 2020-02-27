using Forno.Ecs;
using System.IO;
using System.Linq;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;
using UnityEngine;
using Hash128 = Unity.Entities.Hash128;

namespace Forno.HelenHayes
{

    [Serializable]
    public struct SequentialHelenHayesData : IComponentData
    {
        public BlobAssetReference<SequentialPositionsBlobAsset> BlobData;
    }

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class SequentialHelenHayes : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string FilePath;
        public bool HasHeader = true;

        [Header("Indexes in csv file")]
        [Range(0, Constants.PositionCount - 1)] public int FrontHeadIndex     =  0;
        [Range(0, Constants.PositionCount - 1)] public int TopHeadIndex       =  1;
        [Range(0, Constants.PositionCount - 1)] public int RearHeadIndex      =  2;
        [Range(0, Constants.PositionCount - 1)] public int RightOffsetIndex   =  3;
        [Range(0, Constants.PositionCount - 1)] public int VSacralIndex       =  4;
        [Range(0, Constants.PositionCount - 1)] public int LeftShoulderIndex  =  5;
        [Range(0, Constants.PositionCount - 1)] public int LeftElbowIndex     =  6;
        [Range(0, Constants.PositionCount - 1)] public int LeftWristIndex     =  7;
        [Range(0, Constants.PositionCount - 1)] public int RightShoulderIndex =  8;
        [Range(0, Constants.PositionCount - 1)] public int RightElbowIndex    =  9;
        [Range(0, Constants.PositionCount - 1)] public int RightWristIndex    = 10;
        [Range(0, Constants.PositionCount - 1)] public int LeftAsisIndex      = 11;
        [Range(0, Constants.PositionCount - 1)] public int LeftTightIndex     = 12;
        [Range(0, Constants.PositionCount - 1)] public int LeftKneeIndex      = 13;
        [Range(0, Constants.PositionCount - 1)] public int LeftKneeMedIndex   = 14;
        [Range(0, Constants.PositionCount - 1)] public int LeftShankIndex     = 15;
        [Range(0, Constants.PositionCount - 1)] public int LeftAnkleIndex     = 16;
        [Range(0, Constants.PositionCount - 1)] public int LeftAnkleMedIndex  = 17;
        [Range(0, Constants.PositionCount - 1)] public int LeftHeelIndex      = 18;
        [Range(0, Constants.PositionCount - 1)] public int LeftToeIndex       = 18;
        [Range(0, Constants.PositionCount - 1)] public int RightAsisIndex     = 19;
        [Range(0, Constants.PositionCount - 1)] public int RightTightIndex    = 20;
        [Range(0, Constants.PositionCount - 1)] public int RightKneeIndex     = 21;
        [Range(0, Constants.PositionCount - 1)] public int RightKneeMedIndex  = 22;
        [Range(0, Constants.PositionCount - 1)] public int RightShankIndex    = 23;
        [Range(0, Constants.PositionCount - 1)] public int RightAnkleIndex    = 24;
        [Range(0, Constants.PositionCount - 1)] public int RightAnkleMedIndex = 25;
        [Range(0, Constants.PositionCount - 1)] public int RightHeelIndex     = 26;
        [Range(0, Constants.PositionCount - 1)] public int RightToeIndex      = 27;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            using (var context = new BlobAssetComputationContext<SequentialHelenHayesSettings, SequentialPositionsBlobAsset>(conversionSystem.BlobAssetStore, 1, Allocator.Temp)) {
                var hash = new Hash128((uint)FilePath.GetHashCode(), 0, 0, 0);
                context.AssociateBlobAssetWithUnityObject(hash, gameObject);
                if (context.NeedToComputeBlobAsset(hash)) {
                    var indexes = new NativeArray<int>(Constants.PositionCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                    indexes[Constants.FrontHeadIndex]     = FrontHeadIndex;
                    indexes[Constants.TopHeadIndex]       = TopHeadIndex;
                    indexes[Constants.RearHeadIndex]      = RearHeadIndex;
                    indexes[Constants.RightOffsetIndex]   = RightOffsetIndex;
                    indexes[Constants.VSacralIndex]       = VSacralIndex;
                    indexes[Constants.LeftShoulderIndex]  = LeftShoulderIndex;
                    indexes[Constants.LeftElbowIndex]     = LeftElbowIndex;
                    indexes[Constants.LeftWristIndex]     = LeftWristIndex;
                    indexes[Constants.RightShoulderIndex] = RightShoulderIndex;
                    indexes[Constants.RightElbowIndex]    = RightElbowIndex;
                    indexes[Constants.RightWristIndex]    = RightWristIndex;
                    indexes[Constants.LeftAsisIndex]      = LeftAsisIndex;
                    indexes[Constants.LeftTightIndex]     = LeftTightIndex;
                    indexes[Constants.LeftKneeIndex]      = LeftKneeIndex;
                    indexes[Constants.LeftKneeMedIndex]   = LeftKneeMedIndex;
                    indexes[Constants.LeftShankIndex]     = LeftShankIndex;
                    indexes[Constants.LeftAnkleIndex]     = LeftAnkleIndex;
                    indexes[Constants.LeftAnkleMedIndex]  = LeftAnkleMedIndex;
                    indexes[Constants.LeftHeelIndex]      = LeftHeelIndex;
                    indexes[Constants.LeftToeIndex]       = LeftToeIndex;
                    indexes[Constants.RightAsisIndex]     = RightAsisIndex;
                    indexes[Constants.RightTightIndex]    = RightTightIndex;
                    indexes[Constants.RightKneeIndex]     = RightKneeIndex;
                    indexes[Constants.RightKneeMedIndex]  = RightKneeMedIndex;
                    indexes[Constants.RightShankIndex]    = RightShankIndex;
                    indexes[Constants.RightAnkleIndex]    = RightAnkleIndex;
                    indexes[Constants.RightAnkleMedIndex] = RightAnkleMedIndex;
                    indexes[Constants.RightHeelIndex]     = RightHeelIndex;
                    indexes[Constants.RightToeIndex]      = RightToeIndex;
                    Assert.AreEqual(29, indexes.Distinct().Count());

                    var readToEnd = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, FilePath));
                    var headSkip = (HasHeader ? 1 : 0);
                    var dataLength = readToEnd.Length - headSkip;

                    var positions = new NativeArray<float3>(dataLength * Constants.PositionCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                    for (var i = 0; i < dataLength; ++i) {
                        var data = Array.ConvertAll(readToEnd[i + headSkip].Split(','), (string s) => { if (float.TryParse(s, out var f)) return f; else return float.NaN; });
                        for (var j = 0; j < Constants.PositionCount; ++j) {
                            // Convert coordinate system, and 0.001f means milli metre to metre
                            positions[i * Constants.PositionCount + j] = new float3(data[j * 3], data[j * 3 + 2], data[j * 3 + 1]) * 0.001f;
                        }
                    }

                    context.AddBlobAssetToCompute(hash, new SequentialHelenHayesSettings());
                    var job = new SequentialHelenHayesJob(positions, indexes);
                    job.Schedule().Complete();
                    context.AddComputedBlobAsset(hash, job.BlobAssets[0]);
                    job.BlobAssets.Dispose();
                }

                context.GetBlobAsset(hash, out var blob);
                dstManager.AddComponentData(entity, new SequentialHelenHayesData { BlobData = blob });
            }
        }

        private struct SequentialHelenHayesSettings { }

        [BurstCompile]
        private struct SequentialHelenHayesJob : IJob
        {
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<float3> Positions;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<int> Indexes;
            public NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>> BlobAssets;

            public SequentialHelenHayesJob(NativeArray<float3> positions, NativeArray<int> indexes)
            {
                Positions = positions;
                Indexes = indexes;
                BlobAssets = new NativeArray<BlobAssetReference<SequentialPositionsBlobAsset>>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

                Assert.AreEqual(Indexes.Length, Constants.PositionCount);
                Assert.IsTrue(Positions.Length % Constants.PositionCount == 0);
            }

            public void Execute()
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref var root = ref builder.ConstructRoot<SequentialPositionsBlobAsset>();
                var length = Positions.Length;
                var array = builder.Allocate(ref root.Positions, length);
                var stepLength = length / Constants.PositionCount;
                for (var i = 0; i < stepLength; ++i) {
                    var alignIndex = i * Constants.PositionCount;
                    for (var j = 0; j < Constants.PositionCount; ++j) {
                        array[alignIndex + j] = Positions[alignIndex + Indexes[j]];
                    }
                }
                BlobAssets[0] = builder.CreateBlobAssetReference<SequentialPositionsBlobAsset>(Allocator.Persistent);
                builder.Dispose();
            }
        }
    }
}
