using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WindTurbineVR.Data;
using WindTurbineVR.Core;

namespace WindTurbineVR.Guide
{
    public class TaskController : MonoBehaviour
    {
        TurbineSceneController sceneController;
        //TaskManager taskManager;

        [SerializeField] Task task;

        private UnityEvent taskChecked;

        [SerializeField] string description;

        public Task Task { get => task; }
        public string Description { get => description; }
        
        public UnityEvent TaskChecked { get => taskChecked; set => taskChecked = value; }

        // Start is called before the first frame update
        void Awake()
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
            //TaskChecked = new UnityEvent();//sceneController.TaskChecked;
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
            Debug.Log(TaskChecked == null);
            Debug.Log(TaskChecked);
            // Check task
            task.Check();
            // Then call event for ui to update,
            TaskChecked?.Invoke();

            //Destroy(this);
        }
    }
}
