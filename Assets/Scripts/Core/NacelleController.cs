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
                Debug.Log("rotating. progress: " + progress);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    new Quaternion(transform.rotation.x, endRotation.y, transform.rotation.z, transform.rotation.w), //endRotation,
                    progress// * 2
                );
            }
        }
    }
}
