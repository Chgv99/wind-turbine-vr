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
