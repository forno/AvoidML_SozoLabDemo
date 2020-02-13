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

namespace AvoidML.Nursecare
{
    [GenerateAuthoringComponent]
    public struct NursecareInputData : IComponentData { }

    [DisableAutoCreation]
    public class NursecareInputDataSystem : JobComponentSystem
    {
        public Mocap2float3s mocap2float3s;

        private EntityQuery m_Query;
        private EndInitializationEntityCommandBufferSystem m_CommandBufferSystem;

        protected override void OnCreate()
        {
            mocap2float3s = new Mocap2float3s(Path.Combine(Application.streamingAssetsPath, "nursecare/mocap/train/segment0.csv"));

            m_Query = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] {
                    ComponentType.ReadOnly<NursecareData>(),
                    ComponentType.ReadOnly<NursecareInputData>(),
                    typeof(TargetPosition),
                    typeof(TargetPosition2LerpVelocity) },
                Options = EntityQueryOptions.IncludeDisabled
            });
            m_CommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        [RequireComponentTag(typeof(NursecareInputData))]
        [BurstCompile]
        struct NursecareUpdaterJob : IJobForEachWithEntity<NursecareData, TargetPosition, TargetPosition2LerpVelocity>
        {
            [ReadOnly] public float nextTime;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<float3> positions;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<bool> isEnableds;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(
                Entity entity,
                int index,
                [ReadOnly] ref NursecareData nursecareData,
                [WriteOnly] ref TargetPosition targetPosition,
                [WriteOnly] ref TargetPosition2LerpVelocity lerpInfo)
            {
                if (isEnableds[nursecareData.Index]) {
                    targetPosition = new TargetPosition { Value = positions[nursecareData.Index] };
                    lerpInfo = new TargetPosition2LerpVelocity { reachTime = nextTime };
                    CommandBuffer.RemoveComponent<Disabled>(index, entity);
                } else {
                    CommandBuffer.AddComponent<Disabled>(index, entity);
                }
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
                EntityCommandBuffer.Concurrent CommandBuffer = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
                var destructJob = Entities.WithAll<NursecareInputData>().WithBurst().ForEach((Entity e, int entityInQueryIndex) => CommandBuffer.AddComponent<Disabled>(entityInQueryIndex, e)).Schedule(inputDependencies);
                m_CommandBufferSystem.AddJobHandleForProducer(destructJob);
                return destructJob;
            }

            var job = new NursecareUpdaterJob
            {
                nextTime = UnityEngine.Time.fixedTime + Constants.timeInterval / UnityEngine.Time.timeScale,
                positions = new NativeArray<float3>(Array.ConvertAll(positions, (v) => v ?? 0), Allocator.TempJob),
                isEnableds = new NativeArray<bool>(Array.ConvertAll(positions, (v) => v != null), Allocator.TempJob),
                CommandBuffer = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(m_Query, inputDependencies);

            m_CommandBufferSystem.AddJobHandleForProducer(job);
            return job;
        }
    }
}
