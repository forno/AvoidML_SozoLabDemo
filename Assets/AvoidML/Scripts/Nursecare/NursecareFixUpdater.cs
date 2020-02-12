using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AvoidML.Nursecare
{
    public class NursecareFixUpdater : MonoBehaviour
    {
        private NursecareUpdater updater;
        // Start is called before the first frame update
        void Start()
        {
            updater = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<NursecareUpdater>();
            Time.fixedDeltaTime = 1f / 100f; // hardcode: 100hz data
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            updater.Update();
        }
    }
}
