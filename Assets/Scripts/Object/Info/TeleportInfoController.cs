using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.Object;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Info
{
    public class TeleportInfoController : InfoController //?
    {
        SceneController sceneController;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            CreateUI();
        }

        void Show()
        {
            if (UIInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            UIInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        protected override void CreateUI(float height)
        {
            if (UIInstance != null) return;

            base.CreateUI(height);

            UIInstance.GetComponent<TeleportInfoView>().SetTitle(Info.Title);
            UIInstance.GetComponent<TeleportInfoView>().SetBody(Info.Description);
            //UIInstance.GetComponent<TeleportInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}