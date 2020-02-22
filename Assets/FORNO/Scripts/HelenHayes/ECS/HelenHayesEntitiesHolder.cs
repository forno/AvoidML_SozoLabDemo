using Unity.Entities;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesEntitiesHolder : IComponentData
    {
        public Entity FrontHead;
        public Entity TopHead;
        public Entity RearHead;
        public Entity LeftShoulder;
        public Entity RightShoulder;
        public Entity RightOffset;
        public Entity LeftElbow;
        public Entity RightElbow;
        public Entity LeftWrist;
        public Entity RightWrist;
        public Entity LeftAsis;
        public Entity RightAsis;
        public Entity VSacral;
        public Entity LeftTight;
        public Entity RightTight;
        public Entity LeftKnee;
        public Entity RightKnee;
        public Entity LeftKneeMed;
        public Entity RightKneeMed;
        public Entity LeftShank;
        public Entity RightShank;
        public Entity LeftAnkle;
        public Entity RightAnkle;
        public Entity LeftAnkleMed;
        public Entity RightAnkleMed;
        public Entity LeftToe;
        public Entity RightToe;
        public Entity LeftToeMed;
        public Entity RightToeMed;
    }
}
