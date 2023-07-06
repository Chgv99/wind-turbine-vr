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

        //float angle = turbine.nc.transform.rotation.eulerAngles.y;
        view.Status = turbine.Active;
        view.AngleNacelle = turbine.nc.Angle;
        view.AngleBlades = turbine.rc.BladeAngle;
        view.RotorVelocity = turbine.rc.AngularVelocity;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateAngleNacelle();
    }
}
