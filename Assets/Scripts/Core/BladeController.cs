using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class BladeController : MonoBehaviour
    {
        ClimateController climate;
        RotorController rotor;

        float contribution = 0f;

        public float Contribution { get => contribution; set => contribution = value; }

        Transform center;

        [SerializeField] bool debug;

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("ClimateController").GetComponent<ClimateController>();
            rotor = transform.parent.parent.GetComponent<RotorController>();

            if (rotor == null) Error.LogException("RotorController not found");

            TurnOn();


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
            //Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(transform.up.normalized, Vector3.left) * 50, Color.green);
            ///Wind
            Debug.DrawRay(center.position, -climate.Wind.normalized * 10, Color.blue);
            Debug.DrawRay(center.position, climate.Wind.normalized * 10, Color.white);


            Debug.Log("------------");
            Debug.Log("Blade vector: " + transform.up.normalized);
            Debug.Log("Wind vector: " + climate.Wind.normalized);
            Debug.Log("------------");
            Debug.Log("blade angle = " + Vector3.Angle(transform.up, -Vector3.forward));
            Debug.Log("wind angle = " + Vector3.Angle(-climate.Wind, -Vector3.forward));
            Debug.Log("Blade-Wind angle = " + bwangle);
            Debug.Log("Blade-Wind angle cosine = " + Mathf.Cos(bwangle));
            Debug.Log("Blade-Wind angle difference unit = " + bwangle.Remap(90, 20, 0, 1));


            Debug.Log("contribution: " + 16.66f * bwangle.Remap(90, 20, 0, 1));
        }

        // Update is called once per frame
        void Update()
        {
            float bwangle = Vector3.Angle(-climate.Wind, transform.up);
            if (debug) DrawDebug(bwangle);
            Contribution = 16.66f * bwangle.Remap(90, 20, 0, 1);//transform.localEulerAngles.x.Remap(360, 290, 0, 16.66f);

            /*Debug.Log("blade angle: " + transform.localEulerAngles.x); //max (min) is 290
            Debug.Log("torque sent (1): " + transform.localEulerAngles.x.Remap(360, 290, 0, 1));
            Debug.Log("rotor's negative blue: " + (-transform.parent.parent.forward) );
            rotor.Torque = transform.localEulerAngles.x.Remap(360, 290, 0, 3);*/
        }

        private void FixedUpdate()
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

        IEnumerator RotateOff()
        {
            float moveSpeed = 0.1f;
            float angle = 0;

            // Stops rotation when the angles are near to the angle objective (0)
            while (transform.localEulerAngles.x < -1.5f /*|| (transform.localEulerAngles.x > -1 && transform.localEulerAngles.x < 1)*/)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angle, 0, 0), moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }
    }
}
