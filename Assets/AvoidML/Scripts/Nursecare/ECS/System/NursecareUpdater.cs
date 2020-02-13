using Forno.Ecs;
using System;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
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
        struct NursecareUpdaterJob : IJobForEach<NursecareData, TargetPosition, TargetPosition2LerpVelocity>
        {
            [ReadOnly, DeallocateOnJobCompletion]
            public NativeArray<float3> positions;
            [ReadOnly]
            public float elapsedTime;

            public void Execute([ReadOnly] ref NursecareData nursecareData, [WriteOnly] ref TargetPosition targetPosition, [WriteOnly] ref TargetPosition2LerpVelocity lerpInfo)
            {
                targetPosition.Value = positions[nursecareData.Index];
                lerpInfo.reachTime = elapsedTime + Constants.timeInterval;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            if (mocap2float3s == null)
                return inputDependencies;
            var positions = mocap2float3s.GetData();
            if (positions == null) {
                mocap2float3s.Dispose();
                mocap2float3s = null;
                return inputDependencies;
            }

            var values = new NativeArray<float3>(Array.ConvertAll(positions, (v) => v ?? 0), Allocator.TempJob);
            var job = new NursecareUpdaterJob
            {
                positions = values,
                elapsedTime = UnityEngine.Time.fixedTime
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
