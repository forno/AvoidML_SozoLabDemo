using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Forno.HelenHayes
{
    // This need prefab. You shuold attach it from MonoBehaviour
    [DisableAutoCreation]
    public class HelenHayesLineRendererSystem : SystemBase
    {
        public LineRenderer Prefab;
        private List<LineRenderer> LineRenderers;
        private EntityQuery query;
        private static readonly int lineRendererCountPerModel = 5;

        protected override void OnCreate()
        {
            LineRenderers = new List<LineRenderer>();
        }

        protected override void OnUpdate()
        {
            var lineRendererCount = query.CalculateEntityCount() * lineRendererCountPerModel;
            if (LineRenderers.Count > lineRendererCount) {
                for (var i = lineRendererCount; i < LineRenderers.Count; ++i) {
                    Object.Destroy(LineRenderers[i].gameObject);
                }
                LineRenderers.RemoveRange(lineRendererCount, LineRenderers.Count - lineRendererCount);
            } else if (LineRenderers.Count < lineRendererCount) {
                for (var i = LineRenderers.Count; i < lineRendererCount; ++i) {
                    LineRenderers.Add(Object.Instantiate(Prefab));
                }
            }
            Assert.AreEqual(LineRenderers.Count, lineRendererCount);

            var curIndex = 0;
            Entities
                .WithStoreEntityQueryInField(ref query)
                .WithoutBurst()
                .ForEach((in HelenHayesPositions positions) =>
                {
                    Vector3[] body = {positions.FrontHead, positions.TopHead, positions.RearHead, positions.VSacral};
                    LineRenderers[curIndex].positionCount = body.Length;
                    LineRenderers[curIndex].SetPositions(body);
                    Vector3[] leftArm = {positions.LeftWrist, positions.LeftElbow, positions.LeftShoulder};
                    LineRenderers[++curIndex].positionCount = leftArm.Length;
                    LineRenderers[curIndex].SetPositions(leftArm);
                    Vector3[] rightArm = {positions.RightWrist, positions.RightElbow, positions.RightShoulder};
                    LineRenderers[++curIndex].positionCount = rightArm.Length;
                    LineRenderers[curIndex].SetPositions(rightArm);
                    Vector3[] leftLeg = {positions.LeftToe, positions.LeftHeel, positions.LeftAnkle, positions.LeftShank, positions.LeftKnee, positions.LeftTight, positions.LeftAsis};
                    LineRenderers[++curIndex].positionCount = leftLeg.Length;
                    LineRenderers[curIndex].SetPositions(leftLeg);
                    Vector3[] rightLeg = {positions.RightToe, positions.RightHeel, positions.RightAnkle, positions.RightShank, positions.RightKnee, positions.RightTight, positions.RightAsis};
                    LineRenderers[++curIndex].positionCount = rightLeg.Length;
                    LineRenderers[curIndex].SetPositions(rightLeg);
                    ++curIndex;
                }).Run();
        }
    }

    public class HelenHayesLineRenderer : MonoBehaviour
    {
        public LineRenderer Prefab;

        public void Start()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var helenHayesLineRendererSystem = world.GetOrCreateSystem<HelenHayesLineRendererSystem>();
            helenHayesLineRendererSystem.Prefab = Prefab;
            var simulationSystemGroup = world.GetOrCreateSystem<SimulationSystemGroup>();
            simulationSystemGroup.AddSystemToUpdateList(helenHayesLineRendererSystem);
            simulationSystemGroup.SortSystemUpdateList();
            // Destroy itself
            Destroy(this);
        }
    }
}
