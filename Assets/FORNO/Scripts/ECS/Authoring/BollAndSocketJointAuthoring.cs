using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;
using static Unity.Mathematics.math;

namespace Forno.Ecs
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class BollAndSocketJointAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public PhysicsBodyAuthoring ConnectedBody;
        public float3 PositionLocal;
        public bool EnableCollision;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var worldFromB = new RigidTransform(ConnectedBody.transform.rotation, ConnectedBody.transform.position);
            var worldFromA = new RigidTransform(transform.rotation, transform.position);
            var bFromA = mul(inverse(worldFromB), worldFromA);
            var positionInConnectedEntity = transform(bFromA, PositionLocal);

            var physicsJoint = new PhysicsJoint
            {
                JointData = JointData.CreateBallAndSocket(PositionLocal, positionInConnectedEntity),
                EntityA = entity,
                EntityB = conversionSystem.GetPrimaryEntity(ConnectedBody),
                EnableCollision = EnableCollision ? 1 : 0,
            };
            var jointEntity = dstManager.CreateEntity(typeof(PhysicsJoint));
            dstManager.SetComponentData(jointEntity, physicsJoint);
        }
    }
}
