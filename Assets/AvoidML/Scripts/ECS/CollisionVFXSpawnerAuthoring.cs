using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.VFX;

namespace AvoidML
{
    [Serializable]
    public struct CollisionVFXSpawner : ISharedComponentData, IEquatable<CollisionVFXSpawner>
    {
        public VisualEffect Effect;

        public bool Equals(CollisionVFXSpawner other)
        {
            return Effect == other.Effect;
        }

        public override int GetHashCode()
        {
            return Effect.GetHashCode();
        }
    }

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class CollisionVFXSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public VisualEffect VisualEffect;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new CollisionVFXSpawner { Effect = VisualEffect });
        }
    }

    public class CollicationVFXSpawnerSystem : SystemBase
    {
        public BuildPhysicsWorld System;

        protected override void OnCreate()
        {
            System = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            var collisionWorld = System.PhysicsWorld.CollisionWorld;
            Entities
                .WithoutBurst()
                .ForEach((CollisionVFXSpawner spawner, in WorldRenderBounds bounds) =>
                {
                    var aabb = new Aabb
                    {
                        Min = bounds.Value.Min,
                        Max = bounds.Value.Max
                    };
                    var overlabAabbInput = new OverlapAabbInput
                    {
                        Aabb = aabb,
                        Filter = new CollisionFilter
                        {
                            BelongsTo = ~0u,
                            CollidesWith = ~0u,
                            GroupIndex = 0
                        }
                    };
                    var result = new NativeList<int>(Allocator.Temp);
                    if (collisionWorld.OverlapAabb(overlabAabbInput, ref result)) {
                        spawner.Effect.Play();
                    } else {
                        spawner.Effect.Stop();
                    }
                    result.Dispose();
                }).Run();
        }
    }
}
