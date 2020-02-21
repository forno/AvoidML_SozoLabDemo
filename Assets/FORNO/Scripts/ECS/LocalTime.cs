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
        public float timeFactor = 1;

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var factor = timeFactor;
            Entities
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .ForEach((ref LocalTime time) =>
                {
                    time.Value += deltaTime * factor;
                }).ScheduleParallel();
        }
    }
}
