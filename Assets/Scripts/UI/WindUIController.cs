using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class WindUIController : MonoBehaviour
    {
        Transform camera;

        // Start is called before the first frame update
        void Start()
        {
            camera = GameObject.Find("SceneController").GetComponent<SceneController>().Camera;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = 
                Quaternion.LookRotation(
                    transform.position + 
                    new Vector3(
                        camera.position.x, 
                        transform.position.y,
                        camera.position.z));
        }
    }
}
