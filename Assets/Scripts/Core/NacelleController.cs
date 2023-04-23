using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WindTurbineVR.Core
{
    public class NacelleController : MonoBehaviour
    {
        //Transform nacelle;

        //bool rotate

        #region Internal rotational values
        bool doRotate = false;
        float progress;
        Quaternion startRotation;
        Quaternion endRotation;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void Rotate(Quaternion rotation) => Rotate(transform.rotation, rotation);

        void Rotate(Quaternion startRotation, Quaternion endRotation)
        {
            Debug.Log("Rotate (NacelleController). Start and end rotation = " + startRotation + " to " + endRotation);

            this.startRotation = startRotation;
            this.endRotation = endRotation;

            doRotate = true;
            progress = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (doRotate)
            {
                progress += Time.deltaTime;

                Vector3 newDirection = new Vector3(transform.rotation.eulerAngles.x, endRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                Quaternion objective = Quaternion.Euler(newDirection); //new Quaternion(transform.rotation.x, endRotation.y, transform.rotation.z, transform.rotation.w), //endRotation,

                Debug.Log("rotating. progress: " + progress);
                Debug.Log("start: " + startRotation);
                Debug.Log("current: " + transform.rotation);
                Debug.Log("end: " + objective);
                //Debug.Log(new Quaternion(transform.rotation.x, endRotation.y, transform.rotation.z, transform.rotation.w));
                
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    objective,
                    progress * 0.0001f
                );
            }
        }
    }
}
