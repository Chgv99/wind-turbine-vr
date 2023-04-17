using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineController : MonoBehaviour
    {
        [SerializeField] RotorController rc;
        
        List<BladeController> bc;

        // Start is called before the first frame update
        void Start()
        {
            if (rc == null) Error.LogException("RotorController not found");
            bc = rc.Blades;

            if (bc == null) Error.LogException("BladeController not found");
        }

        public void SwitchOn()
        {
            foreach (BladeController bc in bc) bc.TurnOn();
        }

        public void SwitchOf()
        {
            foreach (BladeController bc in bc) bc.TurnOff();
        }
    }
}
