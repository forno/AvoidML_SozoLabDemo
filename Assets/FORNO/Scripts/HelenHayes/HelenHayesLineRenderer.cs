using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    var curIndex = i * lineRendererCountPerModel;
                    LineRenderers[curIndex].positionCount = 3;
                    LineRenderers[curIndex].SetPosition(0, components[i].FrontHead);
                    LineRenderers[curIndex].SetPosition(1, components[i].TopHead);
                    LineRenderers[curIndex].SetPosition(2, components[i].RearHead);
                }
            }
        }

        private static readonly int lineRendererCountPerModel = 5;
    }
}
