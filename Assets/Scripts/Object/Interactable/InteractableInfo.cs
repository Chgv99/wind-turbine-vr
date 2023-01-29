using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Interactable
{
    public class InteractableInfo : XRSimpleInteractable
    {
        [Space]
        [SerializeField] GameObject UI;

        GameObject uiInstance;

        // Start is called before the first frame update
        void Start()
        {
            UI = Resources.Load("UI") as GameObject;

            hoverEntered.AddListener(ShowInfo);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            hoverEntered.RemoveListener(ShowInfo);
        }

        private void ShowInfo(HoverEnterEventArgs arg0)
        {
            if (uiInstance == null)
            {
                uiInstance = Instantiate(UI);
                Vector3 position = transform.position;
                uiInstance.transform.position = new Vector3(position.x, position.y + 0.5f, position.z);
                uiInstance.GetComponent<UIController>().Mode = Mode.ObjectInfo;
            }
        }
    }
}