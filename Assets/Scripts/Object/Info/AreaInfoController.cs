using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using WindTurbineVR.Object.Interactable;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object
{
    public class AreaInfoController : InfoController
    {
        /*GameObject UI;

        GameObject uiParent;

        GameObject _uiInstance;

        DisplayMode displayMode = DisplayMode.StaticPivot;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;*/

        DisplayTrigger displayTrigger;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            //UI = Resources.Load("UI") as GameObject;
            displayTrigger = DisplayTrigger.TriggerStay;

            ShowInfo();
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

        /*
        private void Enable()
        {
            base.Enable();


        }*/

        protected override void ShowInfo(float height)
        {
            if (_uiInstance == null)
            {
                _uiInstance = Instantiate(UI);
                Vector3 position = transform.position;
                _uiInstance.transform.position = new Vector3(position.x, height, position.z);
                UIController uic = _uiInstance.GetComponent<UIController>();
                uic.ContentType = ContentType.ObjectInfo;
                uic.DisplayMode = displayMode;
                uic.DisplayTrigger = displayTrigger;
                uic.AreaInfoInstance = this.gameObject;
                uic.Info = Info;
                Debug.Log("taskList" + taskList);
                Debug.Log("taskListCount" + taskList.Count);
                Debug.Log("taskList0: " + taskList[0].Description);
                uic.taskControllerList = taskList;
            }
        }
    }
}
