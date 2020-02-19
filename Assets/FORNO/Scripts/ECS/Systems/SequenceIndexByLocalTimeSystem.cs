using Unity.Entities;

namespace Forno.Ecs
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SequenceIndexByLocalTimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithBurst()
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .ForEach((ref SequenceIndex index, in LocalTime time, in SequenceFrequency frequency) =>
                {
                    index.Value = (int)(time.Value * frequency.Value);
                }).ScheduleParallel();
        }
    }
}
