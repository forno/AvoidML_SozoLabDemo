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
using Unity.Transforms;

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
                    ComponentType.ReadOnly<NursecareInputData>(),
                    ComponentType.ReadOnly<NursecareData>(),
                    typeof(TargetPosition),
                    typeof(TargetPosition2LerpVelocity),
                    typeof(Translation)},
                Options = EntityQueryOptions.IncludeDisabled
            });
            m_CommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        struct NursecareUpdaterJob : IJobChunk
        {
            [ReadOnly] public float nextTime;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<float3> positions;
            [ReadOnly, DeallocateOnJobCompletion] public NativeArray<bool> isEnableds;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            [ReadOnly] public ArchetypeChunkComponentType<NursecareData> NursecareDataType;
            public ArchetypeChunkComponentType<TargetPosition> TargetPositionType;
            public ArchetypeChunkComponentType<TargetPosition2LerpVelocity> TargetPosition2LerpVelocityType;
            public ArchetypeChunkComponentType<Translation> TranslationType;
            [ReadOnly] public ArchetypeChunkComponentType<Disabled> DisabledType;
            [ReadOnly] public ArchetypeChunkEntityType EntityType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkNursecareData = chunk.GetNativeArray(NursecareDataType);
                var chunkTargetPosition = chunk.GetNativeArray(TargetPositionType);
                var chunkTargetPosition2LerpVelocity = chunk.GetNativeArray(TargetPosition2LerpVelocityType);
                var chunkTranslation = chunk.GetNativeArray(TranslationType);
                var entities = chunk.GetNativeArray(EntityType);

                if (chunk.Has(DisabledType)) {
                    for (var i = 0; i < chunk.Count; ++i) {
                        if (isEnableds[chunkNursecareData[i].Index]) {
                            var position = positions[chunkNursecareData[i].Index];
                            chunkTargetPosition[i] = new TargetPosition { Value = position };
                            chunkTargetPosition2LerpVelocity[i] = new TargetPosition2LerpVelocity { reachTime = nextTime };
                            // Warp on Restore
                            chunkTranslation[i] = new Translation { Value = position };
                            CommandBuffer.RemoveComponent<Disabled>(chunkIndex, entities[i]);
                        }
                    }
                } else {
                    for (var i = 0; i < chunk.Count; ++i) {
                        if (isEnableds[chunkNursecareData[i].Index]) {
                            chunkTargetPosition[i] = new TargetPosition { Value = positions[chunkNursecareData[i].Index] };
                            chunkTargetPosition2LerpVelocity[i] = new TargetPosition2LerpVelocity { reachTime = nextTime };
                        } else {
                            CommandBuffer.AddComponent<Disabled>(chunkIndex, entities[i]);
                        }
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
                EntityCommandBuffer.Concurrent CommandBuffer = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
                var destructJob = Entities
                    .WithBurst()
                    .WithAll<NursecareInputData>()
                    .ForEach((Entity e, int entityInQueryIndex) => CommandBuffer.AddComponent<Disabled>(entityInQueryIndex, e))
                    .Schedule(inputDependencies);
                m_CommandBufferSystem.AddJobHandleForProducer(destructJob);
                return destructJob;
            }

            var job = new NursecareUpdaterJob
            {
                nextTime = UnityEngine.Time.fixedTime + Constants.timeInterval / UnityEngine.Time.timeScale,
                positions = new NativeArray<float3>(Array.ConvertAll(positions, (v) => v ?? 0), Allocator.TempJob),
                isEnableds = new NativeArray<bool>(Array.ConvertAll(positions, (v) => v != null), Allocator.TempJob),
                CommandBuffer = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent(),

                NursecareDataType = GetArchetypeChunkComponentType<NursecareData>(true),
                TargetPositionType = GetArchetypeChunkComponentType<TargetPosition>(false),
                TargetPosition2LerpVelocityType = GetArchetypeChunkComponentType<TargetPosition2LerpVelocity>(false),
                TranslationType = GetArchetypeChunkComponentType<Translation>(false),
                DisabledType = GetArchetypeChunkComponentType<Disabled>(true),
                EntityType = GetArchetypeChunkEntityType(),
            }.Schedule(m_Query, inputDependencies);

            m_CommandBufferSystem.AddJobHandleForProducer(job);
            return job;
        }
    }
}
