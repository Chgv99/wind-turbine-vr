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
        Transform camera;

        public Transform Camera { get => camera; set => camera = value; }

        // Start is called before the first frame update
        public virtual void Start()
        {
            Exception nullExc = new Exception("Variable not set to an instance of an object.");
            
            if (xrOrigin == null) Debug.LogException(nullExc);
            if (Camera == null)
            {
                Camera = xrOrigin.Find("Camera Offset/Main Camera");
            }
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
            //Debug.Break();
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
