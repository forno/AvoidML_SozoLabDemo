using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;
using System;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Jobs;

namespace AvoidML
{
    [Serializable]
    public struct AvoidByDistance : IComponentData
    {
        // In global space
        public float3 WorkingPoint;
        public float DetectRange;
    }

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class AvoidByDistanceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject WorkingPoint;
        public float DetectRange = 0.5f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new AvoidByDistance
            {
                WorkingPoint = WorkingPoint.transform.position,
                DetectRange = DetectRange
            });
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
                .ForEach((ref PhysicsVelocity velocity, ref Rotation rotation, in AvoidByDistance avoider) =>
                {
                    var pointDistanceInput = new PointDistanceInput
                    {
                        Position = avoider.WorkingPoint,
                        Filter = new CollisionFilter
                        {
                            BelongsTo = 4u, // 100 mean Fugitive
                            CollidesWith = 2u, // 10 mean Tracker
                            GroupIndex = 0,
                        },
                        MaxDistance = 1f,
                    };
                    if (collisionWorld.CalculateDistance(pointDistanceInput, out var closestHit )) {
                        var localPos = closestHit.Position - avoider.WorkingPoint;
                        var localDistance = length(localPos);
                        if (localDistance < avoider.DetectRange) {
                            rotation.Value = Quaternion.Lerp(rotation.Value, Unity.Mathematics.quaternion.RotateY(radians(90)), 0.02f);
                        } else {
                            rotation.Value = Quaternion.Lerp(rotation.Value, Unity.Mathematics.quaternion.RotateY(radians(0)), 0.02f);
                        }
                    }
                }).ScheduleParallel();
            Dependency.Complete();
        }
    }
}
