using Forno.Ecs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesPositions : IComponentData
    {
        public float3 FrontHead;
        public float3 TopHead;
        public float3 RearHead;
        public float3 RightOffset;
        public float3 VSacral;
        public float3 LeftShoulder;
        public float3 LeftElbow;
        public float3 LeftWrist;
        public float3 RightShoulder;
        public float3 RightElbow;
        public float3 RightWrist;
        public float3 LeftAsis;
        public float3 LeftTight;
        public float3 LeftKnee;
        public float3 LeftKneeMed;
        public float3 LeftShank;
        public float3 LeftAnkle;
        public float3 LeftAnkleMed;
        public float3 LeftHeel;
        public float3 LeftToe;
        public float3 RightAsis;
        public float3 RightTight;
        public float3 RightKnee;
        public float3 RightKneeMed;
        public float3 RightShank;
        public float3 RightAnkle;
        public float3 RightAnkleMed;
        public float3 RightHeel;
        public float3 RightToe;
    }

    public class HelenHayesPositionsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref HelenHayesPositions positions, in SequentialHelenHayesData data, in SequenceIndex index, in SequenceTimeFrac frac, in Translation translation) =>
                {
                    ref var sequentialData = ref data.BlobData.Value.Positions;
                    var alignIndex = clamp(Constants.PositionCount * (index.Value + 1), 0, sequentialData.Length - Constants.PositionCount);
                    var alignLastIndex = clamp(Constants.PositionCount * index.Value, 0, sequentialData.Length - Constants.PositionCount);
                    positions.FrontHead     = lerp(sequentialData[alignLastIndex + Constants.FrontHeadIndex    ], sequentialData[alignIndex + Constants.FrontHeadIndex    ], frac.Value) + translation.Value;
                    positions.TopHead       = lerp(sequentialData[alignLastIndex + Constants.TopHeadIndex      ], sequentialData[alignIndex + Constants.TopHeadIndex      ], frac.Value) + translation.Value;
                    positions.RearHead      = lerp(sequentialData[alignLastIndex + Constants.RearHeadIndex     ], sequentialData[alignIndex + Constants.RearHeadIndex     ], frac.Value) + translation.Value;
                    positions.RightOffset   = lerp(sequentialData[alignLastIndex + Constants.RightOffsetIndex  ], sequentialData[alignIndex + Constants.RightOffsetIndex  ], frac.Value) + translation.Value;
                    positions.VSacral       = lerp(sequentialData[alignLastIndex + Constants.VSacralIndex      ], sequentialData[alignIndex + Constants.VSacralIndex      ], frac.Value) + translation.Value;
                    positions.LeftShoulder  = lerp(sequentialData[alignLastIndex + Constants.LeftShoulderIndex ], sequentialData[alignIndex + Constants.LeftShoulderIndex ], frac.Value) + translation.Value;
                    positions.LeftElbow     = lerp(sequentialData[alignLastIndex + Constants.LeftElbowIndex    ], sequentialData[alignIndex + Constants.LeftElbowIndex    ], frac.Value) + translation.Value;
                    positions.LeftWrist     = lerp(sequentialData[alignLastIndex + Constants.LeftWristIndex    ], sequentialData[alignIndex + Constants.LeftWristIndex    ], frac.Value) + translation.Value;
                    positions.RightShoulder = lerp(sequentialData[alignLastIndex + Constants.RightShoulderIndex], sequentialData[alignIndex + Constants.RightShoulderIndex], frac.Value) + translation.Value;
                    positions.RightElbow    = lerp(sequentialData[alignLastIndex + Constants.RightElbowIndex   ], sequentialData[alignIndex + Constants.RightElbowIndex   ], frac.Value) + translation.Value;
                    positions.RightWrist    = lerp(sequentialData[alignLastIndex + Constants.RightWristIndex   ], sequentialData[alignIndex + Constants.RightWristIndex   ], frac.Value) + translation.Value;
                    positions.LeftAsis      = lerp(sequentialData[alignLastIndex + Constants.LeftAsisIndex     ], sequentialData[alignIndex + Constants.LeftAsisIndex     ], frac.Value) + translation.Value;
                    positions.LeftTight     = lerp(sequentialData[alignLastIndex + Constants.LeftTightIndex    ], sequentialData[alignIndex + Constants.LeftTightIndex    ], frac.Value) + translation.Value;
                    positions.LeftKnee      = lerp(sequentialData[alignLastIndex + Constants.LeftKneeIndex     ], sequentialData[alignIndex + Constants.LeftKneeIndex     ], frac.Value) + translation.Value;
                    positions.LeftKneeMed   = lerp(sequentialData[alignLastIndex + Constants.LeftKneeMedIndex  ], sequentialData[alignIndex + Constants.LeftKneeMedIndex  ], frac.Value) + translation.Value;
                    positions.LeftShank     = lerp(sequentialData[alignLastIndex + Constants.LeftShankIndex    ], sequentialData[alignIndex + Constants.LeftShankIndex    ], frac.Value) + translation.Value;
                    positions.LeftAnkle     = lerp(sequentialData[alignLastIndex + Constants.LeftAnkleIndex    ], sequentialData[alignIndex + Constants.LeftAnkleIndex    ], frac.Value) + translation.Value;
                    positions.LeftAnkleMed  = lerp(sequentialData[alignLastIndex + Constants.LeftAnkleMedIndex ], sequentialData[alignIndex + Constants.LeftAnkleMedIndex ], frac.Value) + translation.Value;
                    positions.LeftHeel      = lerp(sequentialData[alignLastIndex + Constants.LeftHeelIndex     ], sequentialData[alignIndex + Constants.LeftHeelIndex     ], frac.Value) + translation.Value;
                    positions.LeftToe       = lerp(sequentialData[alignLastIndex + Constants.LeftToeIndex      ], sequentialData[alignIndex + Constants.LeftToeIndex      ], frac.Value) + translation.Value;
                    positions.RightTight    = lerp(sequentialData[alignLastIndex + Constants.RightTightIndex   ], sequentialData[alignIndex + Constants.RightTightIndex   ], frac.Value) + translation.Value;
                    positions.RightAsis     = lerp(sequentialData[alignLastIndex + Constants.RightAsisIndex    ], sequentialData[alignIndex + Constants.RightAsisIndex    ], frac.Value) + translation.Value;
                    positions.RightKnee     = lerp(sequentialData[alignLastIndex + Constants.RightKneeIndex    ], sequentialData[alignIndex + Constants.RightKneeIndex    ], frac.Value) + translation.Value;
                    positions.RightKneeMed  = lerp(sequentialData[alignLastIndex + Constants.RightKneeMedIndex ], sequentialData[alignIndex + Constants.RightKneeMedIndex ], frac.Value) + translation.Value;
                    positions.RightShank    = lerp(sequentialData[alignLastIndex + Constants.RightShankIndex   ], sequentialData[alignIndex + Constants.RightShankIndex   ], frac.Value) + translation.Value;
                    positions.RightAnkle    = lerp(sequentialData[alignLastIndex + Constants.RightAnkleIndex   ], sequentialData[alignIndex + Constants.RightAnkleIndex   ], frac.Value) + translation.Value;
                    positions.RightAnkleMed = lerp(sequentialData[alignLastIndex + Constants.RightAnkleMedIndex], sequentialData[alignIndex + Constants.RightAnkleMedIndex], frac.Value) + translation.Value;
                    positions.RightHeel     = lerp(sequentialData[alignLastIndex + Constants.RightHeelIndex    ], sequentialData[alignIndex + Constants.RightHeelIndex    ], frac.Value) + translation.Value;
                    positions.RightToe      = lerp(sequentialData[alignLastIndex + Constants.RightToeIndex     ], sequentialData[alignIndex + Constants.RightToeIndex     ], frac.Value) + translation.Value;
                }).ScheduleParallel();
        }
    }
}
