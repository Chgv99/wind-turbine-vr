using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class RotorController : MonoBehaviour
    {
        //GameObject climateController;

        [SerializeField][Range(0, 2f)] float torque = 0;

        // Start is called before the first frame update
        void Start()
        {
            //climateController = GameObject.Find("ClimateController");
            //if (climateController == null) Error.LogException("ClimateController not found");


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, -torque));
        }
    }
}
