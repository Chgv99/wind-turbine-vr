using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.Object;
using WindTurbineVR.UI;

namespace WindTurbineVR.Guide
{
    [RequireComponent(typeof(TaskManager))]
    public class GuideInfoController : InfoController //?
    {
        SceneController sceneController;

        /** TODO:
         *  Call a method in guideController
         *  that shows next guide line */
        GuideController guideController;

        Vector3 guideOrdinal;

        public Vector2 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }

        protected TaskManager taskManager;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            guideController = transform.parent.GetComponent<GuideController>();

            taskManager = GetComponent<TaskManager>();
            taskManager.TaskChecked.AddListener(UpdateTasks);
            taskManager.ListCompleted.AddListener(CompleteList);
            //taskList = GetComponent<TaskManager>().Tasks;

            /**TODO:
             * GUIDES should only show up when
             * previous one is completed and closed.*/
            //ordinal = transform.parent.GetSiblingIndex() + 1;
            CreateUI();
        }

        /*bool TasksCompleted()
        {
            GuideModalController guideModal = _uiInstance.GetComponent<UIController>().ModalInstance.GetComponent<GuideModalController>();
            return guideModal.TasksCompleted();
        }*/

        void Show()
        {
            if (UIInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            UIInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        protected override void CreateUI(float height)
        {
            if (UIInstance != null) return;

            base.CreateUI(height);

            //if (taskList.Count == 0) Debug.Log("task list is empty");
            Debug.Log("UIInstance: " + UIInstance);
            Debug.Log(GuideOrdinal.x + ", " + GuideOrdinal.y);
            Debug.Log(Info.Title + ", " + Info.Description);
            Debug.Log("taskmanager: " + taskManager);
            UIInstance.GetComponent<GuideInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
            UIInstance.GetComponent<GuideInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
            //UiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
            //UiInstance.GetComponent<UIController>().taskControllerList = taskList;
        }

        public void UpdateOrdinal(Vector2 ordinal) => UIInstance.GetComponent<GuideInfoView>().UpdateOrdinal(ordinal);

        public void UpdateTasks() => UIInstance.GetComponent<GuideInfoView>().UpdateTasks(taskManager.GetTasks());

        private void CompleteList()
        {
            UIInstance.GetComponent<GuideInfoView>().CompleteList();
            guideController.NextGuide();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}