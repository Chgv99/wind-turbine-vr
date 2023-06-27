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
using WindTurbineVR.Data;

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

            //enabled = gameObject.activeSelf;

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
                    selectEntered.AddListener(SwitchActiveState);
                    break;
            }

            CreateUI();
            Disable();
        }

        protected void HoverEnable(HoverEnterEventArgs arg0) => Enable();

        protected void HoverDisable(HoverExitEventArgs arg0) => Disable();

        protected void SwitchActiveState(SelectEnterEventArgs arg0)
        {
            if (UIInstance.GetComponent<InteractableInfoView>().IsActive()) Disable();
            else Enable();
        }
        //protected void SelectEnable(SelectEnterEventArgs arg0) => Enable();

        //protected void SelectDisable(SelectExitEventArgs arg0) => Disable();

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
                    selectEntered.RemoveListener(SwitchActiveState);
                    break;
            }

            if (UIInstance != null)
            {
                UIInstance = null;
            }
        }

        protected override void CreateUI(float height)
        {
            if (UIInstance != null) return;

            base.CreateUI(height);

            //UIInstance.GetComponent<InteractableInfoView>().UpdateContent(Info.Title, Info.Description, "test");
            UIInstance.GetComponent<InteractableInfoView>().SetTitle(Info.Title);
            UIInstance.GetComponent<InteractableInfoView>().SetBody(Info.Description != new string[] { } ? Info.Description[0] : gameObject.name + " description");
            UIInstance.GetComponent<InteractableInfoView>().SetUrl(Info.Video);
            //UIInstance.GetComponent<InteractableInfoView>().UpdateColor(color);
        }

        protected void NextPage()
        {

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