using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class VaneController : MonoBehaviour
    {
        ClimateController climate;

        Quaternion windDirection;

        public Quaternion WindDirection { get => windDirection; }

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("ClimateController").GetComponent<ClimateController>();
            windDirection = climate.WindDirection;
        }

        // Update is called once per frame
        void Update()
        {
            windDirection = climate.WindDirection;
            transform.rotation = WindDirection;
            //transform.rotation = Quaternion.LookRotation(transform.position, climate.WindDirection * Vector3.forward);
        }
    }
}