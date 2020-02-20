using Unity.Entities;
using UnityEngine;

namespace AvoidML.Debug
{
    public class SpinMoveSystemManager : MonoBehaviour
    {
        public float frequency;
        public float amplify;
        private SpinMoveSystem system;

        void Start()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SpinMoveSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            system.frequency = frequency;
            system.amplify = amplify;
        }
    }
}
