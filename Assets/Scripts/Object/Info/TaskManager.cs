using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WindTurbineVR.Data;

namespace WindTurbineVR.Object.Info
{
    /** Probablemente en desuso **/

    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<TaskController> tasks;

        private UnityEvent taskChecked;

        public List<TaskController> Tasks { get => tasks; }
        public UnityEvent TaskChecked { get => taskChecked; set => taskChecked = value; }

        /** TODO: MAKE THIS CLASS INTERFERE
         * IN EVENT COMMUNICATION BETWEEN
         * INFOCONTROLLER AND TASKCONTROLLER
         * IN ORDER TO APPLY A COMPLETED LIST
         * BEHAVIOUR */

        // Start is called before the first frame update
        void Start()
        {
            taskChecked = new UnityEvent();

            foreach (TaskController task in Tasks)
            {
                task.TaskChecked = TaskChecked;
            }
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
