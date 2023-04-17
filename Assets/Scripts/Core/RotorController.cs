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
            velocity = 0;
            foreach (BladeController blade in Blades)
            {
                velocity += blade.Contribution;
            }
            Quaternion deltaRotation = Quaternion.Euler(
                new Vector3(0, 0, -velocity) * Time.fixedDeltaTime);

            GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * deltaRotation);

            // OLD BEHAVIOUR
            //GetComponent<Rigidbody>().AddRelativeTorque(transform.forward * -Torque);
        }
    }
}
