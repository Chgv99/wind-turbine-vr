using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class ClimateControlPanelController : MonoBehaviour
    {
        ClimateController climate;

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("ClimateController").GetComponent<ClimateController>();
            if (climate == null) Error.LogException("ClimateController not found");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
