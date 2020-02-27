using Forno.Ecs;
using Unity.Entities;
using UnityEngine.UI;
using UnityEngine;
using static Unity.Mathematics.math;

namespace Forno
{
    public class TimeSlider : MonoBehaviour
    {
        public Slider slider;
        private LocalTimeSystem localTimeSystem;
        private float initDeltaTime;

        void Start()
        {
            if (slider == null) {
                slider = GetComponent<Slider>();
                if (slider == null) {
                    Debug.LogError("TimeSlider: Require slider");
                    Destroy(this);
                }
            }
            localTimeSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<LocalTimeSystem>();
            initDeltaTime = Time.fixedDeltaTime;
        }

        public void UpdateTimeFactor()
        {
            localTimeSystem.IsBackToTheFuture = slider.value < 0;
            Time.timeScale = abs(slider.value);
            Time.fixedDeltaTime = initDeltaTime * Time.timeScale;
        }
    }
}
