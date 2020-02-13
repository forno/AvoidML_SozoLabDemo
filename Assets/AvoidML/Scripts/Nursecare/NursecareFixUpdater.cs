using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AvoidML.Nursecare
{
    public class NursecareFixUpdater : MonoBehaviour
    {
        private NursecareInputDataSystem updater;
        // Start is called before the first frame update
        void Start()
        {
            updater = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<NursecareInputDataSystem>();
            Time.fixedDeltaTime = Constants.timeInterval;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            updater.Update();
        }
    }
}
