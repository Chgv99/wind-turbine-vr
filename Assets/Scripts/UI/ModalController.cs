using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WindTurbineVR.Core;
using TMPro;

namespace WindTurbineVR.UI
{
    public class ModalController : MonoBehaviour
    {
        [SerializeField] GameObject button;

        [SerializeField] GameObject buttonInstance;

        UIController uiCon;

        List<TaskController> tcs;
        List<Toggle> toggles;

        Data.Info info;

        internal Data.Info Info { get => info; set => info = value; }

        // Start is called before the first frame update
        void Awake()
        {
            uiCon = transform.parent.GetComponent<UIController>();
            button = Resources.Load("Button") as GameObject;

            /*closeButton = transform.Find("CloseButton").GetComponent<Button>();

            //Modal needs to be disposed somehow unless undispossable
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(uiCon.Dispose);
            }*/
        }

        public void InstantiateCloseButton()
        {
            buttonInstance = Instantiate(button, transform);
            buttonInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Close";
            //buttonInstance.GetComponent<Button>().onClick.AddListener(uiCon.Dispose);
            buttonInstance.GetComponent<Button>().onClick.AddListener(uiCon.Disable);
        }

        private void OnDestroy()
        {
            if (buttonInstance != null)
                //buttonInstance.GetComponent<Button>().onClick.RemoveListener(uiCon.Dispose);
                buttonInstance.GetComponent<Button>().onClick.RemoveListener(uiCon.Disable);
        }

        public void UpdateTasks()
        {
            Debug.Log("UpdateTasks");
            if (tcs.Count != toggles.Count) Error.LogException("Task list size is different from toggle list size.");

            for (int i = 0; i < tcs.Count; i++)
            {
                if (tcs[i].Task.Completed) toggles[i].isOn = true;
            }
        }

        public void SetContent(string title, string description, List<TaskController> tcs)
        {
            SetContent(title, description);

            this.tcs = tcs;

            toggles = new List<Toggle>();

            GameObject taskDisplay = Resources.Load("UI/Modal/Task") as GameObject;
            //for (int i = 0; i < tasks.Count; i++)
            foreach (TaskController tc in tcs)
            {
                if (tc == null) continue;

                if (tc.Task == null) Error.LogException("TaskController contains a null Task");

                Task task = tc.Task;
                Debug.Log("task: " + task.Description);
                GameObject taskInstance = Instantiate(taskDisplay, transform.Find("Tasks"));
                
                Transform toggleT = taskInstance.transform.Find("Toggle");
                toggleT.Find("Label").GetComponent<TextMeshProUGUI>().text = task.Description;

                Toggle toggle = toggleT.GetComponent<Toggle>();
                toggle.isOn = task.Completed;
                
                toggles.Add(toggle);
            }
        }

        public void SetContent(string title, string description)
        {
            transform.Find("HeaderText").GetComponent<TextMeshProUGUI>().text = title != "" ? title : gameObject.name + " title";
            transform.Find("ModalText").GetComponent<TextMeshProUGUI>().text = description != "" ? description : gameObject.name + " description";
        }  
    }
}
