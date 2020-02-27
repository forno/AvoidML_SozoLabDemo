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
                    var intervalTime = 1f / frequency.Value;
                    velocities.FrontHead     = (sequentialData[alignIndex     ] - sequentialData[alignLastIndex     ]) * intervalTime;
                    velocities.TopHead       = (sequentialData[alignIndex +  1] - sequentialData[alignLastIndex +  1]) * intervalTime;
                    velocities.RearHead      = (sequentialData[alignIndex +  2] - sequentialData[alignLastIndex +  2]) * intervalTime;
                    velocities.RightOffset   = (sequentialData[alignIndex +  3] - sequentialData[alignLastIndex +  3]) * intervalTime;
                    velocities.VSacral       = (sequentialData[alignIndex +  4] - sequentialData[alignLastIndex +  4]) * intervalTime;
                    velocities.LeftShoulder  = (sequentialData[alignIndex +  5] - sequentialData[alignLastIndex +  5]) * intervalTime;
                    velocities.LeftElbow     = (sequentialData[alignIndex +  6] - sequentialData[alignLastIndex +  6]) * intervalTime;
                    velocities.LeftWrist     = (sequentialData[alignIndex +  7] - sequentialData[alignLastIndex +  7]) * intervalTime;
                    velocities.RightElbow    = (sequentialData[alignIndex +  8] - sequentialData[alignLastIndex +  8]) * intervalTime;
                    velocities.RightShoulder = (sequentialData[alignIndex +  9] - sequentialData[alignLastIndex +  9]) * intervalTime;
                    velocities.RightWrist    = (sequentialData[alignIndex + 10] - sequentialData[alignLastIndex + 10]) * intervalTime;
                    velocities.LeftAsis      = (sequentialData[alignIndex + 11] - sequentialData[alignLastIndex + 11]) * intervalTime;
                    velocities.LeftTight     = (sequentialData[alignIndex + 12] - sequentialData[alignLastIndex + 12]) * intervalTime;
                    velocities.LeftKnee      = (sequentialData[alignIndex + 13] - sequentialData[alignLastIndex + 13]) * intervalTime;
                    velocities.LeftKneeMed   = (sequentialData[alignIndex + 14] - sequentialData[alignLastIndex + 14]) * intervalTime;
                    velocities.LeftShank     = (sequentialData[alignIndex + 15] - sequentialData[alignLastIndex + 15]) * intervalTime;
                    velocities.LeftAnkle     = (sequentialData[alignIndex + 16] - sequentialData[alignLastIndex + 16]) * intervalTime;
                    velocities.LeftAnkleMed  = (sequentialData[alignIndex + 17] - sequentialData[alignLastIndex + 17]) * intervalTime;
                    velocities.LeftHeel      = (sequentialData[alignIndex + 18] - sequentialData[alignLastIndex + 18]) * intervalTime;
                    velocities.LeftToe       = (sequentialData[alignIndex + 19] - sequentialData[alignLastIndex + 19]) * intervalTime;
                    velocities.RightTight    = (sequentialData[alignIndex + 20] - sequentialData[alignLastIndex + 20]) * intervalTime;
                    velocities.RightAsis     = (sequentialData[alignIndex + 21] - sequentialData[alignLastIndex + 21]) * intervalTime;
                    velocities.RightKnee     = (sequentialData[alignIndex + 22] - sequentialData[alignLastIndex + 22]) * intervalTime;
                    velocities.RightKneeMed  = (sequentialData[alignIndex + 23] - sequentialData[alignLastIndex + 23]) * intervalTime;
                    velocities.RightShank    = (sequentialData[alignIndex + 24] - sequentialData[alignLastIndex + 24]) * intervalTime;
                    velocities.RightAnkle    = (sequentialData[alignIndex + 25] - sequentialData[alignLastIndex + 25]) * intervalTime;
                    velocities.RightAnkleMed = (sequentialData[alignIndex + 26] - sequentialData[alignLastIndex + 26]) * intervalTime;
                    velocities.RightHeel     = (sequentialData[alignIndex + 27] - sequentialData[alignLastIndex + 27]) * intervalTime;
                    velocities.RightToe      = (sequentialData[alignIndex + 28] - sequentialData[alignLastIndex + 28]) * intervalTime;
                }).ScheduleParallel();
        }
    }
}
