using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WindTurbineVR.Core;
using WindTurbineVR.Data;
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
        StaticAlternativePivot,
        FrontPivot
    }

    // TODO: Move to InfoController classes. Does not belong here
    public enum DisplayTrigger
    {
        None,
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

    public abstract class InfoView : MonoBehaviour
    {
        public SceneController sceneController;

        protected Transform modal;

        [SerializeField] DirectionController dc;

        ContentType contentType = ContentType.None;
        DisplayMode displayMode = DisplayMode.Static;
        DisplayTrigger displayTrigger = DisplayTrigger.Hover;

        Vector2 guideOrdinal = new Vector2();
        
        [Space]
        [SerializeField] private Color normalColor;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Color pressedColor;

        //[Space]
        //[SerializeField] protected List<GameObject> buttons;

        protected GameObject nextPrev;
        protected GameObject prevButton;
        protected GameObject nextButton;

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

        public ContentType ContentType { get => contentType; set => contentType = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }
        public DisplayTrigger DisplayTrigger { get => displayTrigger; set => displayTrigger = value; }
        public Data.Info Info { get => info; set => info = value; }

        protected int page = 0;

        //public GameObject AreaInfoInstance { get => areaInfoInstance; set => areaInfoInstance = value; }
        public Vector2 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }
        public UnityEvent Completed { get => completed; set => completed = value; }
        public Color NormalColor { get => normalColor; set => normalColor = value; }
        public Color HighlightedColor { get => highlightedColor; set => highlightedColor = value; }
        public Color PressedColor { get => pressedColor; set => pressedColor = value; }

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
                Debug.Log("DirectionController constructor");
                camera = sceneController.Camera;
            }

            public void SetDirection()
            {
                //Debug.Log("DisplayMode: " + DisplayMode);

                //Debug.Log("Setting direction to " + (UI.position - camera.position));
                UI.rotation = Quaternion.LookRotation(UI.position - camera.position);
            }
        }

        protected virtual void Awake()
        {
            //GetComponent<Canvas>().enabled = false;
            //SceneController sc = GameObject.Find("SceneController").GetComponent<SceneController>();
            //TurbineSceneController tsc = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();

            modal = transform.Find("Modal");
            dc = new DirectionController(sceneController, transform, DisplayMode);

            Completed = new UnityEvent();

            /**Debug.Log("sceneController: " + sceneController);
            Debug.Log("camera obj: " + sceneController.Camera);
            Debug.Log("canvas: " + GetComponent<Canvas>());*/
            GetComponent<Canvas>().worldCamera = sceneController.Camera.GetComponent<Camera>();
            ///////////sceneController.TaskChecked.AddListener(UpdateObjectTasks);            
            if (displayMode != DisplayMode.StaticFixed && displayMode != DisplayMode.StaticAlternativeFixed)
            {
                dc.SetDirection();
            }

            try
            {
                nextPrev = transform.Find("NextPrevButtons").gameObject;
                prevButton = nextPrev.transform.Find("PreviousButton").gameObject;
                nextButton = nextPrev.transform.Find("NextButton").gameObject;

                prevButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
                nextButton.GetComponent<Button>().onClick.AddListener(NextPage);
            } catch (Exception ex) { Error.LogExceptionNoBreak(ex.Message); }

            //buttons = new List<GameObject>();
            //buttons.Add(transform.Find("NextPrevButtons").Find("PreviousButton").gameObject);
            //buttons.Add(transform.Find("NextPrevButtons").Find("NextButton").gameObject);

            //GetComponent<Canvas>().enabled = false;
        }

        void OnDestroy()
        {
            prevButton.GetComponent<Button>().onClick.RemoveListener(PreviousPage);
            nextButton.GetComponent<Button>().onClick.RemoveListener(NextPage);
        }

        protected void Start()
        {
            
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
                case DisplayMode.StaticAlternativePivot:
                    dc.SetDirection();
                    break;
            }
        }

        public virtual void UpdateColor(Color color)
        {
            //Debug.Log("prev: " + prevButton);
            SetButtonColor(prevButton.GetComponent<Button>(), color);
            SetButtonColor(nextButton.GetComponent<Button>(), color);
            //nextPrev.SetActive(false);
        }

        protected void ShowNextPrev() => nextPrev.SetActive(true);

        protected void HideNextPrev() => nextPrev.SetActive(false);

        protected void SetButtonColor(Button button, Color color)
        {
            NormalColor = color;
            HighlightedColor = new Color(color.r + 0.1f, color.g + 0.1f, color.b + 0.1f);
            PressedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f);

            var colors = button.colors;
            colors.normalColor = NormalColor;
            colors.highlightedColor = HighlightedColor;
            colors.pressedColor = PressedColor;
            button.colors = colors;
        }

        protected abstract void Show();

        public virtual void UpdateContent(Info info)
        {
            Info = info;
            if (Info.Description.Length > 1) nextPrev.SetActive(true);
        }

        public bool IsActive()
        {
            return GetComponent<Canvas>().isActiveAndEnabled;
        }

        public void Enable()
        {
            if (GetComponent<Canvas>() != null) GetComponent<Canvas>().enabled = true;
            //if (GetComponent<ContentSizeFitter>() != null) GetComponent<ContentSizeFitter>().enabled = true;
            RefreshFitter();
        }

        public void Disable()
        {
            if (GetComponent<Canvas>() != null) GetComponent<Canvas>().enabled = false;
            //if (GetComponent<ContentSizeFitter>() != null) GetComponent<ContentSizeFitter>().enabled = false;
        }

        public void Dispose()
        {
            ///////////if (AreaInfoInstance != null) Destroy(AreaInfoInstance);
            Destroy(gameObject);
        }

        public void RefreshFitter()
        {
            if (GetComponent<ContentSizeFitter>() != null)
            {
                GetComponent<ContentSizeFitter>().enabled = false;
                StartCoroutine(EnableContentSizeFitter());
            }
            
        }

        IEnumerator EnableContentSizeFitter()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            GetComponent<ContentSizeFitter>().enabled = true;
        }

        protected virtual void NextPage()
        {
            RefreshFitter();
        }

        protected int GoToNextPage()
        {
            Debug.Log("Go To Next Page: " + page + ", " + Info.Description.Length);
            prevButton.GetComponent<Button>().interactable = true;
            if (page + 1 == Info.Description.Length - 1)
            {
                nextButton.GetComponent<Button>().interactable = false;
                try
                {
                    EndPagination();
                } catch (Exception ex) { Error.LogExceptionNoBreak(ex.Message); }
            }

            if ((page + 1) < Info.Description.Length)
            {
                return ++page;
            }
            else return -1;
        }

        protected abstract void PreviousPage();

        protected int GoToPreviousPage()
        {
            Debug.Log("Go To Previous Page: " + page + ", " + Info.Description.Length);
            nextButton.GetComponent<Button>().interactable = true;
            if ((page - 1) == 0) prevButton.GetComponent<Button>().interactable = false;

            if ((page - 1) >= 0)
            {
                return --page;
            }
            else return -1;
        }

        protected abstract void EndPagination();
    }
}