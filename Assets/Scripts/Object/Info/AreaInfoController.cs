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

        //[SerializeField] bool guideMode = false;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            //UI = Resources.Load("UI") as GameObject;
            displayTrigger = DisplayTrigger.TriggerStay;

            CreateUI();
            //Disable();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name != "XR Origin") return;

            if (UiInstance == null) return;

            //ShowInfo(other.transform.Find("CameraOffset/Main Camera").position.y);
            Enable();
            float height = other.transform.Find("CameraOffset/Main Camera").position.y;
            UiInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
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
            if (UiInstance != null) return;

            base.CreateUI(height);
            UiInstance.GetComponent<UIController>().AreaInfoInstance = this.gameObject;
            /*if (guideMode)
            {
                Debug.Log("guide mode enabled");
                _uiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
                _uiInstance.GetComponent<UIController>().taskControllerList = taskList;
            }*/
        }
    }
}
