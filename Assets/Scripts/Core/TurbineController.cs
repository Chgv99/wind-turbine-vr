using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineController : MonoBehaviour
    {
        [SerializeField] RotorController rc;
        [SerializeField] NacelleController nc;
        [SerializeField] VaneController vc;
        //[SerializeField] AnemometerController ac;

        //List<BladeController> bc;

        // Start is called before the first frame update
        void Start()
        {
            if (rc == null) Error.LogException("RotorController not found");
            if (nc == null) Error.LogException("NacelleController not found");
            if (vc == null) Error.LogException("VaneController not found");
            //if (am == null) Error.LogException("AnemometerController not found");

            SwitchOn();
            //get nacelle rotation
            //compare with current wind rotation
            //do rotation
            Debug.Log("turbinecontroller winddirection: " + vc.WindDirection);
            nc.Rotate(vc.WindDirection, vc);
        }

        void Update()
        {
            
        }

        public void SwitchOn() => rc.TurnOn();

        public void SwitchOff() => rc.TurnOff();
    }
}
