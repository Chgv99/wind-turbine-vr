using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.UI;

public class TurbineMonitorController : MonoBehaviour
{
    [SerializeField] TurbineController turbine;

    TurbineMonitorView view;
    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<TurbineMonitorView>();
    }

    void UpdateAngleNacelle()
    {
        Debug.DrawRay(turbine.nc.transform.position, Vector3.forward, Color.red, 1);
        Debug.DrawRay(turbine.nc.transform.position, turbine.nc.transform.forward * -1, Color.blue, 1);

        //Vector3 direction = turbine.nc.transform.rotation * Vector3.forward;
        float angle = Vector3.SignedAngle(turbine.nc.transform.forward * -1, Vector3.forward, Vector3.down);
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        //float angle = turbine.nc.transform.rotation.eulerAngles.y;
        view.AngleNacelle = angle;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateAngleNacelle();
    }
}
