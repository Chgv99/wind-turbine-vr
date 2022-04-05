using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachineController : MonoBehaviour
{
    //GAMEOBJECTS
    [SerializeField] GameObject counter;
    [SerializeField] GameObject light;
    [SerializeField] GameObject aguja;
    [SerializeField] GameObject cubo;

    [SerializeField] Transform pot;
    [SerializeField] public float potMinMax = 45;

    [SerializeField] float volts;
    [SerializeField] float minVolts = 1;
    [SerializeField] float maxVolts = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*float angle = pot.transform.localEulerAngles.y;
        Debug.Log("(0) " + angle);
        if (angle > 270) angle = - (360 - pot.transform.localEulerAngles.y);
        Debug.Log("(1) " + angle);
        volts = (angle).Remap(-potMinMax, potMinMax, minVolts, maxVolts);
        Debug.Log("(2) " + volts);
        counter.GetComponent<TextMeshPro>().text = (float)Math.Round(volts, 2) + "v";
        aguja.GetComponent<AgujaController>().SetLevel(volts, minVolts, maxVolts);
        light.GetComponent<LightController>().SetVolts(volts); //volts.Remap(1, 100, 1, 2)*/
    }
}
