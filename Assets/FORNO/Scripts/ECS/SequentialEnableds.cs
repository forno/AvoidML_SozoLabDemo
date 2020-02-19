using System;
using Unity.Entities;
using Unity.Jobs;
using static Unity.Mathematics.math;

namespace Forno.Ecs
{
    public struct SequentialEnabledsBlobAsset
    {
        public BlobArray<bool> Enableds;
    }

    [Serializable]
    public struct SequentialEnableds : IComponentData
    {
        public BlobAssetReference<SequentialEnabledsBlobAsset> BlobData;
    }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class EnabledSequenceSystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem m_CommandBufferSystem;

        protected override void OnCreate()
        {
            m_CommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer1 = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            var job1 = Entities
                .WithBurst()
                .ForEach((Entity e, int entityInQueryIndex, in SequentialEnableds enabledsRef, in SequenceIndex index) =>
                {
                    ref var enableds = ref enabledsRef.BlobData.Value.Enableds;
                    var validIndex = clamp(index.Value, 0, enableds.Length - 1);
                    if (!enableds[validIndex]) {
                        commandBuffer1.AddComponent<Disabled>(entityInQueryIndex, e);
                    }
                }).ScheduleParallel(Dependency);

            var commandBuffer2 = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            var job2 = Entities
                .WithBurst()
                .WithAll<Disabled>()
                .ForEach((Entity e, int entityInQueryIndex, in SequentialEnableds enabledsRef, in SequenceIndex index) =>
                {
                    ref var enableds = ref enabledsRef.BlobData.Value.Enableds;
                    var validIndex = clamp(index.Value, 0, enableds.Length - 1);
                    if (enableds[validIndex]) {
                        commandBuffer2.RemoveComponent<Disabled>(entityInQueryIndex, e);
                    }
                }).ScheduleParallel(Dependency);

            m_CommandBufferSystem.AddJobHandleForProducer(job1);
            m_CommandBufferSystem.AddJobHandleForProducer(job2);

            Dependency = JobHandle.CombineDependencies(job1, job2);
        }
    }
}
