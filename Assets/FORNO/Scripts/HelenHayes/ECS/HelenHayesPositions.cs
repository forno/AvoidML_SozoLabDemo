using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesPositions : IComponentData
    {
        public float3 FrontHead;
        public float3 TopHead;
        public float3 RearHead;
        public float3 LeftShoulder;
        public float3 RightShoulder;
        public float3 RightOffset;
        public float3 LeftElbow;
        public float3 RightElbow;
        public float3 LeftWrist;
        public float3 RightWrist;
        public float3 LeftAsis;
        public float3 RightAsis;
        public float3 VSacral;
        public float3 LeftTight;
        public float3 RightTight;
        public float3 LeftKnee;
        public float3 RightKnee;
        public float3 LeftKneeMed;
        public float3 RightKneeMed;
        public float3 LeftShank;
        public float3 RightShank;
        public float3 LeftAnkle;
        public float3 RightAnkle;
        public float3 LeftAnkleMed;
        public float3 RightAnkleMed;
        public float3 LeftToe;
        public float3 RightToe;
        public float3 LeftToeMed;
        public float3 RightToeMed;
    }

    public class HelenHayesPositionsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var translationFromEntity = GetComponentDataFromEntity<Translation>(true);

            Entities
                .WithReadOnly(translationFromEntity)
                .ForEach((ref HelenHayesPositions positions, in HelenHayesEntitiesHolder helenHayes) =>
                {
                    positions.FrontHead     = translationFromEntity[helenHayes.FrontHead    ].Value;
                    positions.TopHead       = translationFromEntity[helenHayes.TopHead      ].Value;
                    positions.RearHead      = translationFromEntity[helenHayes.RearHead     ].Value;
                    positions.LeftShoulder  = translationFromEntity[helenHayes.LeftShoulder ].Value;
                    positions.RightShoulder = translationFromEntity[helenHayes.RightShoulder].Value;
                    positions.RightOffset   = translationFromEntity[helenHayes.RightOffset  ].Value;
                    positions.LeftElbow     = translationFromEntity[helenHayes.LeftElbow    ].Value;
                    positions.RightElbow    = translationFromEntity[helenHayes.RightElbow   ].Value;
                    positions.LeftWrist     = translationFromEntity[helenHayes.LeftWrist    ].Value;
                    positions.RightWrist    = translationFromEntity[helenHayes.RightWrist   ].Value;
                    positions.LeftAsis      = translationFromEntity[helenHayes.LeftAsis     ].Value;
                    positions.RightAsis     = translationFromEntity[helenHayes.RightAsis    ].Value;
                    positions.VSacral       = translationFromEntity[helenHayes.VSacral      ].Value;
                    positions.LeftTight     = translationFromEntity[helenHayes.LeftTight    ].Value;
                    positions.RightTight    = translationFromEntity[helenHayes.RightTight   ].Value;
                    positions.LeftKnee      = translationFromEntity[helenHayes.LeftKnee     ].Value;
                    positions.RightKnee     = translationFromEntity[helenHayes.RightKnee    ].Value;
                    positions.LeftKneeMed   = translationFromEntity[helenHayes.LeftKneeMed  ].Value;
                    positions.RightKneeMed  = translationFromEntity[helenHayes.RightKneeMed ].Value;
                    positions.LeftShank     = translationFromEntity[helenHayes.LeftShank    ].Value;
                    positions.RightShank    = translationFromEntity[helenHayes.RightShank   ].Value;
                    positions.LeftAnkle     = translationFromEntity[helenHayes.LeftAnkle    ].Value;
                    positions.RightAnkle    = translationFromEntity[helenHayes.RightAnkle   ].Value;
                    positions.LeftAnkleMed  = translationFromEntity[helenHayes.LeftAnkleMed ].Value;
                    positions.RightAnkleMed = translationFromEntity[helenHayes.RightAnkleMed].Value;
                    positions.LeftToe       = translationFromEntity[helenHayes.LeftToe      ].Value;
                    positions.RightToe      = translationFromEntity[helenHayes.RightToe     ].Value;
                    positions.LeftToeMed    = translationFromEntity[helenHayes.LeftToeMed   ].Value;
                    positions.RightToeMed   = translationFromEntity[helenHayes.RightToeMed  ].Value;
                }).ScheduleParallel();
        }
    }
}
