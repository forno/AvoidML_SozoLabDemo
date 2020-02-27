using Unity.Entities;
using Unity.Jobs;
using static Unity.Mathematics.math;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesEnabledEffector : IComponentData { }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class HelenHayesEnabledEffectorSystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem system;

        protected override void OnCreate()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = system.CreateCommandBuffer().ToConcurrent();
            Entities
                .WithAll<HelenHayesEnabledEffector>()
                .ForEach((int entityInQueryIndex, in HelenHayesEntitiesHolder helenHayes, in HelenHayesPositions positions) =>
                {
                    if (any(isnan(positions.FrontHead))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.FrontHead);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.FrontHead);
                    }
                    if (any(isnan(positions.TopHead))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.TopHead);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.TopHead);
                    }
                    if (any(isnan(positions.RearHead))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RearHead);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RearHead);
                    }
                    if (any(isnan(positions.RightOffset))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightOffset);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightOffset);
                    }
                    if (any(isnan(positions.VSacral))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.VSacral);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.VSacral);
                    }
                    if (any(isnan(positions.LeftShoulder))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftShoulder);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftShoulder);
                    }
                    if (any(isnan(positions.LeftElbow))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftElbow);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftElbow);
                    }
                    if (any(isnan(positions.LeftWrist))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftWrist);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftWrist);
                    }
                    if (any(isnan(positions.RightShoulder))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightShoulder);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightShoulder);
                    }
                    if (any(isnan(positions.RightElbow))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightElbow);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightElbow);
                    }
                    if (any(isnan(positions.RightWrist))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightWrist);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightWrist);
                    }
                    if (any(isnan(positions.LeftAsis))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAsis);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAsis);
                    }
                    if (any(isnan(positions.LeftTight))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftTight);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftTight);
                    }
                    if (any(isnan(positions.LeftKnee))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftKnee);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftKnee);
                    }
                    if (any(isnan(positions.LeftKneeMed))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftKneeMed);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftKneeMed);
                    }
                    if (any(isnan(positions.LeftShank))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftShank);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftShank);
                    }
                    if (any(isnan(positions.LeftAnkle))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAnkle);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAnkle);
                    }
                    if (any(isnan(positions.LeftAnkleMed))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAnkleMed);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftAnkleMed);
                    }
                    if (any(isnan(positions.LeftHeel))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftHeel);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftHeel);
                    }
                    if (any(isnan(positions.LeftToe))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.LeftToe);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.LeftToe);
                    }
                    if (any(isnan(positions.RightTight))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightTight);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightTight);
                    }
                    if (any(isnan(positions.RightAsis))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightAsis);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightAsis);
                    }
                    if (any(isnan(positions.RightKnee))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightKnee);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightKnee);
                    }
                    if (any(isnan(positions.RightKneeMed))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightKneeMed);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightKneeMed);
                    }
                    if (any(isnan(positions.RightShank))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightShank);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightShank);
                    }
                    if (any(isnan(positions.RightAnkle))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightAnkle);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightAnkle);
                    }
                    if (any(isnan(positions.RightAnkleMed))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightAnkleMed);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightAnkleMed);
                    }
                    if (any(isnan(positions.RightHeel))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightHeel);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightHeel);
                    }
                    if (any(isnan(positions.RightToe))) {
                        commandBuffer.AddComponent<Disabled>(entityInQueryIndex, helenHayes.RightToe);
                    } else {
                        commandBuffer.RemoveComponent<Disabled>(entityInQueryIndex, helenHayes.RightToe);
                    }
                }).ScheduleParallel();
            system.AddJobHandleForProducer(Dependency);
        }
    }
}
