using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR
{
    public class CameraRigController : MonoBehaviour
    {
        [SerializeField] [Range(0.025f, 0.2f)] float speed = 0.025f;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 1 * speed, 0);
        }
    }
}
