using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TurbineDataController : MonoBehaviour
    {
        TurbineData td;

        #region private
        bool active;
        Quaternion rotorRotation;
        Quaternion nacelleRotation;
        #endregion

        public bool Active { get => active; set => active = value; }
        public Quaternion RotorRotation { get => rotorRotation; set => rotorRotation = value; }
        public Quaternion NacelleRotation { get => nacelleRotation; set => nacelleRotation = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}