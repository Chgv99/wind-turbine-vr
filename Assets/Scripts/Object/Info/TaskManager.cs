using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Data;

namespace WindTurbineVR.Object.Info
{
    /** Probablemente en desuso **/

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<TaskController> tasks;

        public List<TaskController> Tasks { get => tasks; }

        //Dictionary<string, Task> tasks = new Dictionary<string, Task

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public List<Task> GetTasks()
        {
            List<Task> taskData = new List<Task>();
            foreach (TaskController taskController in tasks)
            {
                taskData.Add(taskController.Task);
            }
            return taskData;
        }

        public void AddTask(TaskController tc) => Tasks.Add(tc);

        public void RemoveTask(TaskController tc) => Tasks.Remove(tc);

        /* Old behaviour
        public void SetTaskTo(bool completed)
        {

        }

        public Task GetTask(string desc)
        {
            bool success = tasks.TryGetValue(desc, out Task task);

            if (success) return task;
            else return null;
        }

        public void AddTask(Task task)
        {
            if (tasks.ContainsKey(task.Description)) return;

            tasks.Add(task.Description, task);
        }
        */
    }
}
