using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using WindTurbineVR.Data;
using WindTurbineVR.Core;

namespace WindTurbineVR.Guide
{
    /** Probablemente en desuso **/

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<TaskController> tasks;

        private UnityEvent taskChecked;
        private UnityEvent listCompleted;

        public List<TaskController> Tasks { get => tasks; }
        public UnityEvent TaskChecked { get => taskChecked; set => taskChecked = value; }
        public UnityEvent ListCompleted { get => listCompleted; set => listCompleted = value; }

        // Start is called before the first frame update
        void Awake()
        {
            taskChecked = new UnityEvent();
            listCompleted = new UnityEvent();

            foreach (TaskController task in Tasks)
            {
                task.TaskChecked = TaskChecked;
            }

            taskChecked.AddListener(CheckListCompleted);
        }

        void OnDestroy()
        {
            taskChecked.RemoveListener(CheckListCompleted);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CheckListCompleted()
        {
            bool completed = true;
            //If list is completed raise listCompleted event
            foreach (TaskController taskController in tasks)
            {
                if (taskController.Task.Completed) continue;
                completed = taskController.Task.Completed; //false
                break;
            }

            if (completed) listCompleted?.Invoke();
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

        public void AddTask(TaskController tc) => AddAndSetTask(tc);

        private void AddAndSetTask(TaskController tc)
        {
            Tasks.Add(tc);
            tc.TaskChecked = TaskChecked;
        }

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
