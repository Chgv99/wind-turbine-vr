using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class RotorController : MonoBehaviour
    {
        List<BladeController> blades = new List<BladeController>();

        [SerializeField] static float velocity; //degrees per frame

        public List<BladeController> Blades { get => blades; set => blades = value; }

        //public float Velocity { get => velocity; set => velocity = value; }

        /* OLD BEHAVIOUR
        float torque = 0;

        public float Torque { get => torque; set => torque = value; }
        */
        // Start is called before the first frame update
        void Start()
        {
            int count = transform.childCount;

            for (int i = 0; i < count; i++) {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name != "BladeContainer") continue;

                Blades.Add(child.GetChild(0).GetComponent<BladeController>());
            }

            //climateController = GameObject.Find("ClimateController");
            //if (climateController == null) Error.LogException("ClimateController not found");
            //Debug.Log("rotor " + transform.forward);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Debug.Log("RotorController");
            velocity = 0;
            foreach (BladeController blade in Blades)
            {
                //Debug.Log("velocity " + velocity + "+" + blade.Contribution);
                velocity += blade.Contribution;
            }
            Quaternion deltaRotation = Quaternion.Euler(
                new Vector3(0, 0, -velocity) * Time.fixedDeltaTime);

            Vector3 rotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z * -velocity);

            //GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);
            //transform.Rotate(new Vector3(0, 0, 1) * -velocity * Time.fixedDeltaTime);

            //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.localRotation.z));
            // OLD BEHAVIOUR
            GetComponent<Rigidbody>().AddRelativeTorque(transform.forward * -velocity * 0.005f);
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);

        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void OverrideTurnOn()
        {
            foreach (BladeController blade in Blades) blade.OverrideTurnOn();
        }

        public void OverrideTurnOff()
        {
            foreach (BladeController blade in Blades) blade.OverrideTurnOff();
        }

        public void TurnOn()
        {
            foreach (BladeController blade in Blades) blade.TurnOn();
        }

        public void TurnOff()
        {
            foreach (BladeController blade in Blades) blade.TurnOff();
        }
    }
}
