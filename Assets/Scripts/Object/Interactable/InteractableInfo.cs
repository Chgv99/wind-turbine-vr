using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Object.Interactable
{
    public class InteractableInfo : XRSimpleInteractable
    {
        [Space]
        [SerializeField] GameObject UI;

        // Start is called before the first frame update
        void Start()
        {
            hoverEntered.AddListener(ShowInfo);
        }

        private void ShowInfo(HoverEnterEventArgs arg0)
        {
            Debug.Log("Showing info about " + gameObject.name);
            GameObject modalInstance = Instantiate(UI, transform);
        }
    }
}