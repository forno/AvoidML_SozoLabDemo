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
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class NursecareSpawnerSystem : JobComponentSystem
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        [BurstCompile(CompileSynchronously = true)]
        struct NursecareSpawnerJob : IJobForEachWithEntity<NursecareSpawner>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref NursecareSpawner nursecareSpawner)
            {
                for (int i = 0; i < Constants.positionCount; i++) {
                    var instance = CommandBuffer.Instantiate(index, nursecareSpawner.Prefab);
                    CommandBuffer.SetComponent(index, instance, new NursecareData { Index = i });
                    CommandBuffer.AddComponent(index, instance, new Parent { Value = entity });
                    CommandBuffer.AddComponent<LocalToParent>(index, instance);
                    // Init by disabled
                    CommandBuffer.AddComponent<Disabled>(index, instance);
                }

                // Remove Spawn Flag
                CommandBuffer.RemoveComponent<NursecareSpawner>(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle = new NursecareSpawnerJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            }.Schedule(this, inputDeps);

            m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}
