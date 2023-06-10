using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WindTurbineVR.UI
{
    public class InteractableInfoView : InfoView
    {
        [SerializeField] GameObject main;
        [SerializeField] GameObject title;
        [SerializeField] GameObject body;

        [SerializeField] string url = string.Empty;

        GameObject videoButton;
        GameObject videoObject; //show on button click

        public void SetTitle (string text)  { this.title.GetComponent<TextMeshProUGUI>().text = text; }
        public void SetBody(string text) { this.body.GetComponent<TextMeshProUGUI>().text = text; }
        public void SetUrl(string url)
        {
            this.url = url;
            if (url != "") videoButton.SetActive(true);
        }
        public string Url { get => url; set => url = value; }

        void Awake()
        {
            base.Awake(); //unnecessary?

            main = transform.Find("Main").gameObject;
            title = main.transform.Find("TitleText").gameObject;
            body = main.transform.Find("BodyText").gameObject;
            videoButton = main.transform.Find("VideoButton").gameObject;
            videoObject = transform.Find("Video").gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (DisplayTrigger == DisplayTrigger.Hover) GetComponent<Canvas>().enabled = false;
            //Show();
        }

        public void ShowVideo()
        {
            main.SetActive(false);
            videoObject.SetActive(true);
        }

        public void HideVideo()
        {
            main.SetActive(true);
            videoObject.SetActive(false);
        }

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