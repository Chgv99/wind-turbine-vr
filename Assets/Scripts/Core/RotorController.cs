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

        void Start()
        {
            int count = transform.childCount;

            for (int i = 0; i < count; i++) {
                Transform child = transform.GetChild(i);
                if (child.gameObject.name != "BladeContainer") continue;

                Blades.Add(child.GetChild(0).GetComponent<BladeController>());
            }
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

            Vector3 rotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z * -velocity);

            GetComponent<Rigidbody>().AddRelativeTorque(transform.forward * -velocity * 0.005f);
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);   // evita rotación en otros ejes debido a inercias
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
