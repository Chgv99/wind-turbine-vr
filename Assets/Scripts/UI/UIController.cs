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
        public SceneController sceneController;

        //public TaskManager taskManager;

        GameObject infoModal;
        GameObject areaInfoInstance;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        Info info;

        public List<TaskController> taskControllerList;

        /** TODO
         * Perpetuar cambios en la lista de tasks:
         * Ahora se leen desde TaskManager en SceneController
         * Hay clases que intentan recogerlo de UIController (esta clase).
         */

        bool initializated = false;

        bool immortal = false;

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get => displayTrigger; set => displayTrigger = value; }
        public Info Info { get => info; set => info = value; }
        

        public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }

        GameObject _infoModalInstance;

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
                Debug.Log("DisplayMode: " + DisplayMode);

                Debug.Log("Setting direction to " + (UI.position - camera.position));
                UI.rotation = Quaternion.LookRotation(UI.position - camera.position);
            }
        }

        void Awake()
        {
            GetComponent<Canvas>().enabled = false;
        }

        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            sceneController.TaskRemoved.AddListener(UpdateObjectInfo);

            //taskManager = GameObject.Find("SceneController").GetComponent<TaskManager>();
            
            infoModal = Resources.Load("UI/Modal/InfoModal") as GameObject;
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

            switch (DisplayMode)
            {
                case DisplayMode.Static:
                    break;
                case DisplayMode.StaticPivot:
                    dc.SetDirection();
                    break;
                case DisplayMode.FrontPivot:
                    break;
            }
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

        // taskRemoved event listener
        public void UpdateObjectInfo()
        {
            if (_infoModalInstance != null) Destroy(_infoModalInstance);
            ShowObjectInfo();
        }

        private void ShowObjectInfo()
        {
            _infoModalInstance = Instantiate(infoModal, transform);
            if (DisplayTrigger != DisplayTrigger.Hover) _infoModalInstance.GetComponent<ModalController>().InstantiateCloseButton();
            /*if (DisplayTrigger != DisplayTrigger.TriggerStay && AreaInfoInstance != null)
            {
                infoModalInstance.GetComponent<ModalController>().InstantiateCloseButton(AreaInfoInstance);
            }*/

            //Debug.Log(taskControllerList.Count + "lista");
            if (taskControllerList != null) _infoModalInstance.GetComponent<ModalController>().SetContent(Info.Title, Info.Description, taskControllerList);
            else _infoModalInstance.GetComponent<ModalController>().SetContent(Info.Title, Info.Description);
        }

        public void Dispose()
        {
            if (AreaInfoInstance != null) Destroy(AreaInfoInstance);
            Destroy(gameObject);
        }
    }
}