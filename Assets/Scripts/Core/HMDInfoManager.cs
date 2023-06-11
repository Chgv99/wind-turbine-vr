using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace WindTurbineVR.Core
{
    public class HMDInfoManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (!XRSettings.isDeviceActive)
            {
                Debug.Log("No headset detected");
            } 
            else if (
                XRSettings.isDeviceActive && (
                XRSettings.loadedDeviceName == "Mock HMD" ||
                XRSettings.loadedDeviceName == "MockHMDDisplay"))
            {
                Debug.Log("Using Mock HMD");
            }
            else
            {
                Debug.Log("Headset detected. Device name: " + XRSettings.loadedDeviceName);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
