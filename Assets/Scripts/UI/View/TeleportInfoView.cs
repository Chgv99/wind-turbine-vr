using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        void Awake()
        {
            base.Awake(); //unnecessary?

            //teleportController = GetComponent<TeleportController>();

            main = transform.Find("Main").gameObject;
            title = transform.Find("TitleText").gameObject;
            body = main.transform.Find("BodyText").gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (DisplayTrigger == DisplayTrigger.Hover) GetComponent<Canvas>().enabled = false;
            //Show();
        }

        public void Teleport() => teleportController.Teleport();

        protected override void Show()
        {
            //base.Show();
        }

        /*public void UpdateContent(string title, string body, string videoUrl)
        {
            this.Title.text = title;
            this.Body.text = body;
        }*/
    }
}