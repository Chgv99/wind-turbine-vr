using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace WindTurbineVR.Core
{
    public class SceneController : MonoBehaviour
    {
        public Transform xrOrigin;
        public Transform camera;

        private UnityEvent taskRemoved;

        public UnityEvent TaskRemoved { get => taskRemoved; }

        // Start is called before the first frame update
        void Start()
        {
            Exception nullExc = new Exception("Variable not set to an instance of an object.");
            
            if (xrOrigin == null) Debug.LogException(nullExc);
            if (camera == null)
            {
                camera = xrOrigin.Find("Camera Offset/Main Camera");
            }

            taskRemoved = new UnityEvent();
        }

        public void PlayScene(int i)
        {
            SceneManager.LoadScene(i);
        }

        public void PlayScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void EventLog(string str)
        {
            Debug.Log(str);
            Debug.Break();
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
