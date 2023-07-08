using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Object
{
    public class LittleTurbineController : MonoBehaviour
    {
        Transform littleNacelle;

        Transform nacelle;

        // Start is called before the first frame update
        void Start()
        {
            littleNacelle = transform.Find("Góndola");
            nacelle = GameObject.Find("Wind-Turbine/Góndola").transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            littleNacelle.rotation = nacelle.rotation;
        }
    }
}
