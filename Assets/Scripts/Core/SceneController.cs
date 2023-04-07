using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        protected SceneHelper sceneHelper;

        protected class SceneHelper
        {
            UnityEngine.AsyncOperation asyncOperation = new UnityEngine.AsyncOperation();

            public void LoadAsync()
            {
                Scene activeScene = SceneManager.GetActiveScene();

                if (activeScene.name == "Turbine") asyncOperation = SceneManager.LoadSceneAsync("TurbineAerial");
                if (activeScene.name == "TurbineAerial") asyncOperation = SceneManager.LoadSceneAsync("Turbine");

                asyncOperation.allowSceneActivation = false;
            }

            public void AllowSceneActivation() => asyncOperation.allowSceneActivation = true;
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            sceneHelper = new SceneHelper();
            Exception nullExc = new Exception("Variable not set to an instance of an object.");
            
            if (xrOrigin == null) Error.LogException(nullExc.Message);
            if (camera == null)
            {
                //Error.LogExceptionNoBreak(nullExc.Message);
                camera = xrOrigin.Find("CameraOffset/Main Camera");
            }
        }

        public void PlayScene(int i) => SceneManager.LoadScene(i);

        public void PlayScene(string name) => SceneManager.LoadScene(name);

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
