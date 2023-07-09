//using PlasticGui.WorkspaceWindow.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WindTurbineVR.Core;
using WindTurbineVR.Data;

namespace WindTurbineVR.UI
{
    public class GuideInfoView : InfoView
    {
        //GuideModalController guideModalController;
        GameObject taskPrefab;

        List<Toggle> toggles;
        Transform taskListTransform;

        Transform completedText;

        GameObject continueButton;
        GameObject closeButton;

        [Space]
        [SerializeField] TextMeshProUGUI ordinalText;
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI modalText; //change name to description ??
        //tasks

        UnityEvent continueButtonPressed;

        //Info info;

        public UnityEvent ContinueButtonPressed { get => continueButtonPressed; set => continueButtonPressed = value; }

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake(); //unnecessary?
            taskPrefab = Resources.Load("UI/Modal/Task") as GameObject;
            if (taskPrefab == null) Debug.Log("taskprefab nul");
            //guideModalController = modal.GetComponent<GuideModalController>();
            taskListTransform = transform.Find("Tasks");
            completedText = transform.Find("CompletedText");

            continueButton = transform.Find("ContinueButton").gameObject;
            closeButton = transform.Find("CloseButton").gameObject;

            toggles = new List<Toggle>();

            ordinalText = transform.Find("OrdinalText").GetComponent<TextMeshProUGUI>();
            headerText = transform.Find("HeaderText").GetComponent<TextMeshProUGUI>();
            modalText = transform.Find("ModalText").GetComponent<TextMeshProUGUI>();
            //tasks
            continueButtonPressed = new UnityEvent();
            //sceneController.TaskChecked.AddListener(UpdateObjectTasks);

            //Color = new Color(255, 187, 0);

            //Show();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /** TODO: INSTANCIAR BOTÓN O MENSAJE */
        protected override void Show()
        {
            //if (DisplayTrigger != DisplayTrigger.Hover) guideModalController.InstantiateCloseButton();
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

        public void UpdateContent(Vector2 ordinal, Info info, List<Task> tasks)
        {
            //if (DisplayTrigger != DisplayTrigger.Hover) guideModalController.InstantiateCloseButton();

            //if (taskControllerList != null) SetContent(GuideOrdinal, Info.Title, Info.Description, taskControllerList);
            //else Error.LogExceptionNoBreak("Guide panel with no tasks");
            //}

            //public void SetContent(Vector2 ordinal, string title, string description, List<Task> tasks)
            //{
            base.UpdateContent(info);

            //Debug.Log("modalText object: " + modalText);
            headerText.text = info.Title != "" ? info.Title : gameObject.name + " title";
            //Debug.Log("description length: " + info.Description.Length);
            modalText.text = info.Description.Length > 0 ? info.Description[0] : gameObject.name + " description";

            UpdateOrdinal(ordinal);
            //SetContent(title, description);

            //this.tcs = tcs;

            Canvas.ForceUpdateCanvases();

            taskListTransform.gameObject.gameObject.SetActive(false);

            //for (int i = 0; i < toggles.Count; i++)
            //foreach (Toggle toggle in toggles)
            while (toggles.Count > 0)
            {
                //Debug.Log("Destroying toggle i = " + i + ": " + toggles[i]);
                Destroy(toggles[0].gameObject);
                toggles.Remove(toggles[0]);
            }

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

            taskListTransform.gameObject.gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();
            //StartCoroutine(DisableTaskList(taskListTransform.gameObject));
            //StartCoroutine(EnableTaskList(taskListTransform.gameObject));
        }

        public void UpdateOrdinal(Vector2 ordinal) 
        { 
            ordinalText.text = ordinal.x + "/" + ordinal.y;
            headerText.text = Info.Title + " " + ordinalText.text;
        }

        public void UpdateTasks(List<Task> tasks)
        {
            Debug.Log("InfoView.UpdateTasks");
            for (int i = 0; i < toggles.Count; i++)
            {
                Debug.Log("checking " + i);
                toggles[i].isOn = tasks[i].Completed;
            }
            /*
            toggles task.Completed
            for (int i = 0; i < tcs.Count; i++)
            {
                if (tcs[i].Task.Completed) toggles[i].isOn = true;
            }*/
        }

        public override void UpdateColor(Color color)
        {
            base.UpdateColor(color);
            Debug.Log("UpdateColor" + gameObject.name);
            Debug.Log("Color: " + color.ToString());
            SetButtonColor(closeButton.GetComponent<Button>(), color);
            SetButtonColor(continueButton.GetComponent<Button>(), color);
            SetToggleColor(color);
        }

        void SetToggleColor(Color color)
        {
            foreach (Toggle toggle in toggles)
            {
                toggle.transform.Find("Background").GetComponent<Image>().color = color;
            }
        }

        /*void SetButtonColor(Button button, Color color)
        {
            NormalColor = color;
            HighlightedColor = new Color(color.r + 0.1f, color.g + 0.1f, color.b + 0.1f);
            PressedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f);

            var colors = button.colors;
            colors.normalColor = NormalColor;
            colors.highlightedColor = HighlightedColor;
            colors.pressedColor = PressedColor;
            button.colors = colors;
        }*/

        //public void DisableCloseButton() => closeButton.SetActive(false);

        public void CompleteList()
        {
            //Instantiate congratulations text or something
            Debug.Log("List is completed. Congrats.");
            taskListTransform.gameObject.SetActive(false);
            modalText.gameObject.SetActive(false);
            completedText.gameObject.SetActive(true);
            if (Info.CongratsMessage != "") 
                completedText.GetComponent<TextMeshProUGUI>().text = Info.CongratsMessage;
            ShowCloseButton();
            HideContinueButton();
            HideNextPrev();
        }

        public void SetContinueButtonState(bool noTasks)
        {
            if (!noTasks || Info.Description.Length > 1) return;

            ShowContinueButton();
        }

        public void SetBody(string text) { this.modalText.GetComponent<TextMeshProUGUI>().text = text; }

        public void ShowContinueButton() => continueButton.SetActive(true);

        public void HideContinueButton() => continueButton.SetActive(false);

        public void Continue() => continueButtonPressed?.Invoke();

        void ShowCloseButton() => closeButton.SetActive(true);

        /*public void TasksCompleted(GuideModalController controller)
        {
            controller.InstantiateCloseButton();
            Completed?.Invoke();
        }*/

        IEnumerator DisableTaskList(GameObject taskList)
        {
            yield return new WaitForSecondsRealtime(.1f);
            taskList.gameObject.SetActive(false);
        }

        IEnumerator EnableTaskList(GameObject taskList)
        {
            yield return new WaitForSecondsRealtime(2f);
            taskList.gameObject.SetActive(true);
        }

        protected override void EndPagination()
        {
            ShowContinueButton();
        }

        protected override void ResetPagination()
        {
            if (Info != null) SetBody(Info.Description[ResetPage()]);
            PreviousPage();
        }

        protected override void NextPage()
        {
            SetBody(Info.Description[GoToNextPage()]);
            base.NextPage();
        }

        protected override void PreviousPage()
        {
            SetBody(Info.Description[GoToPreviousPage()]);
        }

        protected override void SetTeleportedAction()
        {
            
        }
    }
}