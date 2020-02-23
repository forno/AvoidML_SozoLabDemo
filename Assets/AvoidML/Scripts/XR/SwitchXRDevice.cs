using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace AvoidML.Xr
{
    public class SwitchXRDevice : MonoBehaviour
    {
        public string DeviceName;
        void Start()
        {
            foreach (var device in XRSettings.supportedDevices)
                UnityEngine.Debug.Log(device);
            StartCoroutine(LoadDevice(DeviceName));
        }

        IEnumerator LoadDevice(string newDevice)
        {
            if (String.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0) {
                XRSettings.LoadDeviceByName(newDevice);
                yield return null;
                XRSettings.enabled = true;
            }
        }
    }
}
