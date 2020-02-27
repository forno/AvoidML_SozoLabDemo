using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesPositionsEffector : IComponentData { }

    public class HelenHayesPositionsEffectorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var translationToEntity = GetComponentDataFromEntity<Translation>();
            Entities
                .WithNativeDisableContainerSafetyRestriction(translationToEntity)
                .WithAll<HelenHayesPositionsEffector>()
                .ForEach((in HelenHayesEntitiesHolder helenHayes, in HelenHayesPositions positions) =>
                {
                    if (!any(isnan(positions.FrontHead))) {
                        translationToEntity[helenHayes.FrontHead]     = new Translation { Value = positions.FrontHead };
                    }
                    if (!any(isnan(positions.TopHead))) {
                        translationToEntity[helenHayes.TopHead]       = new Translation { Value = positions.TopHead };
                    }
                    if (!any(isnan(positions.RearHead))) {
                        translationToEntity[helenHayes.RearHead]      = new Translation { Value = positions.RearHead };
                    }
                    if (!any(isnan(positions.RightOffset))) {
                        translationToEntity[helenHayes.RightOffset]   = new Translation { Value = positions.RightOffset };
                    }
                    if (!any(isnan(positions.VSacral))) {
                        translationToEntity[helenHayes.VSacral]       = new Translation { Value = positions.VSacral };
                    }
                    if (!any(isnan(positions.LeftShoulder))) {
                        translationToEntity[helenHayes.LeftShoulder]  = new Translation { Value = positions.LeftShoulder };
                    }
                    if (!any(isnan(positions.LeftElbow))) {
                        translationToEntity[helenHayes.LeftElbow]     = new Translation { Value = positions.LeftElbow };
                    }
                    if (!any(isnan(positions.LeftWrist))) {
                        translationToEntity[helenHayes.LeftWrist]     = new Translation { Value = positions.LeftWrist };
                    }
                    if (!any(isnan(positions.RightShoulder))) {
                        translationToEntity[helenHayes.RightShoulder] = new Translation { Value = positions.RightShoulder };
                    }
                    if (!any(isnan(positions.RightElbow))) {
                        translationToEntity[helenHayes.RightElbow]    = new Translation { Value = positions.RightElbow };
                    }
                    if (!any(isnan(positions.RightWrist))) {
                        translationToEntity[helenHayes.RightWrist]    = new Translation { Value = positions.RightWrist };
                    }
                    if (!any(isnan(positions.LeftAsis))) {
                        translationToEntity[helenHayes.LeftAsis]      = new Translation { Value = positions.LeftAsis };
                    }
                    if (!any(isnan(positions.LeftTight))) {
                        translationToEntity[helenHayes.LeftTight]     = new Translation { Value = positions.LeftTight };
                    }
                    if (!any(isnan(positions.LeftKnee))) {
                        translationToEntity[helenHayes.LeftKnee]      = new Translation { Value = positions.LeftKnee };
                    }
                    if (!any(isnan(positions.LeftKneeMed))) {
                        translationToEntity[helenHayes.LeftKneeMed]   = new Translation { Value = positions.LeftKneeMed };
                    }
                    if (!any(isnan(positions.LeftShank))) {
                        translationToEntity[helenHayes.LeftShank]     = new Translation { Value = positions.LeftShank };
                    }
                    if (!any(isnan(positions.LeftAnkle))) {
                        translationToEntity[helenHayes.LeftAnkle]     = new Translation { Value = positions.LeftAnkle };
                    }
                    if (!any(isnan(positions.LeftAnkleMed))) {
                        translationToEntity[helenHayes.LeftAnkleMed]  = new Translation { Value = positions.LeftAnkleMed };
                    }
                    if (!any(isnan(positions.LeftHeel))) {
                        translationToEntity[helenHayes.LeftHeel]      = new Translation { Value = positions.LeftHeel };
                    }
                    if (!any(isnan(positions.LeftToe))) {
                        translationToEntity[helenHayes.LeftToe]       = new Translation { Value = positions.LeftToe };
                    }
                    if (!any(isnan(positions.RightTight))) {
                        translationToEntity[helenHayes.RightTight]    = new Translation { Value = positions.RightTight };
                    }
                    if (!any(isnan(positions.RightAsis))) {
                        translationToEntity[helenHayes.RightAsis]     = new Translation { Value = positions.RightAsis };
                    }
                    if (!any(isnan(positions.RightKnee))) {
                        translationToEntity[helenHayes.RightKnee]     = new Translation { Value = positions.RightKnee };
                    }
                    if (!any(isnan(positions.RightKneeMed))) {
                        translationToEntity[helenHayes.RightKneeMed]  = new Translation { Value = positions.RightKneeMed };
                    }
                    if (!any(isnan(positions.RightShank))) {
                        translationToEntity[helenHayes.RightShank]    = new Translation { Value = positions.RightShank };
                    }
                    if (!any(isnan(positions.RightAnkle))) {
                        translationToEntity[helenHayes.RightAnkle]    = new Translation { Value = positions.RightAnkle };
                    }
                    if (!any(isnan(positions.RightAnkleMed))) {
                        translationToEntity[helenHayes.RightAnkleMed] = new Translation { Value = positions.RightAnkleMed };
                    }
                    if (!any(isnan(positions.RightHeel))) {
                        translationToEntity[helenHayes.RightHeel]     = new Translation { Value = positions.RightHeel };
                    }
                    if (!any(isnan(positions.RightToe))) {
                        translationToEntity[helenHayes.RightToe]      = new Translation { Value = positions.RightToe };
                    }
                }).ScheduleParallel();
        }
    }
}
