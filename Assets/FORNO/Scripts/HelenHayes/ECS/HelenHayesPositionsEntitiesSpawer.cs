using Unity.Entities;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesPositionsEntitiesSpawer : IComponentData
    {
        public Entity Prefab;
    }

    public class HelenHayesPositionsEntitiesSpawerSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem system;

        protected override void OnCreate()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = system.CreateCommandBuffer().ToConcurrent();
            Entities
                .WithNone<HelenHayesEntitiesHolder>()
                .ForEach((Entity e, int entityInQueryIndex, in HelenHayesPositionsEntitiesSpawer spawer) =>
                {
                    commandBuffer.AddComponent(entityInQueryIndex, e, new HelenHayesEntitiesHolder
                    {
                        FrontHead     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        TopHead       = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RearHead      = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightOffset   = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        VSacral       = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftShoulder  = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftElbow     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftWrist     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightShoulder = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightElbow    = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightWrist    = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftAsis      = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftTight     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftKnee      = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftKneeMed   = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftShank     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftAnkle     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftAnkleMed  = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftHeel      = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        LeftToe       = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightAsis     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightTight    = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightKnee     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightKneeMed  = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightShank    = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightAnkle    = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightAnkleMed = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightHeel     = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                        RightToe      = commandBuffer.Instantiate(entityInQueryIndex, spawer.Prefab),
                    });
                }).ScheduleParallel();
            system.AddJobHandleForProducer(Dependency);
        }
    }
}
