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
        public Entity Parent;
    }

    [UpdateBefore(typeof(BuildPhysicsWorld))]
    public class SequentialPositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var localToWorldFromEntity = GetComponentDataFromEntity<LocalToWorld>(true);
            var deltaTime = UnityEngine.Time.fixedDeltaTime;
            if (deltaTime != 0) {
                Entities
                    .WithReadOnly(localToWorldFromEntity)
                    .ForEach((ref PhysicsVelocity velocity, ref Translation translation, in SequentialPositions positionsRef, in SequenceIndex index, in SequenceTimeFrac frac, in SequenceFrequency frequency) =>
                    {
                        ref var positions = ref positionsRef.BlobData.Value.Positions;
                        var validIndex = clamp(index.Value, 0, positions.Length - 1);
                        var lastIndex = clamp(index.Value - 1, 0, positions.Length - 1);
                        if (any(isnan(positions[lastIndex]))) {
                            lastIndex = validIndex;
                        }
                        var localPosition = transform(localToWorldFromEntity[positionsRef.Parent].Value, positions[validIndex]);
                        var localLastPosition = transform(localToWorldFromEntity[positionsRef.Parent].Value, positions[lastIndex]);
                        translation.Value = lerp(localLastPosition, localPosition, frac.Value);
                        // TODO: implement correct velocity
                        //velocity.Linear = (localPosition - localLastPosition) / deltaTime * frequency.Value / deltaTime;
                    }).ScheduleParallel();
            } else {
                Entities
                    .WithReadOnly(localToWorldFromEntity)
                    .WithAll<SequenceFrequency>()
                    .ForEach((ref PhysicsVelocity velocity, ref Translation translation, in SequentialPositions positionsRef, in SequenceIndex index, in SequenceTimeFrac frac) =>
                    {
                        ref var positions = ref positionsRef.BlobData.Value.Positions;
                        var validIndex = clamp(index.Value, 0, positions.Length - 1);
                        var lastIndex = clamp(index.Value - 1, 0, positions.Length - 1);
                        if (any(isnan(positions[lastIndex]))) {
                            lastIndex = validIndex;
                        }
                        var localPosition = transform(localToWorldFromEntity[positionsRef.Parent].Value, positions[validIndex]);
                        var localLastPosition = transform(localToWorldFromEntity[positionsRef.Parent].Value, positions[lastIndex]);
                        translation.Value = lerp(localLastPosition, localPosition, frac.Value);
                        velocity.Linear = Unity.Mathematics.float3.zero;
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
                .ForEach((Entity entity, int entityInQueryIndex, in SequentialPositions positionsRef, in SequenceIndex index) =>
                {
                    ref var positions = ref positionsRef.BlobData.Value.Positions;
                    var validIndex = clamp(index.Value, 0, positions.Length - 1);
                    if (any(isnan(positions[validIndex]))) {
                        commandBuffer1.AddComponent<Disabled>(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel(Dependency);

            var commandBuffer2 = m_CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            var job2 = Entities
                .WithAll<Disabled>()
                .ForEach((Entity entity, int entityInQueryIndex, in SequentialPositions positionsRef, in SequenceIndex index) =>
                {
                    ref var positions = ref positionsRef.BlobData.Value.Positions;
                    var validIndex = clamp(index.Value, 0, positions.Length - 1);
                    if (!any(isnan(positions[validIndex]))) {
                        commandBuffer2.RemoveComponent<Disabled>(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel(Dependency);

            m_CommandBufferSystem.AddJobHandleForProducer(job1);
            m_CommandBufferSystem.AddJobHandleForProducer(job2);

            Dependency = JobHandle.CombineDependencies(job1, job2);
        }
    }
}
