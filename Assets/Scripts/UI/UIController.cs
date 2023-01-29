using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public enum DisplayMode
    {
        Static,
        StaticPivot,
        FrontPivot
    }

    public enum DisplayTrigger
    {
        Hover,
        Selection
    }

    public enum ContentType
    {
        None = 0,
        ObjectInfo,
        Menu
    }

    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject infoModal;

        DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        bool initializated = false;

        bool immortal = false;

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get => displayTrigger; set => displayTrigger = value; }

        void Awake()
        {
            infoModal = Resources.Load("InfoModal") as GameObject;
        }

        // Update is called once per frame
        void Update()
        {
            dc = new DirectionController(transform, displayMode);

            if (!initializated)
            {
                if (contentType == ContentType.None) return;

                initializated = true;

                switch (contentType)
                {
                    case ContentType.ObjectInfo:
                        ShowObjectInfo();
                        break;
                }
            }

            dc.SetDirection();
        }

        private void ShowObjectInfo()
        {
            GameObject infoModalInstance = Instantiate(infoModal, transform);
            if (displayTrigger != DisplayTrigger.Hover) infoModalInstance.GetComponent<ModalController>().InstantiateCloseButton();
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }

    public class DirectionController
    {
        Transform ui;
        DisplayMode displayMode;

        Transform camera;

        public DirectionController(Transform transform, DisplayMode displayMode)
        {
            this.ui = transform;
            this.displayMode = displayMode;

            camera = GameObject.Find("SceneController").GetComponent<SceneController>().camera;
        }

        public void SetDirection()
        {
            switch (displayMode)
            {
                case DisplayMode.Static:
                    break;
                case DisplayMode.StaticPivot:
                    ui.rotation = Quaternion.LookRotation(ui.position - camera.position);
                    break;
                case DisplayMode.FrontPivot:
                    break;
            }
        }
    }
}