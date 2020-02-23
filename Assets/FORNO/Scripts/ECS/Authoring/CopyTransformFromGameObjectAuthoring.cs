using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Forno.Ecs
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class CopyTransformFromGameObjectAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<CopyTransformFromGameObject>(entity);
        }
    }
}
