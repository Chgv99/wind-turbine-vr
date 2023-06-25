using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class WristMenuController : MonoBehaviour
    {
        TurbineSceneController sceneController;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Button Actions

        public void ClimaticControlMenu()
        {
            sceneController.SwitchClimaticControlMenu();
        }

        // try to name the same as the button
        public void MainMenu() => sceneController.ReturnToMenu();
        #endregion

        #region Menu State
        public void SwitchActive()
        {
            Debug.Log("SwitchActive");
            gameObject.SetActive(!gameObject.activeSelf);
        }
        #endregion
    }
}
