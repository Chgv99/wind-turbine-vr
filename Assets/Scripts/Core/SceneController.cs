using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
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

                //if (activeScene.name == "Turbine") asyncOperation = SceneManager.LoadSceneAsync("TurbineAerial");
                //if (activeScene.name == "TurbineAerial") asyncOperation = SceneManager.LoadSceneAsync("Turbine");
                if (activeScene.buildIndex == 1) asyncOperation = SceneManager.LoadSceneAsync(2);
                if (activeScene.buildIndex == 2) asyncOperation = SceneManager.LoadSceneAsync(1);

                asyncOperation.allowSceneActivation = false;
            }

            public void AllowSceneActivation() => asyncOperation.allowSceneActivation = true;
        }

        // Start is called before the first frame update
        public virtual void Awake()
        {
            Debug.Log("SceneController Start()");
            Exception nullExc = new Exception("Variable not set to an instance of an object.");

            if (audioController == null) Error.LogExceptionNoBreak(nullExc.Message);
            if (xrOrigin == null) Error.LogException(nullExc.Message);
            if (camera == null) camera = xrOrigin.Find("CameraOffset/Main Camera");

            audioController.Play("Test");

            Time.fixedDeltaTime = 1 / 60f;
        }

        public void ReturnToMenu() => PlayScene(0);

        public void PlayScene(int i) => SceneManager.LoadScene(i);

        public void PlayScene(string name) => SceneManager.LoadScene(name);

        public void EventLog(string str)
        {
            Debug.Log(str);
            //Debug.Break();
        }

        public void TeleportPlayer(Vector3 destination)
        {
            Debug.Log("teleporting player");
            xrOrigin.position = destination;
            //Debug.Break();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
