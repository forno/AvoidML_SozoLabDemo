using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;

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
                avoider.CenterPoint = math.transform(localToWorld.Value, new float3(0, 0, 1));
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
                    MaxDistance = 1,
                };
                if (collisionWorld.CalculateDistance(pointDistanceInput, out var closestHit)) {
                    if (closestHit.SurfaceNormal.x > 0) {
                        rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(1)));
                    } else {
                        rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(-1)));
                    }
                }
            }).Run();
    }
}
