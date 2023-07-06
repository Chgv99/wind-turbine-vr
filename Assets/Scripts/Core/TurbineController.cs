using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineController : MonoBehaviour
    {
        TurbineDataController turbineDataController;

        [SerializeField] public RotorController rc;
        [SerializeField] public NacelleController nc;
        [SerializeField] public VaneController vc;
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

            /** TODO: Pasar la rotación de la góndola
             * y el rotor de una escena a otra.
             * Conservar el comportamiento de comenzar
             * a rotar hacia el viento (góndola).*/

            //rc.SetRotation(turbineDataController.RotorRotation);
            nc.SetRotation(turbineDataController.NacelleRotation);

            //SwitchOn();
            SwitchOn();
        }

        void Update()
        {
            turbineDataController.RotorRotation = rc.GetRotation();
            turbineDataController.NacelleRotation = nc.GetRotation();

            /** TODO: CALL NACELLE ROTATION
             * DIRECTLY FROM VANE CONTROLLER
             * (THEY COMMUNICATE DIRECTLY)*/
            //get nacelle rotation
            //compare with current wind rotation
            //do rotation
            //nc.Rotate(vc.WindDirection, vc);
            //Debug.Log("vane winddirection seen from turbinecontroller: " + vc.WindDirection);
            nc.DirectionObjective = vc.WindDirection;
        }

        public void OverrideSwitchOn() => rc.OverrideTurnOn();

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

        public void SwitchStatus()
        {
            Active = !Active;
            if (Active) rc.TurnOn();
            else rc.TurnOff();
        }

        void LoadStatus()
        {

        }
    }
}
