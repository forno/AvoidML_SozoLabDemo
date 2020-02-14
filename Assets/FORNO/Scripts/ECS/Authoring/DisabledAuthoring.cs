using Unity.Entities;
using UnityEngine;

namespace Forno.Ecs
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class DisabledAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.SetEnabled(entity, false);
        }
    }
}
