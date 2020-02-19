using Forno.Ecs;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

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
            localTimeSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<LocalTimeSystem>();
        }

        public void UpdateTimeFactor()
        {
            localTimeSystem.timeFactor = slider.value;
        }
    }
}
