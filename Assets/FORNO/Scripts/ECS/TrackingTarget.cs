using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct TrackingTarget : IBufferElementData
    {
        public Entity Value;
    }

    public class TrackingTargetSystem : SystemBase
    {
        protected override void OnUpdate()
        {
        }
    }
}