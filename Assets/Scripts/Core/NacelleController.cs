using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

namespace WindTurbineVR.Core
{
    public class NacelleController : MonoBehaviour
    {
        //Transform nacelle;

        //bool rotate

        VaneController vc;

        #region Internal rotational values
        float threshold = 90f;

        bool doRotate = false;
        float progress;
        Quaternion startRotation;
        Quaternion endRotation;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void Rotate(Quaternion rotation, VaneController vc)
        {
            this.vc = vc;
            Rotate(rotation);
        }

        public void Rotate(Quaternion rotation) => Rotate(transform.rotation, rotation);

        void Rotate(Quaternion startRotation, Quaternion endRotation)
        {
            Vector3 startVector = startRotation * Vector3.forward;
            Vector3 endVector = new Vector3(transform.rotation.eulerAngles.x, endRotation.eulerAngles.y, transform.rotation.eulerAngles.z); // endRotation * Vector3.forward;
            //endVector = new Vector3(transform.rotation.eulerAngles.x, endRotation.eulerAngles.y, transform.rotation.eulerAngles.z); // endRotation * Vector3.forward;
            //endVector = new Vector3(transform.rotation.eulerAngles.)
            endRotation = Quaternion.Euler(endVector); //new Quaternion(transform.rotation.x, endRotation.y, transform.rotation.z, transform.rotation.w), //endRotation,

            //startVector.
            //Debug.Log("startVector")

            /*float diffAngle = Vector3.Angle(startVector, endVector);
            Debug.Log("DiffAngle: " + diffAngle);
            if (diffAngle > threshold)
            {
                
            }*/

            Debug.Log("Rotate (NacelleController). Start and end rotation = " + startRotation + " to " + endRotation);

            this.startRotation = startRotation;
            this.endRotation = endRotation;

            doRotate = true;
            progress = 0;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            //if (doRotate)
            {
                progress += Time.deltaTime;

                /*Debug.Log("rotating. progress: " + progress);
                Debug.Log("start: " + startRotation);
                Debug.Log("current: " + transform.rotation);
                Debug.Log("end: " + endRotation);*/
                //Debug.Log(new Quaternion(transform.rotation.x, endRotation.y, transform.rotation.z, transform.rotation.w));
                
                transform.rotation = Quaternion.Lerp(
                    transform.rotation, //transform.rotation
                    vc.WindDirection,//endRotation,
                    progress * 0.0005f
                );
            }
        }
    }
}
