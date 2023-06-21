using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateControlController : MonoBehaviour
{
    [SerializeField] Transform reference;

    // Update is called once per frame
    void Update()
    {
        transform.position = reference.position;
    }
}
