using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;

namespace Forno.Ecs
{
    [GenerateAuthoringComponent]
    public struct CopyDisabledToGameObject : IComponentData { }

    public class CopyDisabledToGameObjectSystem : JobComponentSystem
    {
        struct CopyDisabledToGameObjectJob : IJobForEachWithEntity<CopyDisabledToGameObject>
        {
            // Add fields here that your job needs to do its work.
            // For example,
            //    public float deltaTime;

            public void Execute(Entity entity, int index, [ReadOnly] ref CopyDisabledToGameObject c0)
            {
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new CopyDisabledToGameObjectJob();

            // Assign values to the fields on your job here, so that it has
            // everything it needs to do its work when it runs later.
            // For example,
            //     job.deltaTime = UnityEngine.Time.deltaTime;



            // Now that the job is set up, schedule it to be run.
            return job.Schedule(this, inputDependencies);
        }
    }
}
