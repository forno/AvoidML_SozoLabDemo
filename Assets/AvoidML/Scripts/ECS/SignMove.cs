using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace AvoidML
{
    [GenerateAuthoringComponent]
    public struct SignMove : IComponentData { }

    public class SignMoveSystem : SystemBase
    {
        public float frequency = 1;
        public float amplify = 1;

        protected override void OnUpdate()
        {
            var frequency = this.frequency;
            var amplify = this.amplify;
            var time = (float)Time.ElapsedTime;
            Entities
                .WithBurst()
                .ForEach((ref PhysicsVelocity velocity, in SignMove signMove) =>
                {
                    velocity.Linear = float3(sin(time * frequency) * amplify, 0, 0);
                }).ScheduleParallel();
        }
    }
}
