using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

namespace WindTurbineVR.Core
{
    /** TODO: RENAME CONTROLLER TO YAW CONTROLLER */

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
        Quaternion directionObjective;
        #endregion

        public Quaternion DirectionObjective { get => directionObjective; set => directionObjective = value; }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        /*
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

            this.startRotation = startRotation;
            this.endRotation = endRotation;

            doRotate = true;
            progress = 0;
        }*/

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        // Update is called once per frame
        /*void Update()
        {
            Debug.Log("NacelleController");
            Vector3 current = transform.rotation * Vector3.forward;
            Vector3 objective = directionObjective * Vector3.forward;

            if (Vector3.Angle(current, objective) > 5)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, directionObjective, 0.01f);
            }
        }*/

        void FixedUpdate()
        {
            //Debug.Log("NacelleController");
            Vector3 current = transform.rotation * Vector3.forward;
            Vector3 objective = directionObjective * Vector3.forward;
            if (Vector3.Angle(current, objective) > 5)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, directionObjective, 0.01f);
            }
        }
    }
}
