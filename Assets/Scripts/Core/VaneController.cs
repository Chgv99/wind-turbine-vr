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
            climate = GameObject.Find("Climate").GetComponent<ClimateController>();
            windDirection = climate.WindDirection;
        }

        // Update is called once per frame
        void Update()
        {
            windDirection = climate.WindDirection;
            Debug.Log("windDirection from vane: " + windDirection.ToString());
            transform.rotation = WindDirection;
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
            //transform.rotation = Quaternion.LookRotation(transform.position, climate.WindDirection * Vector3.forward);
        }
    }
}
