using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    /** Probablemente en desuso **/

    public class TaskManager : MonoBehaviour
    {
        Dictionary<string, Task> tasks = new Dictionary<string, Task>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

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
    }
}
