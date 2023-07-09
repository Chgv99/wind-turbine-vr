using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.Object
{
    public class LittleTurbineController : MonoBehaviour
    {
        Transform littleNacelle;
        Transform littleRotor;

        List<Transform> littleBlade;

        Transform nacelle;
        Transform rotor;

        List<Transform> blade;

        // Start is called before the first frame update
        void Start()
        {
            Transform turbine = GameObject.Find("Wind-Turbine").transform;
            littleNacelle = transform.Find("Góndola");
            nacelle = turbine.Find("Góndola");

            littleRotor = transform.Find("Góndola/Góndola/Rotor");
            rotor = turbine.Find("Góndola/Rotor");

            littleBlade = new List<Transform>
            {
                littleRotor.Find("Blade 1/Blade"),
                littleRotor.Find("Blade 2/Blade"),
                littleRotor.Find("Blade 3/Blade")
            };
            blade = new List<Transform>();
            for (int i = 0; i < rotor.childCount; i++)
            {
                Transform child = rotor.GetChild(i);
                if (child.gameObject.name != "BladeContainer") continue;

                blade.Add(child.GetChild(0));
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            littleNacelle.rotation = nacelle.rotation;
            
            littleRotor.rotation = rotor.rotation;

            #region Blades
            Vector3 bladeRotation;
            Vector3 littleBladeRotation;

            bladeRotation = blade[0].localRotation.eulerAngles;
            littleBladeRotation = littleBlade[0].localRotation.eulerAngles;
            littleBlade[0].localRotation = Quaternion.Euler(new Vector3(bladeRotation.x, littleBladeRotation.y, littleBladeRotation.z));
            bladeRotation = blade[1].localRotation.eulerAngles;
            littleBladeRotation = littleBlade[1].localRotation.eulerAngles;
            littleBlade[1].localRotation = Quaternion.Euler(new Vector3(bladeRotation.x, littleBladeRotation.y, littleBladeRotation.z));
            bladeRotation = blade[2].localRotation.eulerAngles;
            littleBladeRotation = littleBlade[2].localRotation.eulerAngles;
            littleBlade[2].localRotation = Quaternion.Euler(new Vector3(bladeRotation.x, littleBladeRotation.y, littleBladeRotation.z));
            #endregion
            //littleBlade[0].rotation = Quaternion.Euler();
            //littleBlade[1].rotation = blade.rotation;
            //littleBlade[2].rotation = blade.rotation;
            //Quaternion q = Quaternion
        }
    }
}
