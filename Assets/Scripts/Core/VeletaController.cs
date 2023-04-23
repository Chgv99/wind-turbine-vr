using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class VeletaController : MonoBehaviour
    {
        ClimateController climate;

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("ClimateController").GetComponent<ClimateController>();    
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = climate.WindDirection;
            //transform.rotation = Quaternion.LookRotation(transform.position, climate.WindDirection * Vector3.forward);
        }
    }
}
