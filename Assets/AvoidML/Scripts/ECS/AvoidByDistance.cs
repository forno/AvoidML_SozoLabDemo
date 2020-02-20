using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;

namespace AvoidML
{
    [GenerateAuthoringComponent]
    public struct AvoidByDistance : IComponentData
    {
        public float3 CenterPoint;
    }

    public class AvoidByDistanceUpdateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref AvoidByDistance avoider, in LocalToWorld localToWorld) =>
                {
                    avoider.CenterPoint = math.transform(localToWorld.Value, new float3(0, 0, 1f));
                }).ScheduleParallel();
        }
    }

    public class AvoidByDistanceSystem : SystemBase
    {
        private BuildPhysicsWorld system;

        protected override void OnCreate()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            var collisionWorld = system.PhysicsWorld.CollisionWorld;

            Entities
                .WithoutBurst()
                .ForEach((ref Rotation rotation, in AvoidByDistance avoider) =>
                {
                    var pointDistanceInput = new PointDistanceInput
                    {
                        // XXX: I suppose that local space and world space are same
                        Position = avoider.CenterPoint,
                        Filter = new CollisionFilter
                        {
                            BelongsTo = 4u, // 100 mean Fugitive
                            CollidesWith = 2u, // 10 mean Tracker
                            GroupIndex = 0,
                        },
                        MaxDistance = 0.5f,
                    };
                    var allHits = new NativeList<DistanceHit>(Allocator.Temp);
                    if (collisionWorld.CalculateDistance(pointDistanceInput, ref allHits)) {
                        var job = new ComputeMassCenter(avoider.CenterPoint, allHits);
                        job.Schedule().Complete();
                        // XXX: I supporse that local and world space are same.
                        //var localPos = math.mul(math.inverse(rotation.Value), job.mass[0]);
                        if (job.mass[0].x > 0) {
                            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(1)));
                        } else if (job.mass[0].x < 0) {
                            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(-1)));
                        }
                        job.mass.Dispose();
                    }
                    allHits.Dispose();
                }).Run();
        }
    }

    [BurstCompile]
    public struct ComputeMassCenter : IJob
    {
        public float3 center;
        [ReadOnly] public NativeArray<DistanceHit> hits;
        public NativeArray<float3> mass;

        public ComputeMassCenter(float3 center, NativeArray<DistanceHit> hits)
        {
            this.center = center;
            this.hits = hits;
            mass = new NativeArray<float3>(1, Allocator.TempJob);
        }

        public void Execute()
        {
            for (var i = 0; i < hits.Length; ++i) {
                mass[0] += hits[i].Position - center;
            }
        }
    }
}
