using Forno.Ecs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesVelocities : IComponentData
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

    public class HelenHayesVelocitysSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // From SequantialHelenHayseVelocitys
            Entities
                .ForEach((ref HelenHayesVelocities velocities, in SequentialHelenHayesData data, in SequenceIndex index, in SequenceFrequency frequency) =>
                {
                    ref var sequentialData = ref data.BlobData.Value.Positions;
                    var alignIndex = clamp(Constants.PositionCount * (index.Value + 1), 0, sequentialData.Length - Constants.PositionCount);
                    var alignLastIndex = clamp(Constants.PositionCount * index.Value, 0, sequentialData.Length - Constants.PositionCount);
                    velocities.FrontHead     = (sequentialData[alignIndex     ] - sequentialData[alignLastIndex     ]) * frequency.Value;
                    velocities.TopHead       = (sequentialData[alignIndex +  1] - sequentialData[alignLastIndex +  1]) * frequency.Value;
                    velocities.RearHead      = (sequentialData[alignIndex +  2] - sequentialData[alignLastIndex +  2]) * frequency.Value;
                    velocities.RightOffset   = (sequentialData[alignIndex +  3] - sequentialData[alignLastIndex +  3]) * frequency.Value;
                    velocities.VSacral       = (sequentialData[alignIndex +  4] - sequentialData[alignLastIndex +  4]) * frequency.Value;
                    velocities.LeftShoulder  = (sequentialData[alignIndex +  5] - sequentialData[alignLastIndex +  5]) * frequency.Value;
                    velocities.LeftElbow     = (sequentialData[alignIndex +  6] - sequentialData[alignLastIndex +  6]) * frequency.Value;
                    velocities.LeftWrist     = (sequentialData[alignIndex +  7] - sequentialData[alignLastIndex +  7]) * frequency.Value;
                    velocities.RightElbow    = (sequentialData[alignIndex +  8] - sequentialData[alignLastIndex +  8]) * frequency.Value;
                    velocities.RightShoulder = (sequentialData[alignIndex +  9] - sequentialData[alignLastIndex +  9]) * frequency.Value;
                    velocities.RightWrist    = (sequentialData[alignIndex + 10] - sequentialData[alignLastIndex + 10]) * frequency.Value;
                    velocities.LeftAsis      = (sequentialData[alignIndex + 11] - sequentialData[alignLastIndex + 11]) * frequency.Value;
                    velocities.LeftTight     = (sequentialData[alignIndex + 12] - sequentialData[alignLastIndex + 12]) * frequency.Value;
                    velocities.LeftKnee      = (sequentialData[alignIndex + 13] - sequentialData[alignLastIndex + 13]) * frequency.Value;
                    velocities.LeftKneeMed   = (sequentialData[alignIndex + 14] - sequentialData[alignLastIndex + 14]) * frequency.Value;
                    velocities.LeftShank     = (sequentialData[alignIndex + 15] - sequentialData[alignLastIndex + 15]) * frequency.Value;
                    velocities.LeftAnkle     = (sequentialData[alignIndex + 16] - sequentialData[alignLastIndex + 16]) * frequency.Value;
                    velocities.LeftAnkleMed  = (sequentialData[alignIndex + 17] - sequentialData[alignLastIndex + 17]) * frequency.Value;
                    velocities.LeftHeel      = (sequentialData[alignIndex + 18] - sequentialData[alignLastIndex + 18]) * frequency.Value;
                    velocities.LeftToe       = (sequentialData[alignIndex + 19] - sequentialData[alignLastIndex + 19]) * frequency.Value;
                    velocities.RightTight    = (sequentialData[alignIndex + 20] - sequentialData[alignLastIndex + 20]) * frequency.Value;
                    velocities.RightAsis     = (sequentialData[alignIndex + 21] - sequentialData[alignLastIndex + 21]) * frequency.Value;
                    velocities.RightKnee     = (sequentialData[alignIndex + 22] - sequentialData[alignLastIndex + 22]) * frequency.Value;
                    velocities.RightKneeMed  = (sequentialData[alignIndex + 23] - sequentialData[alignLastIndex + 23]) * frequency.Value;
                    velocities.RightShank    = (sequentialData[alignIndex + 24] - sequentialData[alignLastIndex + 24]) * frequency.Value;
                    velocities.RightAnkle    = (sequentialData[alignIndex + 25] - sequentialData[alignLastIndex + 25]) * frequency.Value;
                    velocities.RightAnkleMed = (sequentialData[alignIndex + 26] - sequentialData[alignLastIndex + 26]) * frequency.Value;
                    velocities.RightHeel     = (sequentialData[alignIndex + 27] - sequentialData[alignLastIndex + 27]) * frequency.Value;
                    velocities.RightToe      = (sequentialData[alignIndex + 28] - sequentialData[alignLastIndex + 28]) * frequency.Value;
                }).ScheduleParallel();
        }
    }
}
