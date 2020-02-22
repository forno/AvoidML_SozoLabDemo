using Unity.Entities;

namespace Forno.HelenHayes
{
    [GenerateAuthoringComponent]
    public struct HelenHayesEntitiesHolder : IComponentData
    {
        public Entity FrontHead;
        public Entity TopHead;
        public Entity RearHead;
        public Entity RightShoulder;
        public Entity LeftShoulder;
        public Entity RightOffset;
        public Entity LeftElbow;
        public Entity RightElbow;
        public Entity LeftWrist;
        public Entity RightWrist;
        public Entity RightAsis;
        public Entity LeftAsis;
        public Entity VSacral;
        public Entity RightTight;
        public Entity LeftTight;
        public Entity RightKnee;
        public Entity LeftKnee;
        public Entity LeftKneeMed;
        public Entity RightKneeMed;
        public Entity RightShank;
        public Entity LeftShank;
        public Entity RightAnkle;
        public Entity LeftAnkle;
        public Entity LeftAnkleMed;
        public Entity RightAnkleMed;
        public Entity RightToe;
        public Entity LeftToe;
        public Entity LeftToeMed;
        public Entity RightToeMed;
    }
}
