using UnityEngine;

namespace AvoidML.Debug
{
    public class SpinMoveBehaviour : MonoBehaviour
    {
        public Vector3 axis = Vector3.up;
        public float frequency = 1;
        public float amplify = 1;

        // Update is called once per frame
        void Update()
        {
            transform.rotation *= Quaternion.AngleAxis(Mathf.Sin(Time.time * frequency * Mathf.PI) * amplify, axis);
        }
    }
}
