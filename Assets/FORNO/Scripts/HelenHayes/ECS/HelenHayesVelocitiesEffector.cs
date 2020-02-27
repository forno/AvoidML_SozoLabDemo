using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using static Unity.Mathematics.math;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesVelocitiesEffector : IComponentData { }

    public class HelenHayesPositionsVelocitiesEffectorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var velocityToEntity = GetComponentDataFromEntity<PhysicsVelocity>();
            Entities
                .WithNativeDisableContainerSafetyRestriction(velocityToEntity)
                .WithAll<HelenHayesVelocitiesEffector>()
                .ForEach((in HelenHayesEntitiesHolder helenHayes, in HelenHayesVelocities velocities) =>
                {
                    if (!any(isnan(velocities.FrontHead))) {
                        velocityToEntity[helenHayes.FrontHead]     = new PhysicsVelocity { Linear = velocities.FrontHead };
                    }
                    if (!any(isnan(velocities.TopHead))) {
                        velocityToEntity[helenHayes.TopHead]       = new PhysicsVelocity { Linear = velocities.TopHead };
                    }
                    if (!any(isnan(velocities.RearHead))) {
                        velocityToEntity[helenHayes.RearHead]      = new PhysicsVelocity { Linear = velocities.RearHead };
                    }
                    if (!any(isnan(velocities.RightOffset))) {
                        velocityToEntity[helenHayes.RightOffset]   = new PhysicsVelocity { Linear = velocities.RightOffset };
                    }
                    if (!any(isnan(velocities.VSacral))) {
                        velocityToEntity[helenHayes.VSacral]       = new PhysicsVelocity { Linear = velocities.VSacral };
                    }
                    if (!any(isnan(velocities.LeftShoulder))) {
                        velocityToEntity[helenHayes.LeftShoulder]  = new PhysicsVelocity { Linear = velocities.LeftShoulder };
                    }
                    if (!any(isnan(velocities.LeftElbow))) {
                        velocityToEntity[helenHayes.LeftElbow]     = new PhysicsVelocity { Linear = velocities.LeftElbow };
                    }
                    if (!any(isnan(velocities.LeftWrist))) {
                        velocityToEntity[helenHayes.LeftWrist]     = new PhysicsVelocity { Linear = velocities.LeftWrist };
                    }
                    if (!any(isnan(velocities.RightShoulder))) {
                        velocityToEntity[helenHayes.RightShoulder] = new PhysicsVelocity { Linear = velocities.RightShoulder };
                    }
                    if (!any(isnan(velocities.RightElbow))) {
                        velocityToEntity[helenHayes.RightElbow]    = new PhysicsVelocity { Linear = velocities.RightElbow };
                    }
                    if (!any(isnan(velocities.RightWrist))) {
                        velocityToEntity[helenHayes.RightWrist]    = new PhysicsVelocity { Linear = velocities.RightWrist };
                    }
                    if (!any(isnan(velocities.LeftAsis))) {
                        velocityToEntity[helenHayes.LeftAsis]      = new PhysicsVelocity { Linear = velocities.LeftAsis };
                    }
                    if (!any(isnan(velocities.LeftTight))) {
                        velocityToEntity[helenHayes.LeftTight]     = new PhysicsVelocity { Linear = velocities.LeftTight };
                    }
                    if (!any(isnan(velocities.LeftKnee))) {
                        velocityToEntity[helenHayes.LeftKnee]      = new PhysicsVelocity { Linear = velocities.LeftKnee };
                    }
                    if (!any(isnan(velocities.LeftKneeMed))) {
                        velocityToEntity[helenHayes.LeftKneeMed]   = new PhysicsVelocity { Linear = velocities.LeftKneeMed };
                    }
                    if (!any(isnan(velocities.LeftShank))) {
                        velocityToEntity[helenHayes.LeftShank]     = new PhysicsVelocity { Linear = velocities.LeftShank };
                    }
                    if (!any(isnan(velocities.LeftAnkle))) {
                        velocityToEntity[helenHayes.LeftAnkle]     = new PhysicsVelocity { Linear = velocities.LeftAnkle };
                    }
                    if (!any(isnan(velocities.LeftAnkleMed))) {
                        velocityToEntity[helenHayes.LeftAnkleMed]  = new PhysicsVelocity { Linear = velocities.LeftAnkleMed };
                    }
                    if (!any(isnan(velocities.LeftHeel))) {
                        velocityToEntity[helenHayes.LeftHeel]      = new PhysicsVelocity { Linear = velocities.LeftHeel };
                    }
                    if (!any(isnan(velocities.LeftToe))) {
                        velocityToEntity[helenHayes.LeftToe]       = new PhysicsVelocity { Linear = velocities.LeftToe };
                    }
                    if (!any(isnan(velocities.RightTight))) {
                        velocityToEntity[helenHayes.RightTight]    = new PhysicsVelocity { Linear = velocities.RightTight };
                    }
                    if (!any(isnan(velocities.RightAsis))) {
                        velocityToEntity[helenHayes.RightAsis]     = new PhysicsVelocity { Linear = velocities.RightAsis };
                    }
                    if (!any(isnan(velocities.RightKnee))) {
                        velocityToEntity[helenHayes.RightKnee]     = new PhysicsVelocity { Linear = velocities.RightKnee };
                    }
                    if (!any(isnan(velocities.RightKneeMed))) {
                        velocityToEntity[helenHayes.RightKneeMed]  = new PhysicsVelocity { Linear = velocities.RightKneeMed };
                    }
                    if (!any(isnan(velocities.RightShank))) {
                        velocityToEntity[helenHayes.RightShank]    = new PhysicsVelocity { Linear = velocities.RightShank };
                    }
                    if (!any(isnan(velocities.RightAnkle))) {
                        velocityToEntity[helenHayes.RightAnkle]    = new PhysicsVelocity { Linear = velocities.RightAnkle };
                    }
                    if (!any(isnan(velocities.RightAnkleMed))) {
                        velocityToEntity[helenHayes.RightAnkleMed] = new PhysicsVelocity { Linear = velocities.RightAnkleMed };
                    }
                    if (!any(isnan(velocities.RightHeel))) {
                        velocityToEntity[helenHayes.RightHeel]     = new PhysicsVelocity { Linear = velocities.RightHeel };
                    }
                    if (!any(isnan(velocities.RightToe))) {
                        velocityToEntity[helenHayes.RightToe]      = new PhysicsVelocity { Linear = velocities.RightToe };
                    }
                }).ScheduleParallel();
        }
    }
}
