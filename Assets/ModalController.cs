using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WindTurbineVR.UI;

namespace WindTurbineVR
{
    public class ModalController : MonoBehaviour
    {
        [SerializeField]
        Button closeButton;

        UIController uiCon;

        // Start is called before the first frame update
        void Awake()
        {
            uiCon = transform.parent.GetComponent<UIController>();
            closeButton = transform.Find("CloseButton").GetComponent<Button>();

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(uiCon.Dispose);
            }
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(uiCon.Dispose);
        }

        // Update is called once per frame
        void Update()
        {

        }        
    }
}
