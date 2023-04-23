using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineController : MonoBehaviour
    {
        [SerializeField] RotorController rc;
        
        //List<BladeController> bc;

        // Start is called before the first frame update
        void Start()
        {
            if (rc == null) Error.LogException("RotorController not found");
            //bc = rc.Blades;

            //if (bc == null) Error.LogException("BladeController not found");
            SwitchOn();
        }

        public void SwitchOn() => rc.TurnOn();

        public void SwitchOff() => rc.TurnOff();
    }
}
