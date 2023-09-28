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
            //Debug.Log("windSpeed from anemometer: " + windSpeed);

            /** stablish a maximum rotation speed
             *  then map windspeed to the range
             *  from zero to the maximum rotation speed */

            
        }

        void FixedUpdate()
        {
            transform.Rotate(transform.up * -1, windSpeed * 10 * Time.deltaTime);
            //GetComponent<Rigidbody>().AddTorque(transform.up * windSpeed * 100);
        }
    }
}
