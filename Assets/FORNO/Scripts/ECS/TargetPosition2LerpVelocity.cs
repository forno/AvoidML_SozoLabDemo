using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct TargetPosition2LerpVelocity : IComponentData
    {
        public float ReachTime;
    }

    [UpdateBefore(typeof(BuildPhysicsWorld))]
    public class TargetPosition2LerpVelocitySystem : JobComponentSystem
    {
        [BurstCompile]
        struct TargetPosition2LerpVelocityJob : IJobForEach<TargetPosition2LerpVelocity, TargetPosition, Translation, PhysicsVelocity>
        {
            [ReadOnly]
            public float elapsedTime;
            [ReadOnly]
            public float impulseTime;

            public void Execute(
                [ReadOnly] ref TargetPosition2LerpVelocity systemInfo,
                    [ReadOnly] ref TargetPosition targetPosition,
                    [ReadOnly] ref Translation translation,
                    [WriteOnly] ref PhysicsVelocity velocity)
            {
                velocity.Linear = (targetPosition.Value - translation.Value) / math.max(systemInfo.ReachTime - elapsedTime, impulseTime);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new TargetPosition2LerpVelocityJob
            {
                elapsedTime = UnityEngine.Time.fixedTime,
                impulseTime = UnityEngine.Time.fixedDeltaTime
            };

            return job.Schedule(this, inputDependencies);
        }
    }
}
