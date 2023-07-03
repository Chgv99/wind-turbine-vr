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
        /** TODO:
         *  Call a method in guideController
         *  that shows next guide line */
        GuideController guideController;

        Vector3 guideOrdinal;

        public Vector2 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }

        protected TaskManager taskManager;

        bool noTasks = false;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            guideController = transform.parent.GetComponent<GuideController>();

            taskManager = GetComponent<TaskManager>();
            if (taskManager == null) noTasks = true;

            if (!noTasks)
            {
                taskManager.TaskChecked.AddListener(UpdateTasks);
                taskManager.ListCompleted.AddListener(CompleteList);
            }
            
            //taskList = GetComponent<TaskManager>().Tasks;

            /**TODO:
             * GUIDES should only show up when
             * previous one is completed and closed.*/
            //ordinal = transform.parent.GetSiblingIndex() + 1;
            CreateUITrack();
        }

        /*bool TasksCompleted()
        {
            GuideModalController guideModal = _uiInstance.GetComponent<UIController>().ModalInstance.GetComponent<GuideModalController>();
            return guideModal.TasksCompleted();
        }*/

        /*void Show()
        {
            if (UIInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            UIInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }*/

        protected override void CreateUI(float height)
        {
            if (UIInstance != null) return;

            base.CreateUI(height);

            //if (taskList.Count == 0) Debug.Log("task list is empty");
            UIInstance.GetComponent<GuideInfoView>().ContinueButtonPressed.AddListener(CompleteList);
            UIInstance.GetComponent<GuideInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
            //UIInstance.GetComponent<GuideInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
            //UIInstance.GetComponent<GuideInfoView>().Color = color;
            UIInstance.GetComponent<GuideInfoView>().UpdateColor(color);
            //UIInstance.GetComponent<GuideInfoView>().DisableCloseButton();
            //UiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
            //UiInstance.GetComponent<UIController>().taskControllerList = taskList;
            UIInstance.GetComponent<GuideInfoView>().SetContinueButtonState(noTasks);
        }

        public void UpdateOrdinal(Vector2 ordinal) => UIInstance.GetComponent<GuideInfoView>().UpdateOrdinal(ordinal);

        public void UpdateTasks() => UIInstance.GetComponent<GuideInfoView>().UpdateTasks(taskManager.GetTasks());

        private void CompleteList()
        {
            Debug.Log("CompleteList()");
            UIInstance.GetComponent<GuideInfoView>().CompleteList();
            guideController.NextGuide();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            if (UIInstance == null) return;
            UIInstance.GetComponent<GuideInfoView>().ContinueButtonPressed.RemoveListener(CompleteList);
        }
    }

}