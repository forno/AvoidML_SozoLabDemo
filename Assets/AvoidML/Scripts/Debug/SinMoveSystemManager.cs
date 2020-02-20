using Unity.Entities;
using UnityEngine;

namespace AvoidML.Debug
{
    public class SinMoveSystemManager : MonoBehaviour
    {
        public float frequency;
        public float amplify;
        private SinMoveSystem system;

        void Start()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SinMoveSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            system.frequency = frequency;
            system.amplify = amplify;
        }
    }
}
