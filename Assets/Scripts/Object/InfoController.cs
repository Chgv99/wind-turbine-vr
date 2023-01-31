using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object
{
    /*
     * Add timer behaviour to hover exit?
     * 
     */

    [RequireComponent(typeof(Info))]
    public abstract class InfoController : XRSimpleInteractable
    {
        protected GameObject UI;
        protected GameObject _uiInstance;

        Info info;

        [Space]
        [SerializeField] protected DisplayMode displayMode;

        HoverEnterEvent _triggerEvent;

        public Info Info { get => info; set => info = value; }

        // Start is called before the first frame update
        public void Start()
        {
            UI = Resources.Load("UI") as GameObject;
            Info = GetComponent<Info>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_uiInstance != null)
            {

            }
        }

        protected void ShowInfo()
        {
            Vector3 position = transform.position;
            ShowInfo(position.y + 0.5f);
        }

        protected abstract void ShowInfo(float height);

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
    }
}