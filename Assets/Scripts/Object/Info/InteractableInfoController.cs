using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.UI;
using WindTurbineVR.Object;
using System.Threading.Tasks;

namespace WindTurbineVR.Object.Interactable
{
    /*
     * Add timer behaviour to hover exit?
     * 
     */

    public class InteractableInfoController : InfoController
    {
        [Space]
        [SerializeField] protected DisplayTrigger displayTrigger;

        //HoverEnterEvent _triggerEvent;

        // Start is called before the first frame update
        public void Start()
        {
            base.Start();

            switch (displayTrigger)
            {
                case DisplayTrigger.Hover:
                    //hoverEntered.AddListener(ShowInfo);
                    //hoverExited.AddListener(DisposeUI);
                    hoverEntered.AddListener(HoverEnable);
                    hoverExited.AddListener(HoverDisable);
                    break;
                case DisplayTrigger.Selection:
                    //selectEntered.AddListener(ShowInfo);
                    selectEntered.AddListener(SelectEnable);
                    break;
            }

            ShowInfo();
            Disable();
        }

        protected void HoverEnable(HoverEnterEventArgs arg0) => Enable();

        protected void HoverDisable(HoverExitEventArgs arg0) => Disable();

        protected void SelectEnable(SelectEnterEventArgs arg0) => Enable();

        protected void SelectDisable(SelectExitEventArgs arg0) => Disable();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            switch (displayTrigger)
            {
                case DisplayTrigger.Hover:
                    //hoverEntered.RemoveListener(ShowInfo);
                    //hoverExited.RemoveListener(DisposeUI);
                    hoverEntered.RemoveListener(Enable);
                    hoverExited.RemoveListener(Disable);
                    break;
                case DisplayTrigger.Selection:
                    selectEntered.RemoveListener(SelectEnable);
                    break;
            }

            if (_uiInstance != null)
            {

            }
        }

        private void ShowInfo(SelectEnterEventArgs arg0) => ShowInfo();

        private void ShowInfo(HoverEnterEventArgs arg0) => ShowInfo();

        protected void ShowInfo()
        {
            /*if (_uiInstance == null)
            {
                _uiInstance = Instantiate(UI);
                Vector3 position = transform.position;
                _uiInstance.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
                _uiInstance.GetComponent<UIController>().ContentType = ContentType.ObjectInfo;
                _uiInstance.GetComponent<UIController>().DisplayMode = displayMode;
                _uiInstance.GetComponent<UIController>().DisplayTrigger = displayTrigger;
            }*/

            Vector3 position = transform.position;
            ShowInfo(position.y + 0.5f);
        }

        protected override void ShowInfo(float height)
        {
            Debug.Log("Show Info");
            //Debug.Log(taskList.Length);
            if (_uiInstance == null)
            {
                _uiInstance = Instantiate(UI);
                Vector3 position = new Vector3();
                if (displayMode == DisplayMode.StaticAlternative) position = (alternativeUI != null) ? alternativeUI.position : position;
                else position = new Vector3(transform.position.x, height, transform.position.z);

                _uiInstance.transform.position = position;
                UIController uic = _uiInstance.GetComponent<UIController>();
                uic.ContentType = ContentType.ObjectInfo;
                uic.DisplayMode = displayMode;
                uic.DisplayTrigger = displayTrigger;
                uic.Info = Info;
                uic.taskControllerList = taskList;
            }
        }

        /*
        private void DisposeUI(HoverExitEventArgs arg0)
        {
            if (_uiInstance != null)
            {
                DisposeUI();
            }
        }

        protected void DisposeUI()
        {
            Destroy(_uiInstance);
        }
        */
    }
}