using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class BladeController : MonoBehaviour
    {
        RotorController rotor;

        // Start is called before the first frame update
        void Start()
        {
            rotor = transform.parent.parent.GetComponent<RotorController>();

            if (rotor == null) Error.LogException("RotorController not found");

            TurnOn();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("blade angle: " + transform.localEulerAngles.x); //max (min) is 290
            Debug.Log("torque sent (1): " + transform.localEulerAngles.x.Remap(360, 290, 0, 1));
            //Debug.Log("torque sent (2): " + );
            //Debug.Log("rotor's negative blue: " + (-transform.parent.forward) );
            rotor.Torque = transform.localEulerAngles.x.Remap(360, 290, 0, 1);
        }

        public void TurnOn()
        {
            StartCoroutine(RotateOn());
        }

        IEnumerator RotateOn()
        {
            float moveSpeed = 0.1f;
            float angle = -70;

            // Stops rotation when the angles are near to the angle objective (-70 aka 290)
            // Initial angle is less than zero (approximately zero) at first, but then is quickly converted to a positive angle.
            // That's why we need the part after the OR operand. Positive check just in case.
            while (transform.localEulerAngles.x > 291.5f || (transform.localEulerAngles.x > -1 && transform.localEulerAngles.x < 1) )
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angle, 0, 0), moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }

        /**IEnumerator RotateOff()
        {

        }*/
    }
}
