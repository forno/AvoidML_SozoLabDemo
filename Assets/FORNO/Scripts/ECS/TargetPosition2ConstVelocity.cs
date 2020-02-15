using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using static Unity.Mathematics.math;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct TargetPosition2ConstVelocity : IComponentData
    {
        public float LimitVelocity;
    }

    [UpdateBefore(typeof(BuildPhysicsWorld))]
    public class TargetPosition2ConstVelocitySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return Entities
                .WithBurst()
                .ForEach((ref PhysicsVelocity velocity, in TargetPosition target, in Translation translation, in TargetPosition2ConstVelocity info) =>
                {
                    velocity.Linear = normalize(target.Value - translation.Value) * info.LimitVelocity;
                    if (any(isnan(velocity.Linear)))
                        velocity.Linear = float3(0);
                }).Schedule(inputDependencies);
        }
    }
}
