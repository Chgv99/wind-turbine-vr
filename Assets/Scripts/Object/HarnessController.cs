using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WindTurbineVR.Core;

namespace WindTurbineVR
{
    public class HarnessController : MonoBehaviour
    {
        TurbineSceneController sceneController;

        [SerializeField] Transform mainCamera;

        UnityEvent harnessAttached;
        UnityEvent harnessDetached;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            harnessAttached = sceneController.HarnessAttached;
            harnessDetached = sceneController.HarnessDetached;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = mainCamera.position + new Vector3(0, -0.5f, 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
        }

        public void CallHarnessAttached()
        {
            harnessAttached?.Invoke();
        }

        public void CallHarnessDetached()
        {
            harnessDetached?.Invoke();
        }
    }
}
