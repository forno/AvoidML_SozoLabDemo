using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AvoidML
{
    public class SignMoveSystemManager : MonoBehaviour
    {
        public float frequency;
        public float amplify;
        private SignMoveSystem system;

        void Start()
        {
            system = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SignMoveSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            system.frequency = frequency;
            system.amplify = amplify;
        }
    }
}
