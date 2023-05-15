using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class GuideUIController : UIController
    {
        GuideModalController guideModalController;

        // Start is called before the first frame update
        void Start()
        {
            color = new Color(255,187,0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Show()
        {
            guideModalController = Instantiate(modalPrefab, transform).GetComponent<GuideModalController>();
            if (DisplayTrigger != DisplayTrigger.Hover) guideModalController.InstantiateCloseButton();

            //modalController.SetContent(Info.Title, Info.Description); //sacar un nivel?
            if (taskControllerList != null) guideModalController.SetContent(GuideOrdinal, Info.Title, Info.Description, taskControllerList);
            else Error.LogExceptionNoBreak("Guide panel with no tasks");
        }

        // taskRemoved event listener
        public void UpdateObjectTasks()
        {
            Debug.Log("UpdateObjectInfo");
            //GuideModalController controller = ModalInstance.GetComponent<GuideModalController>();
            if (guideModalController.UpdateTasks()) TasksCompleted(guideModalController);
            /*if (_infoModalInstance != null) Destroy(_infoModalInstance);
            ShowObjectInfo();*/
        }
    }
}