using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR
{
    public class PlayerParentingController : MonoBehaviour
    {
        [SerializeField] Transform newParent;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Parenting collision: " + other.gameObject.name);
            other.transform.parent = newParent;
        }
    }
}
