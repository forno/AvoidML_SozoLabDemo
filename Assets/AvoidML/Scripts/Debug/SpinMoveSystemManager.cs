using Unity.Entities;
using UnityEngine;

namespace AvoidML.Debug
{
    public class SpinMoveSystemManager : MonoBehaviour
    {
        public float Frequency;
        public float Amplify;
        private SpinMoveSystem system;

        void Start()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SpinMoveSystem>();
        }
    }
}
