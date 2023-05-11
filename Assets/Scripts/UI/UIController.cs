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
        StaticFixed, //won't look for the player on awake
        StaticPivot,
        StaticAlternative,
        StaticAlternativeFixed, //won't look for the player on awake
        FrontPivot
    }

    // TODO: Move to InfoController classes. Does not belong here
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
        public TurbineSceneController sceneController;

        //public TaskManager taskManager;

        GameObject infoModal;
        GameObject areaInfoInstance;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        Data.Info info;

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
        public Data.Info Info { get => info; set => info = value; }
        

        public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }

        GameObject _infoModalInstance;

        private class DirectionController
        {
            Transform camera;
            Transform ui;

            DisplayMode displayMode;

            public Transform UI { get => ui; set => ui = value; }
            public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

            public DirectionController(SceneController sceneController, Transform transform, DisplayMode displayMode)
            {
                this.UI = transform;
                this.DisplayMode = displayMode;

                camera = sceneController.Camera;
            }

            public void SetDirection()
            {
                //Debug.Log("DisplayMode: " + DisplayMode);

                //Debug.Log("Setting direction to " + (UI.position - camera.position));
                UI.rotation = Quaternion.LookRotation(UI.position - camera.position);
            }
        }

        void Awake()
        {
            //GetComponent<Canvas>().enabled = false;
        }

        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            Debug.Log("debug");
            Debug.Log(sceneController);
            Debug.Log(sceneController.TaskChecked);
            sceneController.TaskChecked.AddListener(UpdateObjectInfo);

            //taskManager = GameObject.Find("SceneController").GetComponent<TaskManager>();
            
            infoModal = Resources.Load("UI/Modal/InfoModal") as GameObject;
            dc = new DirectionController(sceneController, transform, DisplayMode);
            
            if (displayMode != DisplayMode.StaticFixed && displayMode != DisplayMode.StaticAlternativeFixed)
            {
                dc.SetDirection();
            }

            SetContent();
            GetComponent<Canvas>().enabled = false;
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
                case DisplayMode.StaticPivot:
                    dc.SetDirection();
                    break;
                case DisplayMode.Static:
                    break;
                case DisplayMode.StaticFixed:
                    break;
                case DisplayMode.FrontPivot:
                    break;
                case DisplayMode.StaticAlternative:
                    break;
                case DisplayMode.StaticAlternativeFixed:
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
            Debug.Log("UpdateObjectInfo");
            _infoModalInstance.GetComponent<ModalController>().UpdateTasks();
            /*if (_infoModalInstance != null) Destroy(_infoModalInstance);
            ShowObjectInfo();*/
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

        public void Enable() => GetComponent<Canvas>().enabled = true;

        public void Disable() => GetComponent<Canvas>().enabled = false;

        public void Dispose()
        {
            if (AreaInfoInstance != null) Destroy(AreaInfoInstance);
            Destroy(gameObject);
        }
    }
}