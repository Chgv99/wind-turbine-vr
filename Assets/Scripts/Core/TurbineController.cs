using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineController : MonoBehaviour
    {
        TurbineDataController turbineDataController;

        [SerializeField] RotorController rc;
        [SerializeField] NacelleController nc;
        [SerializeField] VaneController vc;
        //[SerializeField] AnemometerController ac;

        #region private
        bool active = false;
        #endregion

        public bool Active
        {
            get => active; 
            set
            {
                active = value;
                turbineDataController.Active = active;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            turbineDataController = GameObject.Find("TurbineDataController").GetComponent<TurbineDataController>();

            if (rc == null) Error.LogException("RotorController not found");
            if (nc == null) Error.LogException("NacelleController not found");
            if (vc == null) Error.LogException("VaneController not found");
            //if (am == null) Error.LogException("AnemometerController not found");

            /** TODO: Pasar la rotaci�n de la g�ndola
             * y el rotor de una escena a otra.
             * Conservar el comportamiento de comenzar
             * a rotar hacia el viento (g�ndola).*/

            //rc.SetRotation(turbineDataController.RotorRotation);
            nc.SetRotation(turbineDataController.NacelleRotation);

            SwitchOn();
            //get nacelle rotation
            //compare with current wind rotation
            //do rotation
            nc.Rotate(vc.WindDirection, vc);
        }

        void Update()
        {
            turbineDataController.RotorRotation = rc.GetRotation();
            turbineDataController.NacelleRotation = nc.GetRotation();
        }

        public void SwitchOn() => SwitchStatusOn();

        void SwitchStatusOn()
        {
            Active = true;
            rc.TurnOn();
        }

        public void SwitchOff() => SwitchStatusOff();

        void SwitchStatusOff()
        {
            Active = false;
            rc.TurnOff();
        }

        void LoadStatus()
        {

        }
    }
}
