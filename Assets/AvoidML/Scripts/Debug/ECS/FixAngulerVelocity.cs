using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace AvoidML.Debug
{
    [GenerateAuthoringComponent]
    public struct FixAngulerVelocity : IComponentData { }

    public class FixAngulerVelocitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<FixAngulerVelocity>()
                .ForEach((ref PhysicsVelocity velocity) =>
                {
                    velocity.Angular = float3.zero;
                }).ScheduleParallel();
        }
    }
}
