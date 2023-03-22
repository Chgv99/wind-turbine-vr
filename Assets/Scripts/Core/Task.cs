using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindTurbineVR.Core
{
    public class Task
    {
        #region CONSTS
        string _DEFAULT_DESC = "<DEFAULT TASK>";
        #endregion
        #region Vars
        private bool completed;
        private string description;
        #endregion
        public bool Completed { get => completed; }
        public string Description { get => description; }

        public Task() { this.description = _DEFAULT_DESC; this.completed = false; }

        public Task(string description) { this.description = description; this.completed = false; }

        public void Check() => completed = true;
    }
}
