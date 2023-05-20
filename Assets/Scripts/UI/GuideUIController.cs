using PlasticGui.WorkspaceWindow.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Core;
using WindTurbineVR.Data;

namespace WindTurbineVR.UI
{

    /** TODO:
     *  CHANGE CLASS NAME TO GUIDEVIEW
     *  (AND FOLLOW CONVENTION IN OTHER UICONTROLLERS) */

    public class GuideUIController : UIController
    {
        GuideModalController guideModalController;

        GameObject taskPrefab;

        List<Toggle> toggles;
        Transform taskListTransform;

        [SerializeField] TextMeshProUGUI ordinalText;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI modalText; //change name to description ??
        //tasks


        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            taskPrefab = Resources.Load("UI/Modal/Task") as GameObject;
            if (taskPrefab == null) Debug.Log("taskprefab nul");
            //guideModalController = modal.GetComponent<GuideModalController>();
            taskListTransform = modal.Find("Tasks");

            ordinalText = modal.Find("OrdinalText").GetComponent<TextMeshProUGUI>();
            headerText = modal.Find("HeaderText").GetComponent<TextMeshProUGUI>();
            modalText = modal.Find("ModalText").GetComponent<TextMeshProUGUI>();
            //tasks

            //sceneController.TaskChecked.AddListener(UpdateObjectTasks);

            color = new Color(255,187,0);

            Show();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Show()
        {
            if (DisplayTrigger != DisplayTrigger.Hover) guideModalController.InstantiateCloseButton();
            //UpdateContent();
        }
        //public List<TaskController> taskControllerList;

        // taskRemoved event listener
        /*
        public void UpdateObjectTasks()
        {
            Debug.Log("UpdateObjectInfo");
            //GuideModalController controller = ModalInstance.GetComponent<GuideModalController>();
            if (guideModalController.UpdateTasks()) TasksCompleted(guideModalController);
        }*/

        public void UpdateOrdinal(Vector2 ordinal) => ordinalText.text = ordinal.x + "/" + ordinal.y;

        public void UpdateContent(Vector2 ordinal, Info info, List<Task> tasks)
        {
            if (DisplayTrigger != DisplayTrigger.Hover) guideModalController.InstantiateCloseButton();

            //if (taskControllerList != null) SetContent(GuideOrdinal, Info.Title, Info.Description, taskControllerList);
            //else Error.LogExceptionNoBreak("Guide panel with no tasks");
            //}

            //public void SetContent(Vector2 ordinal, string title, string description, List<Task> tasks)
            //{
            Debug.Log("modalText object: " + modalText);
            //headerText.text = info.Title != "" ? info.Title : gameObject.name + " title";
            modalText.text = info.Description != "" ? info.Description : gameObject.name + " description";

            UpdateOrdinal(ordinal);
            //SetContent(title, description);

            //this.tcs = tcs;

            toggles = new List<Toggle>();

            foreach (Task task in tasks)
            {
                //if (task == null) continue;

                //if (task.Task == null) Error.LogException("TaskController contains a null Task");

                //Task task = task.Task;
                //Debug.Log("task: " + task.Description);
                GameObject taskInstance = Instantiate(taskPrefab, taskListTransform);

                Transform toggleT = taskInstance.transform.Find("Toggle");
                toggleT.Find("Label").GetComponent<TextMeshProUGUI>().text = task.Description;

                Toggle toggle = toggleT.GetComponent<Toggle>();
                toggle.isOn = task.Completed;

                toggles.Add(toggle);
            }
            
            //StartCoroutine(DisableTaskList(taskListTransform.gameObject));
            //StartCoroutine(EnableTaskList(taskListTransform.gameObject));
        }

        public void TasksCompleted(GuideModalController controller)
        {
            controller.InstantiateCloseButton();
            Completed?.Invoke();
        }

        IEnumerator DisableTaskList(GameObject taskList)
        {
            yield return new WaitForSecondsRealtime(1f);
            taskList.gameObject.SetActive(false);
        }

        IEnumerator EnableTaskList(GameObject taskList)
        {
            yield return new WaitForSecondsRealtime(2f);
            taskList.gameObject.SetActive(true);
        }
    }
}