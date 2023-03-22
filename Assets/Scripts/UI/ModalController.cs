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

        Info info;

        internal Info Info { get => info; set => info = value; }

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
            buttonInstance.GetComponent<Button>().onClick.AddListener(uiCon.Dispose);
        }

        private void OnDestroy()
        {
            if (buttonInstance != null) 
                buttonInstance.GetComponent<Button>().onClick.RemoveListener(uiCon.Dispose);
        }

        public void SetContent(string title, string description, List<TaskController> tasks)
        {
            SetContent(title, description);

            GameObject taskDisplay = Resources.Load("UI/Modal/Task") as GameObject;
            //for (int i = 0; i < tasks.Count; i++)
            foreach (TaskController tc in tasks)
            {
                if (tc == null) continue;

                if (tc.Task == null) Error.LogException("TaskController contains a null Task");

                Task task = tc.Task;
                Debug.Log("task: " + task.Description);
                GameObject taskInstance = Instantiate(taskDisplay, transform.Find("Tasks"));
                Transform toggle = taskInstance.transform.Find("Toggle");
                toggle.GetComponent<Toggle>().isOn = task.Completed;
                toggle.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = task.Description;
            }
        }

        public void SetContent(string title, string description)
        {
            transform.Find("HeaderText").GetComponent<TextMeshProUGUI>().text = title != "" ? title : gameObject.name + " title";
            transform.Find("ModalText").GetComponent<TextMeshProUGUI>().text = description != "" ? description : gameObject.name + " description";
        }  
    }
}
