using Unity.Entities;
using Unity.Physics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PhysicsVelocityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<PhysicsVelocity>(entity);
    }
}
