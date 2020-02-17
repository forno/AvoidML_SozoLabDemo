using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    public struct SequentialPositionsBlobAsset
    {
        public BlobArray<float3> Positions;
    }

    [Serializable]
    public struct SequentialPositions : IComponentData
    {
        public BlobAssetReference<SequentialPositionsBlobAsset> Value;
    }
}
