using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace AvoidML.Debug
{
    [GenerateAuthoringComponent]
    public struct SpinMove : IComponentData
    {
        public float3 Axis;
        public float Frequency;
        public float Amplify;
    }

    public class SpinMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = (float)Time.ElapsedTime;

            Entities
                .WithBurst()
                .ForEach((ref PhysicsVelocity velocity, in SpinMove spin) =>
                {
                    velocity.Angular = spin.Axis * math.sin(time * spin.Frequency) * spin.Amplify;
                }).ScheduleParallel();
        }
    }
}
