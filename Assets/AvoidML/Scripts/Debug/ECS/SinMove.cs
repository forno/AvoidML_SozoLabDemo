using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using static Unity.Mathematics.math;

namespace AvoidML.Debug
{
    [GenerateAuthoringComponent]
    public struct SinMove : IComponentData { }

    public class SinMoveSystem : SystemBase
    {
        public float frequency = 1;
        public float amplify = 1;

        protected override void OnUpdate()
        {
            var frequency = this.frequency * PI;
            var amplify = this.amplify;
            var time = (float)Time.ElapsedTime;
            Entities
                .WithBurst()
                .ForEach((ref PhysicsVelocity velocity, in SinMove signMove) =>
                {
                    velocity.Linear = float3(sin(time * frequency) * amplify, 0, 0);
                }).ScheduleParallel();
        }
    }
}
