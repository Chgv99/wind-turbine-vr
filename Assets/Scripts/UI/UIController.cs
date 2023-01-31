using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.Object;

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
        [SerializeField] GameObject infoModal;
        GameObject areaInfoInstance;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;

        Info info;

        bool initializated = false;

        bool immortal = false;

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get; set; } = DisplayTrigger.Hover;
        public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }
        public Info Info { get => info; set => info = value; }

        void Awake()
        {
            GetComponent<Canvas>().enabled = false;
        }

        void Start()
        {
            infoModal = Resources.Load("InfoModal") as GameObject;
            Debug.Log("displayMode" + displayMode);
            dc = new DirectionController(transform, displayMode);
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

    public class DirectionController
    {
        Transform ui;
        Transform camera;

        DisplayMode displayMode;

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
                    Debug.Log("Setting direction to " + (ui.position - camera.position));
                    ui.rotation = Quaternion.LookRotation(ui.position - camera.position);
                    break;
                case DisplayMode.FrontPivot:
                    break;
            }
        }
    }
}