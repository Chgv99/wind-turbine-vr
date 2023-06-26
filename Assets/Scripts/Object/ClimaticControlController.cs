using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

public class ClimaticControlController : MonoBehaviour
{
    [SerializeField] ClimateController climateController;

    [SerializeField] Transform reference;

    Transform pivot;

    void Awake()
    {
        pivot = transform.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = reference.position;
        SetWind(pivot.localRotation);
    }

    void SetWind(Quaternion rotation)
    {
        climateController.SetWind(rotation);
    }
}
