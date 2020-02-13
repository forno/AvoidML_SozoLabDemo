using Forno.Ecs;
using System.IO;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Forno.Nursecare
{
    [GenerateAuthoringComponent]
    public struct NursecareInputData : IComponentData
    {
        public int Index;
    }

    [DisableAutoCreation]
    public class NursecareInputDataSystem : JobComponentSystem
    {
        public Mocap2float3s mocap2float3s;

        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            mocap2float3s = new Mocap2float3s(Path.Combine(Application.streamingAssetsPath, "nursecare/mocap/train/segment0.csv"));
        }

        [BurstCompile]
        struct NursecareUpdaterJob : IJobForEach<NursecareInputData, TargetPosition, TargetPosition2LerpVelocity>
        {
            [ReadOnly, DeallocateOnJobCompletion]
            public NativeArray<float3> positions;
            [ReadOnly]
            public float elapsedTime;

            public void Execute([ReadOnly] ref NursecareInputData nursecareData, [WriteOnly] ref TargetPosition targetPosition, [WriteOnly] ref TargetPosition2LerpVelocity lerpInfo)
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
