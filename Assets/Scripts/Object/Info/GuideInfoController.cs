using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Info
{
    [RequireComponent(typeof(TaskManager))]
    public class GuideInfoController : InfoController
    {
        SceneController sceneController;

        Vector3 guideOrdinal;

        public Vector3 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }

        [Space]
        protected List<TaskController> taskList;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            taskList = GetComponent<TaskManager>().Tasks;

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
            if (UiInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            UiInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        protected override void CreateUI(float height)
        {
            if (UiInstance != null) return;

            base.CreateUI(height);

            if (taskList.Count == 0) Debug.Log("task list is empty");

            UiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
            UiInstance.GetComponent<UIController>().taskControllerList = taskList;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}