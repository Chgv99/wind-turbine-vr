using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WindTurbineVR.Core
{
    public class TaskController : MonoBehaviour
    {
        //TaskManager taskManager;

        [SerializeField] Task task;

        [SerializeField] string description;

        public Task Task { get => task; }
        public string Description { get => description; }

        private UnityEvent taskChecked;

        // Start is called before the first frame update
        void Start()
        {
            //taskManager = GameObject.Find("SceneController").GetComponent<TaskManager>();
            //taskManager.AddTask(this); //this component
            if (description == "")
            {
                Error.LogException("Empty task description. This will produce a <DEFAULT TASK>.");
                task = new Task();
            }
            task = new Task(description);
            taskChecked = GameObject.Find("SceneController").GetComponent<TurbineSceneController>().TaskChecked;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // Method listens for the event that will check this task.
        public void ActionListener()
        {
            Debug.Log("action listener");
            Debug.Log("null event? ");
            Debug.Log(taskChecked == null);
            Debug.Log(taskChecked);
            // Check task
            task.Check();
            // Then call event for ui to update,
            taskChecked?.Invoke();

            //Destroy(this);
        }
    }
}
