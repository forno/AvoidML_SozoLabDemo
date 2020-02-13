using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
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
        Entity parent;

        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            parent = World.EntityManager.CreateEntity(typeof(LocalToWorld));
            World.EntityManager.SetComponentData(parent, new LocalToWorld { Value = float4x4.identity });
        }

        [BurstCompile(CompileSynchronously = true)]
        struct NursecareSpawnerJob : IJobForEachWithEntity<NursecareSpawner>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public Entity Parent;

            public void Execute(Entity entity, int index, [ReadOnly] ref NursecareSpawner nursecareSpawner)
            {
                for (int i = 0; i < Constants.positionCount; i++) {
                    var instance = CommandBuffer.Instantiate(index, nursecareSpawner.Prefab);
                    CommandBuffer.SetComponent(index, instance, new NursecareData { Index = i });
                    CommandBuffer.AddComponent(index, instance, new Parent { Value = Parent });
                    CommandBuffer.AddComponent<LocalToParent>(index, instance);
                }

                // Delete Spawner
                CommandBuffer.DestroyEntity(index, entity);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle = new NursecareSpawnerJob
            {
                CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                Parent = parent
            }.Schedule(this, inputDeps);

            m_EntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}
