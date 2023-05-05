using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR
{
    public class BackgroundTurbineController : MonoBehaviour
    {
        Rigidbody rotor;

        [SerializeField] [Range(78,120)] float velocity = 78;

        // Start is called before the first frame update
        void Start()
        {
            rotor = transform.Find("Rotor").GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Quaternion deltaRotation = Quaternion.Euler(
                new Vector3(0,0,velocity) * Time.fixedDeltaTime);

            rotor.MoveRotation(rotor.rotation * deltaRotation);
        }
    }
}
