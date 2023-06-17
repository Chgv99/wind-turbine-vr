using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] Light light;

        void Start()
        {
            light.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "LitArea") return;
            Debug.Log("Trigger entered");
            light.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "LitArea") return;
            Debug.Log("Trigger exited");
            light.enabled = false;
        }
    }
}
