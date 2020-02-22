using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Forno.HelenHayes
{
    public class HelenHayesLineRenderer : MonoBehaviour
    {
        public LineRenderer Prefab;
        private List<LineRenderer> LineRenderers;
        private EntityManager EntityManager;

        // Start is called before the first frame update
        void Start()
        {
            EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            LineRenderers = new List<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            var query = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<HelenHayesPositions>());
            using (var components = query.ToComponentDataArray<HelenHayesPositions>(Allocator.TempJob)) {
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
                    Vector3[] leftLeg = {component.LeftAsis, component.LeftTight, component.LeftKnee, component.LeftShank, component.LeftAnkle, component.LeftToe};
                    LineRenderers[curIndex + 1].positionCount = leftLeg.Length;
                    LineRenderers[curIndex + 1].SetPositions(leftLeg);
                }
            }
        }

        private static readonly int lineRendererCountPerModel = 5;
    }
}
