using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WindTurbineVR.UI
{
    public class ModalController : MonoBehaviour
    {
        [SerializeField] GameObject button;

        [SerializeField]
        GameObject buttonInstance;

        UIController uiCon;

        // Start is called before the first frame update
        void Awake()
        {
            uiCon = transform.parent.GetComponent<UIController>();
            button = Resources.Load("Button") as GameObject;

            /*closeButton = transform.Find("CloseButton").GetComponent<Button>();

            //Modal needs to be disposed somehow unless undispossable
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(uiCon.Dispose);
            }*/
        }

        public void InstantiateCloseButton()
        {
            buttonInstance = Instantiate(button, transform);
            buttonInstance.transform.Find("Text").GetComponent<Text>().text = "Close";
            buttonInstance.GetComponent<Button>().onClick.AddListener(uiCon.Dispose);
        }

        private void OnDestroy()
        {
            if (buttonInstance != null) 
                buttonInstance.GetComponent<Button>().onClick.RemoveListener(uiCon.Dispose);
        }

        // Update is called once per frame
        void Update()
        {

        }        
    }
}
