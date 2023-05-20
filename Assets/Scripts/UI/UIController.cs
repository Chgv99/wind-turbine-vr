using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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

    // TODO: Remove. This will be implemented as subclasses.
    public enum ContentType
    {
        None = 0,
        SimpleSign,
        ObjectInfo,
        Menu,
        Guide
    }

    public abstract class UIController : MonoBehaviour
    {
        public TurbineSceneController sceneController;

        //[SerializeField] protected GameObject modalPrefab;
        protected Transform modal;

        //public TaskManager taskManager;

        //GameObject infoModal;
        //GameObject guideModal;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        Vector2 guideOrdinal = new Vector2();

        protected Color color = Color.white;

        Data.Info info;

//        public List<TaskController> taskControllerList;

        /** TODO
         * Perpetuar cambios en la lista de tasks:
         * Ahora se leen desde TaskManager en SceneController
         * Hay clases que intentan recogerlo de UIController (esta clase).
         */

        bool initializated = false;

        bool immortal = false;

        UnityEvent completed;

        //bool guide = false;

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get => displayTrigger; set => displayTrigger = value; }
        public Data.Info Info { get => info; set => info = value; }
        //public bool Guide { get => guide; set => guide = value; }

        //public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }
        public Vector2 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }
        public UnityEvent Completed { get => completed; set => completed = value; }
        //public GameObject ModalInstance { get => _modalInstance; set => _modalInstance = value; }

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
            Completed = new UnityEvent();
        }

        protected void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            modal = transform.Find("Modal");
            dc = new DirectionController(sceneController, transform, DisplayMode);

            GetComponent<Canvas>().worldCamera = sceneController.Camera.GetComponent<Camera>();
            ///////////sceneController.TaskChecked.AddListener(UpdateObjectTasks);            
            if (displayMode != DisplayMode.StaticFixed && displayMode != DisplayMode.StaticAlternativeFixed)
            {
                dc.SetDirection();
            }

            //GetComponent<Canvas>().enabled = false;
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

        // Old behaviour. Now it is delegated to child classes.
        /*public void SetContent()
        {
            if (contentType == ContentType.None) return;

            initializated = true;

            switch (contentType)
            {
                case ContentType.ObjectInfo:
                    ///////////ShowObjectInfo();
                    break;
                case ContentType.Guide:
                    ///////////ShowGuide();
                    break;
            }
        }*/

        protected abstract void Show();

        // Old behaviour
        /*protected virtual void Show()
        {

            modalController = Instantiate(modalPrefab, transform).GetComponent<ModalController>();
            if (DisplayTrigger != DisplayTrigger.Hover) modalController.InstantiateCloseButton();

            modalController.SetContent(Info.Title, Info.Description); //sacar un nivel?
        }*/

        /*
        private void ShowGuide()
        {
            ModalInstance = Instantiate(guideModal, transform);
            GuideModalController controller = ModalInstance.GetComponent<GuideModalController>();

            if (DisplayTrigger != DisplayTrigger.Hover) controller.InstantiateCloseButton(); // should only appear when tasks are done

            Debug.Log("Info:");
            Debug.Log(controller);
            Debug.Log(GuideOrdinal);
            Debug.Log(Info.Title);
            Debug.Log(Info.Description);
            Debug.Log(taskControllerList);

            if (taskControllerList != null) controller.SetContent(GuideOrdinal, Info.Title, Info.Description, taskControllerList);
            else Error.LogExceptionNoBreak("Guide panel with no tasks");

            controller.SetContent(Info.Title, Info.Description); //sacar un nivel?
        }

        private void ShowObjectInfo()
        {
            ModalInstance = Instantiate(infoModal, transform);
            InfoModalController controller = ModalInstance.GetComponent<InfoModalController>();
            if (DisplayTrigger != DisplayTrigger.Hover) controller.InstantiateCloseButton();
            
            controller.SetContent(Info.Title, Info.Description); //sacar un nivel?
        }*/

        public void Enable() => GetComponent<Canvas>().enabled = true;

        public void Disable() => GetComponent<Canvas>().enabled = false;

        public void Dispose()
        {
            ///////////if (AreaInfoInstance != null) Destroy(AreaInfoInstance);
            Destroy(gameObject);
        }
    }
}