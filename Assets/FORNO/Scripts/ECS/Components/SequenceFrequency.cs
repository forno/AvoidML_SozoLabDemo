using Unity.Entities;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct SequenceFrequency : IComponentData
    {
        public float Value;
    }
}
