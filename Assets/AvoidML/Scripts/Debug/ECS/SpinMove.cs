using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace AvoidML.Debug
{
    [GenerateAuthoringComponent]
    public struct SpinMove : IComponentData
    {
        public float3 Value;
    }

    public class SpinMoveSystem : SystemBase
    {
        public float frequency = 1;
        public float amplify = 1;

        protected override void OnUpdate()
        {
            var time = (float)Time.ElapsedTime;
            var frequency = this.frequency * math.PI;
            var amplify = this.amplify;

            Entities
                .WithBurst()
                .ForEach((ref Rotation rotation, in SpinMove spin) =>
                {
                    rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(spin.Value, math.sin(time * frequency) * amplify));
                }).ScheduleParallel();
        }
    }
}
