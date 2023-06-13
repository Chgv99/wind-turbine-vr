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
        VaneController vc;

        [SerializeField] Transform littleCog;

        [SerializeField] [Range(0, -0.1f)] float littleSpeed = -0.08f;

        #region Internal rotational values
        float threshold = 90f;

        bool doRotate = false;
        float progress;
        Quaternion startRotation;
        Quaternion endRotation;
        Quaternion directionObjective;
        #endregion

        public Quaternion DirectionObjective { get => directionObjective; set => directionObjective = value; }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        void FixedUpdate()
        {
            //Debug.Log("NacelleController");
            Vector3 current = transform.rotation * Vector3.forward;
            Vector3 objective = directionObjective * Vector3.forward;
            if (Vector3.Angle(current, objective) > 5)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, directionObjective, 0.01f);
                Vector3 forwardA = transform.rotation * Vector3.forward;
                Vector3 forwardB = directionObjective * Vector3.forward;
                float angle = Vector3.SignedAngle(forwardA, forwardB, Vector3.up);
                //float angle = Quaternion.Angle(transform.rotation, directionObjective); 
                littleCog.Rotate(new Vector3(0, 1, 0), ((angle < 0) ? 1 : -1) * littleSpeed);
            }
        }
    }
}
