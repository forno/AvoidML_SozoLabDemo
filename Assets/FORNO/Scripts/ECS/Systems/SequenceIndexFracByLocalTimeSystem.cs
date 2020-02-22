using Unity.Entities;
using Unity.Mathematics;

namespace Forno.Ecs
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateBefore(typeof(SequentialPositionInitialSystem))]
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
}
