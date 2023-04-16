using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class RotorController : MonoBehaviour
    {
        float torque = 0;

        public float Torque { get => torque; set => torque = value; }

        // Start is called before the first frame update
        void Start()
        {
            //climateController = GameObject.Find("ClimateController");
            //if (climateController == null) Error.LogException("ClimateController not found");

            //Debug.Log("rotor " + transform.forward);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //GetComponent<Rigidbody>().torqye = -transform.forward * new Vector3(0, 0, GetComponent<Rigidbody>().angularVelocity.z);
            GetComponent<Rigidbody>().AddRelativeTorque(transform.forward * -Torque);
        }
    }
}
