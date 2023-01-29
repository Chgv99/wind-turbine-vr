using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Interactable
{
    /*
     * Add timer behaviour to hover exit?
     * 
     */

    public class InteractableInfo : XRSimpleInteractable
    {
        [Space]
        [SerializeField] GameObject UI;

        [Space]
        [SerializeField] DisplayTrigger displayTrigger;

        [Space]
        [SerializeField] DisplayMode displayMode;

        GameObject _uiInstance;

        HoverEnterEvent _triggerEvent;

        // Start is called before the first frame update
        void Start()
        {
            UI = Resources.Load("UI") as GameObject;

            switch (displayTrigger)
            {
                case DisplayTrigger.Hover:
                    hoverEntered.AddListener(ShowInfo);
                    hoverExited.AddListener(DisposeUI);
                    break;
                case DisplayTrigger.Selection:
                    selectEntered.AddListener(ShowInfo);
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            switch (displayTrigger)
            {
                case DisplayTrigger.Hover:
                    hoverEntered.RemoveListener(ShowInfo);
                    hoverExited.RemoveListener(DisposeUI);
                    break;
                case DisplayTrigger.Selection:
                    selectEntered.RemoveListener(ShowInfo);
                    break;
            }

            if (_uiInstance != null)
            {

            }
        }

        private void ShowInfo(SelectEnterEventArgs arg0) => ShowInfo();

        private void ShowInfo(HoverEnterEventArgs arg0) => ShowInfo();

        private void ShowInfo()
        {
            if (_uiInstance == null)
            {
                _uiInstance = Instantiate(UI);
                Vector3 position = transform.position;
                _uiInstance.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
                _uiInstance.GetComponent<UIController>().ContentType = ContentType.ObjectInfo;
                _uiInstance.GetComponent<UIController>().DisplayMode = displayMode;
                _uiInstance.GetComponent<UIController>().DisplayTrigger = displayTrigger;
            }
        }

        private void DisposeUI(HoverExitEventArgs arg0)
        {
            if (_uiInstance != null)
            {
                Destroy(_uiInstance);
            }
        }
    }
}