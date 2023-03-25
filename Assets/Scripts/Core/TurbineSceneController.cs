using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace WindTurbineVR.Core
{
    public class TurbineSceneController : SceneController
    {
        private UnityEvent taskChecked;

        public UnityEvent TaskChecked { get => taskChecked; }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            taskChecked = new UnityEvent();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
