using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    public struct SequentialEnabledsBlobAsset
    {
        public BlobArray<bool> Positions;
    }

    [Serializable]
    public struct SequentialEnableds : IComponentData
    {
        public BlobAssetReference<SequentialEnabledsBlobAsset> Value;
    }
}
