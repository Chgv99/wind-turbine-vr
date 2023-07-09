using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class TeleportInfoView : InfoView
    {
        TeleportController teleportController;

        [SerializeField] GameObject main;
        [SerializeField] GameObject title;
        [SerializeField] GameObject body;

        Button button;

        public void SetTitle(string text) { this.title.GetComponent<TextMeshProUGUI>().text = text; }
        public void SetBody(string text) { this.body.GetComponent<TextMeshProUGUI>().text = text; }
        public TeleportController TeleportController { set => teleportController = value; }

        protected override void Awake()
        {
            base.Awake(); //unnecessary?

            playerTeleported = sceneController.PlayerTeleported;
            //teleportController = GetComponent<TeleportController>();

            main = transform.Find("Main").gameObject;
            title = transform.Find("TitleText").gameObject;
            body = main.transform.Find("BodyText").gameObject;

            if (DisplayTrigger == DisplayTrigger.Hover) GetComponent<Canvas>().enabled = false;
        }

        // Start is called before the first frame update
        //void Start()
        //{

            //Show();
        //}

        public void Teleport()
        {
            //Disable();
            playerTeleported?.Invoke();
            teleportController.Teleport();
        }

        protected override void SetTeleportedAction()
        {
            playerTeleported.AddListener(Disable);
        }

        protected override void Show()
        {
            //base.Show();
        }

        protected override void EndPagination()
        {
            throw new System.NotImplementedException();
        }

        protected override void NextPage()
        {
            throw new System.NotImplementedException();
        }

        protected override void PreviousPage()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResetPagination()
        {
            throw new System.NotImplementedException();
        }

        /*public void UpdateContent(string title, string body, string videoUrl)
        {
            this.Title.text = title;
            this.Body.text = body;
        }*/
    }
}