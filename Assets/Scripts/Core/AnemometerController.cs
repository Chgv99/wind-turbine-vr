using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class AnemometerController : MonoBehaviour
    {
        ClimateController climate;

        float windSpeed;

        public float WindSpeed { get => windSpeed; set => windSpeed = value; }

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("Climate").GetComponent<ClimateController>();
            windSpeed = climate.WindSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            windSpeed = climate.WindSpeed;

            /** stablish a maximum rotation speed
             *  then map windspeed to the range
             *  from zero to the maximum rotation speed */

        }
    }
}
