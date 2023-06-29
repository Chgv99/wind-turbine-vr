using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using WindTurbineVR.Object;
using WindTurbineVR.UI;

namespace WindTurbineVR.Object.Info
{
    [RequireComponent(typeof(TeleportController))]
    public class TeleportInfoController : InfoController //?
    {
        //SceneController sceneController;

        TeleportController teleportController;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            
            teleportController = GetComponent<TeleportController>();

            //sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            CreateUITrack();
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

            UIInstance.GetComponent<TeleportInfoView>().SetTitle(Info.Title); //teleportController.destination.gameObject.name
            UIInstance.GetComponent<TeleportInfoView>().SetBody(Info.Description[0]);
            //UIInstance.GetComponent<TeleportInfoView>().UpdateContent(GuideOrdinal, Info, taskManager.GetTasks());
            UIInstance.GetComponent<TeleportInfoView>().TeleportController = teleportController;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}