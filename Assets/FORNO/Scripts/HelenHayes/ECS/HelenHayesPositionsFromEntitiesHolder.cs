using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesPositionsFromEntitiesHolder : IComponentData { }

    public class HelenHayesPositionsFromEntitiesHolderSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var translationFromEntity = GetComponentDataFromEntity<Translation>(true);

            Entities
                .WithReadOnly(translationFromEntity)
                .WithAll<HelenHayesPositionsFromEntitiesHolder>()
                .ForEach((ref HelenHayesPositions positions, in HelenHayesEntitiesHolder helenHayes) =>
                {
                    positions.FrontHead = translationFromEntity[helenHayes.FrontHead].Value;
                    positions.TopHead = translationFromEntity[helenHayes.TopHead].Value;
                    positions.RearHead = translationFromEntity[helenHayes.RearHead].Value;
                    positions.RightOffset = translationFromEntity[helenHayes.RightOffset].Value;
                    positions.VSacral = translationFromEntity[helenHayes.VSacral].Value;
                    positions.LeftShoulder = translationFromEntity[helenHayes.LeftShoulder].Value;
                    positions.LeftElbow = translationFromEntity[helenHayes.LeftElbow].Value;
                    positions.LeftWrist = translationFromEntity[helenHayes.LeftWrist].Value;
                    positions.RightShoulder = translationFromEntity[helenHayes.RightShoulder].Value;
                    positions.RightElbow = translationFromEntity[helenHayes.RightElbow].Value;
                    positions.RightWrist = translationFromEntity[helenHayes.RightWrist].Value;
                    positions.LeftAsis = translationFromEntity[helenHayes.LeftAsis].Value;
                    positions.LeftTight = translationFromEntity[helenHayes.LeftTight].Value;
                    positions.LeftKnee = translationFromEntity[helenHayes.LeftKnee].Value;
                    positions.LeftKneeMed = translationFromEntity[helenHayes.LeftKneeMed].Value;
                    positions.LeftShank = translationFromEntity[helenHayes.LeftShank].Value;
                    positions.LeftAnkle = translationFromEntity[helenHayes.LeftAnkle].Value;
                    positions.LeftAnkleMed = translationFromEntity[helenHayes.LeftAnkleMed].Value;
                    positions.LeftHeel = translationFromEntity[helenHayes.LeftHeel].Value;
                    positions.LeftToe = translationFromEntity[helenHayes.LeftToe].Value;
                    positions.RightTight = translationFromEntity[helenHayes.RightTight].Value;
                    positions.RightAsis = translationFromEntity[helenHayes.RightAsis].Value;
                    positions.RightKnee = translationFromEntity[helenHayes.RightKnee].Value;
                    positions.RightKneeMed = translationFromEntity[helenHayes.RightKneeMed].Value;
                    positions.RightShank = translationFromEntity[helenHayes.RightShank].Value;
                    positions.RightAnkle = translationFromEntity[helenHayes.RightAnkle].Value;
                    positions.RightAnkleMed = translationFromEntity[helenHayes.RightAnkleMed].Value;
                    positions.RightHeel = translationFromEntity[helenHayes.RightHeel].Value;
                    positions.RightToe = translationFromEntity[helenHayes.RightToe].Value;
                }).ScheduleParallel();
        }
    }
}
