using Unity.Entities;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct SequenceTimeFrac : IComponentData
    {
        public float Value;
    }
}
