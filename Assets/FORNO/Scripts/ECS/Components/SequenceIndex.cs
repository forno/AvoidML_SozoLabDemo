using Unity.Entities;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct SequenceIndex : IComponentData
    {
        public int Value;
    }
}
