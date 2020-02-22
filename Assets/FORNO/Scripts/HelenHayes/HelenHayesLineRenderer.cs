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
        private GameObject parent;
        private List<LineRenderer> lineRenderers;
        private EntityQuery query;
        private static readonly int lineRendererCountPerModel = 5;

        protected override void OnCreate()
        {
            parent = new GameObject("HelenHayesLineRendererSystem Managed Object");
            lineRenderers = new List<LineRenderer>();
        }

        protected override void OnUpdate()
        {
            var lineRendererCount = query.CalculateEntityCount() * lineRendererCountPerModel;
            if (lineRenderers.Count > lineRendererCount) {
                for (var i = lineRendererCount; i < lineRenderers.Count; ++i) {
                    Object.Destroy(lineRenderers[i].gameObject);
                }
                lineRenderers.RemoveRange(lineRendererCount, lineRenderers.Count - lineRendererCount);
            } else if (lineRenderers.Count < lineRendererCount) {
                for (var i = lineRenderers.Count; i < lineRendererCount; ++i) {
                    var lineRenderer = Object.Instantiate(Prefab);
                    lineRenderer.transform.parent = parent.transform;
                    lineRenderers.Add(lineRenderer);
                }
            }
            Assert.AreEqual(lineRenderers.Count, lineRendererCount);

            var curIndex = 0;
            Entities
                .WithStoreEntityQueryInField(ref query)
                .WithoutBurst()
                .ForEach((in HelenHayesPositions positions) =>
                {
                    Vector3[] body = {positions.FrontHead, positions.TopHead, positions.RearHead, positions.VSacral};
                    lineRenderers[curIndex].positionCount = body.Length;
                    lineRenderers[curIndex].SetPositions(body);
                    Vector3[] leftArm = {positions.LeftWrist, positions.LeftElbow, positions.LeftShoulder};
                    lineRenderers[++curIndex].positionCount = leftArm.Length;
                    lineRenderers[curIndex].SetPositions(leftArm);
                    Vector3[] rightArm = {positions.RightWrist, positions.RightElbow, positions.RightShoulder};
                    lineRenderers[++curIndex].positionCount = rightArm.Length;
                    lineRenderers[curIndex].SetPositions(rightArm);
                    Vector3[] leftLeg = {positions.LeftToe, positions.LeftHeel, positions.LeftAnkle, positions.LeftShank, positions.LeftKnee, positions.LeftTight, positions.LeftAsis};
                    lineRenderers[++curIndex].positionCount = leftLeg.Length;
                    lineRenderers[curIndex].SetPositions(leftLeg);
                    Vector3[] rightLeg = {positions.RightToe, positions.RightHeel, positions.RightAnkle, positions.RightShank, positions.RightKnee, positions.RightTight, positions.RightAsis};
                    lineRenderers[++curIndex].positionCount = rightLeg.Length;
                    lineRenderers[curIndex].SetPositions(rightLeg);
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
