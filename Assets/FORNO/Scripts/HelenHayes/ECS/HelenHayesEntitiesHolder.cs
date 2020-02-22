using Unity.Entities;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesEntitiesHolder : IComponentData
    {
        public Entity FrontHead;
        public Entity TopHead;
        public Entity RearHead;
        public Entity RightOffset;
        public Entity VSacral;
        public Entity LeftShoulder;
        public Entity LeftElbow;
        public Entity LeftWrist;
        public Entity RightElbow;
        public Entity RightShoulder;
        public Entity RightWrist;
        public Entity LeftAsis;
        public Entity LeftTight;
        public Entity LeftKnee;
        public Entity LeftKneeMed;
        public Entity LeftShank;
        public Entity LeftAnkle;
        public Entity LeftAnkleMed;
        public Entity LeftHeel;
        public Entity LeftToe;
        public Entity RightAsis;
        public Entity RightTight;
        public Entity RightKnee;
        public Entity RightKneeMed;
        public Entity RightShank;
        public Entity RightAnkle;
        public Entity RightAnkleMed;
        public Entity RightHeel;
        public Entity RightToe;
    }
}
