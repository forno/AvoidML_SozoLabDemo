using Unity.Entities;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct LocalTime : IComponentData
    {
        public float Value;
    }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(UpdateWorldTimeSystem))]
    public class LocalTimeSystem : SystemBase
    {
        public bool IsBackToTheFuture;

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime * (IsBackToTheFuture ? -1 : 1);
            Entities
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .ForEach((ref LocalTime time) =>
                {
                    time.Value += deltaTime;
                }).ScheduleParallel();
        }
    }
}
