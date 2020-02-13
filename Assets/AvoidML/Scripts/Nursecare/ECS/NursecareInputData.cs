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
    public struct NursecareInputData : IComponentData
    {
        public bool IsEnabled;
    }

    [DisableAutoCreation]
    public class NursecareInputDataSystem : JobComponentSystem
    {
        public Mocap2float3s mocap2float3s;

        private EntityQuery m_Query;

        protected override void OnCreate()
        {
            mocap2float3s = new Mocap2float3s(Path.Combine(Application.streamingAssetsPath, "nursecare/mocap/train/segment0.csv"));

            m_Query = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] { ComponentType.ReadOnly<NursecareData>(), typeof(NursecareInputData), typeof(TargetPosition), typeof(TargetPosition2LerpVelocity) },
                Options = EntityQueryOptions.IncludeDisabled
            });
        }

        [BurstCompile]
        struct NursecareUpdaterJob : IJobChunk
        {
            [ReadOnly] public float nextTime;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<float3> positions;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<bool> isEnableds;

            [ReadOnly] public ArchetypeChunkComponentType<NursecareData> NursecareDataType;
            public ArchetypeChunkComponentType<NursecareInputData> NursecareInputDataType;
            public ArchetypeChunkComponentType<TargetPosition> TargetPositionType;
            public ArchetypeChunkComponentType<TargetPosition2LerpVelocity> TargetPosition2LerpVelocityType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkNursecareData = chunk.GetNativeArray(NursecareDataType);
                var chunkNursecareInputData = chunk.GetNativeArray(NursecareInputDataType);
                var chunkTargetPosition = chunk.GetNativeArray(TargetPositionType);
                var chunkLerpInfo = chunk.GetNativeArray(TargetPosition2LerpVelocityType);

                for (var i = 0; i < chunk.Count; ++i) {
                    var isEnabled = isEnableds[chunkNursecareData[i].Index];
                    chunkNursecareInputData[i] = new NursecareInputData { IsEnabled = isEnabled };
                    if (isEnabled) {
                        chunkTargetPosition[i] = new TargetPosition { Value = positions[chunkNursecareData[i].Index] };
                        chunkLerpInfo[i] = new TargetPosition2LerpVelocity { reachTime = nextTime };
                    }
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
                // Disable all data
                return Entities.WithBurst().ForEach((ref NursecareInputData inputData) => inputData.IsEnabled = false).Schedule(inputDependencies);
            }

            return new NursecareUpdaterJob
            {
                nextTime = UnityEngine.Time.fixedTime + Constants.timeInterval / UnityEngine.Time.timeScale,
                positions = new NativeArray<float3>(Array.ConvertAll(positions, (v) => v ?? 0), Allocator.TempJob),
                isEnableds = new NativeArray<bool>(Array.ConvertAll(positions, (v) => v != null), Allocator.TempJob),
                NursecareDataType = GetArchetypeChunkComponentType<NursecareData>(true),
                NursecareInputDataType = GetArchetypeChunkComponentType<NursecareInputData>(false),
                TargetPositionType = GetArchetypeChunkComponentType<TargetPosition>(false),
                TargetPosition2LerpVelocityType = GetArchetypeChunkComponentType<TargetPosition2LerpVelocity>(false)
            }.Schedule(m_Query, inputDependencies);

        }
    }

    public class NursecareInputDataImpicitSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem m_CommandBufferSystem;

        protected override void OnCreate()
        {
            m_CommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer.Concurrent CommandBuffer = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();

            var job = Entities
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .WithChangeFilter<NursecareInputData>()
                .WithBurst()
                .ForEach((Entity e, int entityInQueryIndex, in NursecareInputData inputData) =>
                {
                    if (inputData.IsEnabled) {
                        CommandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, e);
                    } else
                        CommandBuffer.AddComponent<Disabled>(entityInQueryIndex, e);
                }).Schedule(inputDeps);

            m_CommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}
