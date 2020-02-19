using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct TargetPosition : IComponentData
    {
        public float3 Value;
    }
}
