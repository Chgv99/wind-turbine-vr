using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using WindTurbineVR.Object.Interactable;
using WindTurbineVR.UI;
using WindTurbineVR.Data;

namespace WindTurbineVR.Object.Info
{
    public class AreaInfoController : InfoController
    {
        /*GameObject UI;

        GameObject uiParent;

        GameObject _uiInstance;

        DisplayMode displayMode = DisplayMode.StaticPivot;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;*/

        //DisplayTrigger displayTrigger;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            //UI = Resources.Load("UI") as GameObject;
            displayTrigger = DisplayTrigger.TriggerStay;

            CreateUI();
            Disable();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name != "XR Origin") return;

            if (_uiInstance == null) return;

            //ShowInfo(other.transform.Find("CameraOffset/Main Camera").position.y);
            Enable();
            float height = other.transform.Find("CameraOffset/Main Camera").position.y;
            _uiInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name != "XR Origin") return;

            //DisposeUI();
            Disable();
        }

        protected override void CreateUI(float height)
        {
            if (_uiInstance != null) return;

            base.CreateUI(height);
            _uiInstance.GetComponent<UIController>().AreaInfoInstance = this.gameObject;
        }
    }
}
