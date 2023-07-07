using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WindTurbineVR.Core
{
    public class BladeController : MonoBehaviour
    {
        ClimateController climate;
        RotorController rotor;

        #region private
        float contribution = 0f;
        float angleOn = -70;
        float angleOff = 0;
        float moveSpeed = 0.1f;
        #endregion

        Transform center;

        [SerializeField] bool debug;

        float angle;

        public float Contribution { get => contribution; set => contribution = value; }
        public float Angle { get => angle; set => angle = value; }

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("Climate").GetComponent<ClimateController>();
            rotor = transform.parent.parent.GetComponent<RotorController>();

            if (rotor == null) Error.LogException("RotorController not found");

            //TurnOn();


            center = transform.Find("Center");
        }

        IEnumerator TestRoutine()
        {
            yield return new WaitForSecondsRealtime(10);
            TurnOff();
        }

        void DrawDebug(float bwangle)
        {
            // Debug Rays
            /// Z AXIS REFERENCE
            Debug.DrawRay(transform.position, -Vector3.forward.normalized * 100, Color.red);
            ///Blade
            Debug.DrawRay(center.position, transform.up.normalized * 10, new Color(0, 50, 0));
            Debug.DrawRay(center.position, transform.right.normalized * 10, new Color(50, 0, 50));
            Debug.DrawRay(center.position, transform.parent.up.normalized * 10, new Color(50, 50, 0));
            ///Wind
            Debug.DrawRay(center.position, -climate.Wind.normalized * 10, Color.blue);
            Debug.DrawRay(center.position, climate.Wind.normalized * 10, Color.white);

            /*
            Debug.Log("------------");
            Debug.Log("Blade vector: " + transform.up.normalized);
            Debug.Log("Wind vector: " + climate.Wind.normalized);
            Debug.Log("------------");
            Debug.Log("blade angle = " + Vector3.Angle(transform.up, -Vector3.forward));
            Debug.Log("wind angle = " + Vector3.Angle(-climate.Wind, -Vector3.forward));
            Debug.Log("Blade-Wind angle = " + bwangle);
            Debug.Log("Blade-Wind angle cosine = " + Mathf.Cos(bwangle));
            Debug.Log("Blade-Wind angle difference unit = " + bwangle.Remap(90, 20, 0, 1));
            */

            //Debug.Log("contribution: " + 16.66f * bwangle.Remap(90, 20, 0, 1));
        }

        // Update is called once per frame
        void Update()
        {
            angle = Vector3.SignedAngle(transform.up, transform.parent.up, transform.right);

            //Debug.Log("Climate Wind (Blade Controller) = " + climate.Wind);
            /*float bwangle = Vector3.Angle(climate.WindDirection * -Vector3.forward, transform.up);
            if (debug) DrawDebug(bwangle);
            Contribution = 16.66f * bwangle.Remap(90, 20, 0, 1);*/
            //transform.localEulerAngles.x.Remap(360, 290, 0, 16.66f);

            /*Debug.Log("blade angle: " + transform.localEulerAngles.x); //max (min) is 290
            Debug.Log("torque sent (1): " + transform.localEulerAngles.x.Remap(360, 290, 0, 1));
            Debug.Log("rotor's negative blue: " + (-transform.parent.parent.forward) );
            rotor.Torque = transform.localEulerAngles.x.Remap(360, 290, 0, 3);*/
        }

        private void FixedUpdate()
        {
            float bwangle = Vector3.Angle(climate.WindDirection * -Vector3.forward, transform.up);
            //Debug.Log("bwangle: " + bwangle);
            if (debug) DrawDebug(bwangle);
            Contribution = climate.WindSpeed * bwangle.Remap(90, 20, 0, 1);
            //Debug.Log(gameObject.name + " contribution: " + Contribution);
        }

        public void OverrideTurnOn()
        {
            transform.localRotation = Quaternion.Euler(angleOn, 0, 0);
        }

        public void OverrideTurnOff()
        {

        }

        public void TurnOn() => CoroutineCheck(RotateOn());

        public void TurnOff() => CoroutineCheck(RotateOff());

        void CoroutineCheck(IEnumerator coroutine)
        {
            StopAllCoroutines();
            StartCoroutine(coroutine);
        }

        IEnumerator RotateOn()
        {
            // Stops rotation when the angles are near to the angle objective (-70 aka 290)
            // Initial angle is less than zero (approximately zero) at first, but then is quickly converted to a positive angle.
            // That's why we need the part after the OR operand. Positive check just in case.
            while (transform.localEulerAngles.x > 291.5f || (transform.localEulerAngles.x > -1 && transform.localEulerAngles.x < 1) )
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angleOn, 0, 0), moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(angleOn, 0, 0);
            yield return null;
        }

        IEnumerator RotateOff()
        {
            // Stops rotation when the angles are near to the angle objective (0)
            while (transform.localEulerAngles.x < -1.5f /*|| (transform.localEulerAngles.x > -1 && transform.localEulerAngles.x < 1)*/)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angleOff, 0, 0), moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(angleOff, 0, 0);
            yield return null;
        }
    }
}
