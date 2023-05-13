using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Info
{
    public class GuideInfoController : InfoController
    {
        SceneController sceneController;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            /**TODO:
             * GUIDES should only show up when
             * previous one is completed and closed.*/

            CreateUI();
        }

        void Show()
        {
            if (_uiInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            _uiInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        protected override void CreateUI(float height)
        {
            if (_uiInstance != null) return;

            base.CreateUI(height);
            //_uiInstance.GetComponent<UIController>().AreaInfoInstance = this.gameObject;
            _uiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
            _uiInstance.GetComponent<UIController>().taskControllerList = taskList;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}