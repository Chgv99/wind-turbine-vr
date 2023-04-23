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

            nc.Rotate(vc.WindDirection);
        }

        public void SwitchOn() => rc.TurnOn();

        public void SwitchOff() => rc.TurnOff();
    }
}
