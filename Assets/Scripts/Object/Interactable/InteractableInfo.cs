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
    public class InteractableInfo : XRSimpleInteractable
    {
        [Space]
        [SerializeField] GameObject UI;

        [Space]
        [SerializeField] DisplayTrigger displayTrigger;

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
                    break;
                case DisplayTrigger.Selection:
                    selectEntered.RemoveListener(ShowInfo);
                    break;
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
                _uiInstance.GetComponent<UIController>().Mode = Mode.ObjectInfo;
            }
        }
    }
}