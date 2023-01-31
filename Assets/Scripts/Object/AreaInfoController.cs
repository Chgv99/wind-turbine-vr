using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name == "XR Origin")
            {
                ShowInfo(other.transform.Find("CameraOffset/Main Camera").position.y);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name == "XR Origin") DisposeUI();
        }

        protected override void ShowInfo(float height)
        {
            if (_uiInstance == null)
            {
                _uiInstance = Instantiate(UI);
                Vector3 position = transform.position;
                _uiInstance.transform.position = new Vector3(position.x, height, position.z);
                _uiInstance.GetComponent<UIController>().ContentType = ContentType.ObjectInfo;
                _uiInstance.GetComponent<UIController>().DisplayMode = displayMode;
                _uiInstance.GetComponent<UIController>().DisplayTrigger = displayTrigger;
                _uiInstance.GetComponent<UIController>().AreaInfoInstance = this.gameObject;
                _uiInstance.GetComponent<UIController>().Info = Info;
            }
        }
    }
}
