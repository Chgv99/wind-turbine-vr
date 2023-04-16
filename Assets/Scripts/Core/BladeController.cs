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
            rotor = transform.parent.GetComponent<RotorController>();

            if (rotor == null) Error.LogException("RotorController not found");

            TurnOn();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("blade's up: " + transform.up); //max is 0.34
            //Debug.Log("torque sent (1): " + ((1 / transform.up.y) / 0.34f) / 8.6f);
            Debug.Log("torque sent (2): " + (((1 / transform.up.y) / 0.34f) / 8.6f).Remap(0.3419972f, 1, 0, 1));
            //Debug.Log("rotor's negative blue: " + (-transform.parent.forward) );
            //rotor.Torque = (((1 / transform.up.y) / 0.34f) / 8.6f).Remap(0.3419972f, 1, 0, 1);
            //transform.localEulerAngles = new Vector3(0f, 0f, 90f);
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
            while (transform.eulerAngles.x > 291.5f || (transform.eulerAngles.x > -1 && transform.eulerAngles.x < 1) )
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle, 0, 0), moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.rotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }

        /**IEnumerator RotateOff()
        {

        }*/
    }
}
