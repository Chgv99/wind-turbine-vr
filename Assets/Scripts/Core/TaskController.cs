using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WindTurbineVR.Core
{
    public class TaskController : MonoBehaviour
    {
        TurbineSceneController sceneController;
        //TaskManager taskManager;

        [SerializeField] Task task;

        [SerializeField] string description;

        public Task Task { get => task; }
        public string Description { get => description; }

        private UnityEvent taskChecked;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            if (sceneController == null) Error.LogException("SceneController not found");

            //taskManager = GameObject.Find("SceneController").GetComponent<TaskManager>();
            //taskManager.AddTask(this); //this component
            if (description == "")
            {
                Error.LogException("Empty task description. This will produce a <DEFAULT TASK>.");
                task = new Task();
            }
            task = new Task(description);
            taskChecked = sceneController.TaskChecked;
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
