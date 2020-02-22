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

        void Start()
        {
            if (slider == null)
                slider = GetComponent<Slider>();
            localTimeSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<LocalTimeSystem>();
        }

        public void UpdateTimeFactor()
        {
            localTimeSystem.IsBackToTheFuture = slider.value < 0;
            Time.timeScale = abs(slider.value);
        }
    }
}
