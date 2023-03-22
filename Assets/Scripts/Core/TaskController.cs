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

        private UnityEvent taskRemoved;

        // Start is called before the first frame update
        void Start()
        {
            //taskManager = GameObject.Find("SceneController").GetComponent<TaskManager>();
            //taskManager.AddTask(this); //this component
            if (description == "")
            {
                Debug.LogError("Empty task description. This will produce a <DEFAULT TASK>.");
                Debug.Break();
                task = new Task();
            }
            task = new Task(description);
            taskRemoved = GameObject.Find("SceneController").GetComponent<SceneController>().TaskRemoved;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // Method listens for the event that will check this task.
        public void ActionListener()
        {
            Debug.Log("action listener");
            // Call event for ui to update,
            taskRemoved?.Invoke();
            // then destroy this component.
            Destroy(this);
        }
    }
}
