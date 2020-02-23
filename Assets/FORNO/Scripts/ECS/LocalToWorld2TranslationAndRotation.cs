using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct LocalToWorld2TranslationAndRotation : IComponentData { }

    public class LocalToWorld2TranslationAndRotationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<LocalToWorld2TranslationAndRotation>()
                .ForEach((ref Translation translation, ref Rotation rotation, in LocalToWorld localToWorld) =>
                {
                    translation.Value = localToWorld.Position;
                    rotation.Value = localToWorld.Rotation;
                }).ScheduleParallel();
        }
    }
}
