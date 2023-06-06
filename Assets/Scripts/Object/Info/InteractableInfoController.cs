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

namespace WindTurbineVR.Object.Info
{
    /*
     * Add timer behaviour to hover exit?
     * 
     */

    public class InteractableInfoController : InfoController
    {
        // Start is called before the first frame update
        public void Start()
        {
            prefabUI = Resources.Load("UI/InteractableUI") as GameObject;
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

            CreateUI();
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

            if (UIInstance != null)
            {
                UIInstance = null;
            }
        }

        //private void CreateUI(SelectEnterEventArgs arg0) => CreateUI();

        //private void CreateUI(HoverEnterEventArgs arg0) => CreateUI();


        // Commented because behaviour was
        // not different from parent class'
        /*protected void CreateUI(float height)
        {
            Debug.Log("Show Info");
            if (_uiInstance != null) return;

            
        }*/
    }
}