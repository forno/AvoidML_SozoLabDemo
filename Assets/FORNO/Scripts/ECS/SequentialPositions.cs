using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace Forno.Ecs
{
    public struct SequentialPositionsBlobAsset
    {
        public BlobArray<float3> Positions;
    }

    [Serializable]
    public struct SequentialPositions : IComponentData
    {
        public BlobAssetReference<SequentialPositionsBlobAsset> BlobData;
    }

    [UpdateBefore(typeof(BuildPhysicsWorld))]
    public class SequentialPositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = UnityEngine.Time.fixedDeltaTime;
            if (deltaTime != 0) {
                Entities
                    .WithBurst()
                    .ForEach((ref PhysicsVelocity velocity, ref Translation translation, in SequentialPositions positionsRef, in SequenceIndex index, in LocalTime time, in SequenceFrequency frequency) =>
                    {
                        ref var positions = ref positionsRef.BlobData.Value.Positions;
                        var length = positions.Length;
                        var validIndex = clamp(index.Value, 0, length - 1);
                        var lastIndex = clamp(index.Value - 1, 0, length - 1);
                        if (any(isnan(positions[lastIndex]))) {
                            lastIndex = validIndex;
                        }
                        var divideTime = frac(time.Value * frequency.Value);
                        translation.Value = lerp(positions[lastIndex], positions[validIndex], divideTime);
                    }).ScheduleParallel();
            } else {
                Entities
                    .WithBurst()
                    .ForEach((ref PhysicsVelocity velocity, ref Translation translation, in SequentialPositions positionsRef, in SequenceIndex index, in LocalTime time, in SequenceFrequency frequency) =>
                    {
                        ref var positions = ref positionsRef.BlobData.Value.Positions;
                        var length = positions.Length;
                        var validIndex = clamp(index.Value, 0, length - 1);
                        var lastIndex = clamp(index.Value - 1, 0, length - 1);
                        if (any(isnan(positions[lastIndex]))) {
                            lastIndex = validIndex;
                        }
                        var divideTime = frac(time.Value * frequency.Value);
                        translation.Value = lerp(positions[lastIndex], positions[validIndex], divideTime);
                        velocity.Linear = new float3();
                    }).ScheduleParallel();
            }
        }
    }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SequentialPositionInitialSystem : SystemBase
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
                .ForEach((Entity e, int entityInQueryIndex, in SequentialPositions positionsRef, in SequenceIndex index) =>
                {
                    ref var positions = ref positionsRef.BlobData.Value.Positions;
                    var validIndex = clamp(index.Value, 0, positions.Length - 1);
                    if (any(isnan(positions[validIndex]))) {
                        commandBuffer1.AddComponent<Disabled>(entityInQueryIndex, e);
                    }
                }).ScheduleParallel(Dependency);

            var commandBuffer2 = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            var job2 = Entities
                .WithBurst()
                .WithAll<Disabled>()
                .ForEach((Entity e, int entityInQueryIndex, in SequentialPositions positionsRef, in SequenceIndex index) =>
                {
                    ref var positions = ref positionsRef.BlobData.Value.Positions;
                    var validIndex = clamp(index.Value, 0, positions.Length - 1);
                    if (!any(isnan(positions[validIndex]))) {
                        commandBuffer2.RemoveComponent<Disabled>(entityInQueryIndex, e);
                    }
                }).ScheduleParallel(Dependency);

            m_CommandBufferSystem.AddJobHandleForProducer(job1);
            m_CommandBufferSystem.AddJobHandleForProducer(job2);

            Dependency = JobHandle.CombineDependencies(job1, job2);
        }
    }
}
