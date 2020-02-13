using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace AvoidML.Nursecare
{
    [GenerateAuthoringComponent]
    public struct NursecareSpawner : IComponentData
    {
        public Entity Prefab;
        public int PositionCount;
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class NursecareSpawnerSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        [BurstCompile(CompileSynchronously = true)]
        struct NursecareSpawnerJob : IJobForEachWithEntity<NursecareSpawner, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public void Execute(Entity entity, int index, [ReadOnly] ref NursecareSpawner nursecareSpawner, [ReadOnly] ref LocalToWorld location)
            {
                for (int i = 0; i < nursecareSpawner.PositionCount; i++) {
                    var instance = CommandBuffer.Instantiate(index, nursecareSpawner.Prefab);
                    CommandBuffer.SetComponent(index, instance, new NursecareData { Index = i });
                }

                // Delete Spawner
                CommandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle = new NursecareSpawnerJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);

            m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}
