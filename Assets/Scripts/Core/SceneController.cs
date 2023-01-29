using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class SceneController : MonoBehaviour
    {
        public Transform xrOrigin;
        public Transform camera;

        // Start is called before the first frame update
        void Start()
        {
            Exception nullExc = new Exception("Variable not set to an instance of an object.");
            //Debug.LogException("xrOrigin is not set", transform);
            if (xrOrigin == null) Debug.LogException(nullExc);
            if (camera == null) Debug.LogException(nullExc);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
