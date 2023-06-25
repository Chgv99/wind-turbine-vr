using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR
{
    public class CollisionDetector : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(gameObject.name + "collided with " + collision.gameObject.name);
        }
    }
}
