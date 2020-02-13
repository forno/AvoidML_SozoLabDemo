using Unity.Entities;

namespace AvoidML.Nursecare
{
    [GenerateAuthoringComponent]
    public struct NursecareData : IComponentData
    {
        public int Index;
    }
}
