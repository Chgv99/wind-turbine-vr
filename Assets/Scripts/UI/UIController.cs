using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WindTurbineVR.Core;
//using WindTurbineVR.Object;

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
        Selection,
        TriggerStay
    }

    public enum ContentType
    {
        None = 0,
        SimpleSign,
        ObjectInfo,
        Menu
    }

    public class UIController : MonoBehaviour
    {
        GameObject infoModal;
        GameObject areaInfoInstance;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        Info info;

        bool initializated = false;

        bool immortal = false;

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get => displayTrigger; set => displayTrigger = value; }
        public Info Info { get => info; set => info = value; }

        public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }

        private class DirectionController
        {
            Transform camera;
            Transform ui;

            DisplayMode displayMode;

            public Transform UI { get => ui; set => ui = value; }
            public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

            public DirectionController(Transform transform, DisplayMode displayMode)
            {
                this.UI = transform;
                this.DisplayMode = displayMode;

                camera = GameObject.Find("SceneController").GetComponent<SceneController>().camera;
            }

            public void SetDirection()
            {
                switch (DisplayMode)
                {
                    case DisplayMode.Static:
                        break;
                    case DisplayMode.StaticPivot:
                        Debug.Log("Setting direction to " + (UI.position - camera.position));
                        UI.rotation = Quaternion.LookRotation(UI.position - camera.position);
                        break;
                    case DisplayMode.FrontPivot:
                        break;
                }
            }
        }

        void Awake()
        {
            GetComponent<Canvas>().enabled = false;
        }

        void Start()
        {
            infoModal = Resources.Load("InfoModal") as GameObject;
            dc = new DirectionController(transform, DisplayMode);
            dc.SetDirection();
            SetContent();
            GetComponent<Canvas>().enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            //dc = new DirectionController(transform, displayMode);

            if (!initializated)
            {
                
            }

            dc.SetDirection();
        }

        public void SetContent()
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

        private void ShowObjectInfo()
        {
            GameObject infoModalInstance = Instantiate(infoModal, transform);
            if (DisplayTrigger != DisplayTrigger.Hover) infoModalInstance.GetComponent<ModalController>().InstantiateCloseButton();
            /*if (DisplayTrigger != DisplayTrigger.TriggerStay && AreaInfoInstance != null)
            {
                infoModalInstance.GetComponent<ModalController>().InstantiateCloseButton(AreaInfoInstance);
            }*/
            infoModalInstance.GetComponent<ModalController>().SetContent(Info.Title, Info.Description);
        }

        public void Dispose()
        {
            if (AreaInfoInstance != null) Destroy(AreaInfoInstance);
            Destroy(gameObject);
        }
    }
}