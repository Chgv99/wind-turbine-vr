using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WindTurbineVR.UI
{
    public class InteractableInfoView : InfoView
    {
        GameObject main;

        [Space]
        [SerializeField] GameObject title;
        [SerializeField] GameObject body;
        string url = string.Empty;

        [SerializeField] GameObject videoButton;
        [SerializeField] GameObject closeVideoButton;

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
            title = transform.Find("TitleText").gameObject;
            body = main.transform.Find("BodyText").gameObject;
            //videoButton = main.transform.Find("VideoButton").gameObject;
            //closeVideoButton = main.transform.Find("VideoButton").gameObject;
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
            videoObject.transform.Find("Video").GetComponent<VideoLoader>().Play(url);
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

        protected override void NextPage()
        {
            SetBody(Info.Description[GoToNextPage()]);
        }

        protected override void EndPagination()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateColor(Color color)
        {
            base.UpdateColor(color);
            SetButtonColor(videoButton.GetComponent<Button>(), color);
            SetButtonColor(closeVideoButton.GetComponent<Button>(), color);
        }

        protected override void PreviousPage()
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