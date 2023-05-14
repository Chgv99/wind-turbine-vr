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

        Vector3 guideOrdinal;

        public Vector3 GuideOrdinal { get => guideOrdinal; set => guideOrdinal = value; }

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");

            /**TODO:
             * GUIDES should only show up when
             * previous one is completed and closed.*/
            //ordinal = transform.parent.GetSiblingIndex() + 1;
            CreateUI();
        }

        /*bool TasksCompleted()
        {
            GuideModalController guideModal = _uiInstance.GetComponent<UIController>().ModalInstance.GetComponent<GuideModalController>();
            return guideModal.TasksCompleted();
        }*/

        void Show()
        {
            if (UiInstance == null) return;

            Enable();
            float height = sceneController.xrOrigin.Find("CameraOffset/Main Camera").position.y;
            UiInstance.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }

        protected override void CreateUI(float height)
        {
            if (UiInstance != null) return;

            base.CreateUI(height);
            //_uiInstance.GetComponent<UIController>().AreaInfoInstance = this.gameObject;
            UiInstance.GetComponent<UIController>().ContentType = ContentType.Guide;
            UiInstance.GetComponent<UIController>().taskControllerList = taskList;
            //_uiInstance.GetComponent<UIController>().GuideOrdinal = 
                //new Vector2(transform.parent.GetSiblingIndex() + 1, transform.parent.parent.childCount);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}