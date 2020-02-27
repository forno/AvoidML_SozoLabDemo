using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(LocalTimeSystem))]
    public class SequenceIndexFracByLocalTimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .ForEach((ref SequenceIndex index, ref SequenceTimeFrac frac, in LocalTime time, in SequenceFrequency frequency) =>
                {
                    var ans = time.Value * frequency.Value;
                    index.Value = (int)ans;
                    frac.Value = math.frac(ans);
                }).ScheduleParallel();
        }
    }

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(UpdateWorldTimeSystem))]
    public class SequenceIndexFracSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var elapsedTime = (float)Time.ElapsedTime;
            Entities
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .WithNone<LocalTime>()
                .ForEach((ref SequenceIndex index, ref SequenceTimeFrac frac, in SequenceFrequency frequency) =>
                {
                    var ans = elapsedTime * frequency.Value;
                    index.Value = (int)ans;
                    frac.Value = math.frac(ans);
                }).ScheduleParallel();
        }
    }
}
