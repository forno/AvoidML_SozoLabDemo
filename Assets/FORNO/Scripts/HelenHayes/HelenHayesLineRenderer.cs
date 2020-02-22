using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Forno.HelenHayes
{
    public class HelenHayesLineRenderer : MonoBehaviour
    {
        public LineRenderer Prefab;
        private List<LineRenderer> LineRenderers;
        private EntityManager EntityManager;
        private EntityQuery Query;

        // Start is called before the first frame update
        void Start()
        {
            EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            LineRenderers = new List<LineRenderer>();
            Query = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<HelenHayesPositions>());
        }

        // Update is called once per frame
        void Update()
        {
            using (var components = Query.ToComponentDataArray<HelenHayesPositions>(Allocator.TempJob)) {
                var lineRendererCount = components.Length * lineRendererCountPerModel;
                if (LineRenderers.Count > lineRendererCount) {
                    for (var i = lineRendererCount; i < LineRenderers.Count; ++i) {
                        Destroy(LineRenderers[i].gameObject);
                    }
                    LineRenderers.RemoveRange(lineRendererCount, LineRenderers.Count - lineRendererCount);
                } else if (LineRenderers.Count < lineRendererCount) {
                    for (var i = LineRenderers.Count; i < lineRendererCount; ++i) {
                        LineRenderers.Add(Instantiate(Prefab));
                    }
                }
                Assert.AreEqual(LineRenderers.Count, lineRendererCount);

                for (var i = 0; i < components.Length; ++i) {
                    var component = components[i];
                    var curIndex = i * lineRendererCountPerModel;
                    Vector3[] body = {component.FrontHead, component.TopHead, component.RearHead, component.RightOffset, component.VSacral};
                    LineRenderers[curIndex].positionCount = body.Length;
                    LineRenderers[curIndex].SetPositions(body);
                    Vector3[] leftArm = {component.LeftWrist, component.LeftElbow, component.LeftShoulder, component.RightOffset};
                    LineRenderers[curIndex + 1].positionCount = leftArm.Length;
                    LineRenderers[curIndex + 1].SetPositions(leftArm);
                    Vector3[] rightArm = {component.RightWrist, component.RightElbow, component.RightShoulder, component.RightOffset};
                    LineRenderers[curIndex + 2].positionCount = rightArm.Length;
                    LineRenderers[curIndex + 2].SetPositions(rightArm);
                    Vector3[] leftLeg = {component.LeftToe, component.LeftHeel, component.LeftAnkle, component.LeftShank, component.LeftKnee, component.LeftTight, component.LeftAsis, component.VSacral};
                    LineRenderers[curIndex + 3].positionCount = leftLeg.Length;
                    LineRenderers[curIndex + 3].SetPositions(leftLeg);
                    Vector3[] rightLeg = {component.RightToe, component.RightHeel, component.RightAnkle, component.RightShank, component.RightKnee, component.RightTight, component.RightAsis, component.VSacral};
                    LineRenderers[curIndex + 4].positionCount = rightLeg.Length;
                    LineRenderers[curIndex + 4].SetPositions(rightLeg);
                }
            }
        }

        private static readonly int lineRendererCountPerModel = 5;
    }
}
