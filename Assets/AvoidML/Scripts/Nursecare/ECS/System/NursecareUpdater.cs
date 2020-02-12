using System;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace AvoidML.Nursecare
{
    [DisableAutoCreation]
    public class NursecareUpdater : JobComponentSystem
    {
        public Mocap2float3s mocap2float3s;

        protected override void OnCreate()
        {
            mocap2float3s = new Mocap2float3s(Path.Combine(Application.streamingAssetsPath, "nursecare/mocap/train/segment0.csv"));
        }

        [BurstCompile]
        struct NursecareUpdaterJob : IJobForEach<NursecareData, Translation>
        {
            [ReadOnly, DeallocateOnJobCompletion]
            public NativeArray<float3> positions;

            public void Execute([ReadOnly] ref NursecareData nursecareData, [WriteOnly] ref Translation translation)
            {
                translation.Value = positions[nursecareData.Index];
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var positions = mocap2float3s.GetData();
            if (positions == null)
                return inputDependencies;

            var values = new NativeArray<float3>(Array.ConvertAll(positions, (v) => v ?? 0), Allocator.TempJob);
            var job = new NursecareUpdaterJob
            {
                positions = values
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
