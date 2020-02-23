using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace AvoidML
{
    [Serializable]
    public struct CollisionParticleSpawner : ISharedComponentData, IEquatable<CollisionParticleSpawner>
    {
        public ParticleSystem Particle;

        public bool Equals(CollisionParticleSpawner other)
        {
            return Particle == other.Particle;
        }

        public override int GetHashCode()
        {
            return Particle.GetHashCode();
        }
    }

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class CollisionParticleSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public ParticleSystem Particle;
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new CollisionParticleSpawner { Particle = Particle });
        }
    }

    public class CollicationParticleSpawnerSystem : SystemBase
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
                .ForEach((CollisionParticleSpawner spawner, in WorldRenderBounds bounds) =>
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
                    var result = new NativeList<int>(Allocator.TempJob);
                    if (collisionWorld.OverlapAabb(overlabAabbInput, ref result)) {
                        if (!spawner.Particle.isPlaying)
                            spawner.Particle.Play();
                    } else {
                        if (!spawner.Particle.isStopped)
                            spawner.Particle.Stop();
                    }
                    result.Dispose();
                }).Run();
        }
    }
}
