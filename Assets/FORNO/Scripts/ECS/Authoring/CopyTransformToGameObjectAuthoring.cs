using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Forno.Ecs
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class CopyTransformToGameObjectAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<CopyTransformToGameObject>(entity);
        }
    }
}
