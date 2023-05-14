using Codice.Client.BaseCommands.Merge;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class GuideModalController : ModalController
    {
        [SerializeField] List<TaskController> tcs;
        [SerializeField] List<Toggle> toggles;

        // Start is called before the first frame update
        void Awake()
        {
            base.Awake();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool TasksCompleted()
        {
            for (int i = 0; i < tcs.Count; i++)
            {
                if (tcs[i].Task.Completed) toggles[i].isOn = true;
                else return false;
            }
            return true;
        }

        public bool UpdateTasks()
        {
            Debug.Log("UpdateTasks");
            if (tcs.Count != toggles.Count) Error.LogException("Task list size is different from toggle list size.");

            for (int i = 0; i < tcs.Count; i++)
            {
                if (tcs[i].Task.Completed) toggles[i].isOn = true;
            }

            return TasksCompleted();
        }

        public void SetContent(Vector2 ordinal, string title, string description, List<TaskController> tcs)
        {
            base.SetContent(title, description);
            transform.Find("OrdinalText").GetComponent<TextMeshProUGUI>().text = ordinal.x + "/" + ordinal.y;
            //SetContent(title, description);

            this.tcs = tcs;

            toggles = new List<Toggle>();

            GameObject taskDisplay = Resources.Load("UI/Modal/Task") as GameObject;
            //for (int i = 0; i < tasks.Count; i++)
            Transform taskListTransform = transform.Find("Tasks");
            foreach (TaskController tc in tcs)
            {
                if (tc == null) continue;

                if (tc.Task == null) Error.LogException("TaskController contains a null Task");

                Task task = tc.Task;
                Debug.Log("task: " + task.Description);
                GameObject taskInstance = Instantiate(taskDisplay, taskListTransform);

                Transform toggleT = taskInstance.transform.Find("Toggle");
                toggleT.Find("Label").GetComponent<TextMeshProUGUI>().text = task.Description;

                Toggle toggle = toggleT.GetComponent<Toggle>();
                toggle.isOn = task.Completed;

                toggles.Add(toggle);
            }
            //taskListTransform.gameObject.SetActive(false);
            StartCoroutine(DisableTaskList(taskListTransform.gameObject));
            StartCoroutine(EnableTaskList(taskListTransform.gameObject));
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