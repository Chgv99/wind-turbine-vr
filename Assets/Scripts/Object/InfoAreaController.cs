using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Object.Interactable;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object
{
    public class InfoAreaController : InteractableInfo
    {
        /*GameObject UI;

        GameObject uiParent;

        GameObject _uiInstance;

        DisplayMode displayMode = DisplayMode.StaticPivot;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;*/

        // Start is called before the first frame update
        void Start()
        {
            UI = Resources.Load("UI") as GameObject;

            //base.Start();
            if (displayTrigger == DisplayTrigger.TriggerStay)
            {

            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name == "XR Origin")
            {
                ShowInfo(other.transform.Find("CameraOffset/Main Camera").position.y);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("collider: " + other.gameObject.name);
            if (other.gameObject.name == "XR Origin") DisposeUI();
        }
    }
}
