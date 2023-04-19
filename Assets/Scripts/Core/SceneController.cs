using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WindTurbineVR.Core.Audio;

namespace WindTurbineVR.Core
{
    public class SceneController : MonoBehaviour
    {
        #region Audio
        public AudioController audioController;
        public void PlaySound(string name) => audioController.Play(name);
        #endregion

        [HideInInspector]
        public static SceneController instance;

        public Transform xrOrigin;
        Transform camera;

        //GameObject loadingScreen;
        //static GameObject _loadingScreenInstance;

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
            instance = this;
            //sceneHelper = new SceneHelper();
            
            //loadingScreen = Resources.Load("UI/LoadingScreen") as GameObject;
            //_loadingScreenInstance.GetComponent<LoadingScreenController>().SetCamera(Camera.GetComponent<Camera>());
            //if (_loadingScreenInstance != null) _loadingScreenInstance.GetComponent<LoadingScreenController>().Destroy();

            Debug.Log("SceneController Start()");
            Exception nullExc = new Exception("Variable not set to an instance of an object.");

            if (audioController == null) Error.LogExceptionNoBreak(nullExc.Message);
            if (xrOrigin == null) Error.LogException(nullExc.Message);
            if (camera == null) camera = xrOrigin.Find("CameraOffset/Main Camera");

            audioController.Play("Test");
        }

        public void PlayScene(int i)
        {
            //_loadingScreenInstance = Instantiate(loadingScreen, camera);
            camera.Find("LoadingScreen").GetComponent<LoadingScreenController>().StartLoading();
            SceneManager.LoadScene(i);
        }

        public void PlayScene(string name)
        {
            //_loadingScreenInstance = Instantiate(loadingScreen, camera);
            camera.Find("LoadingScreen").GetComponent<LoadingScreenController>().StartLoading();
            SceneManager.LoadScene(name);
        }

        /*public void EventLog(string str)
        {
            Debug.Log(str);
            //Debug.Break();
        }*/

        public void Quit()
        {
            Application.Quit();
        }
    }
}
