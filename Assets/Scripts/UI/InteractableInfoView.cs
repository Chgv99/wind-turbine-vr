using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Data;

namespace WindTurbineVR.UI
{
    public class InteractableInfoView : InfoView
    {
        GameObject main;

        [Space]
        [SerializeField] GameObject title;
        [SerializeField] GameObject body;
        //[SerializeField] GameObject picture;
        string videoUrl = string.Empty;

        [SerializeField] GameObject picturesButton;

        GameObject picturesObject; //show on button click

        [SerializeField] GameObject videoButton;
        [SerializeField] GameObject closeVideoButton;

        GameObject videoObject; //show on button click

        public void SetTitle (string text) => title.GetComponent<TextMeshProUGUI>().text = text;
        public void SetBody(string text) => body.GetComponent<TextMeshProUGUI>().text = text;
        public void SetPicture(Sprite image)
        {
            picturesObject.transform.Find("Image").GetComponent<Image>().sprite = image;
        }
        public void SetVideo(string url)
        {
            this.videoUrl = url;
            if (url != "") videoButton.SetActive(true);
        }
        public string Url { get => videoUrl; set => videoUrl = value; }

        void Awake()
        {
            base.Awake(); //unnecessary?

            main = transform.Find("Main").gameObject;
            title = transform.Find("TitleText").gameObject;
            body = main.transform.Find("BodyText").gameObject;
            
            picturesObject = transform.Find("Pictures").gameObject;
            picturesButton = main.transform.Find("PicturesButton").gameObject;
            picturesButton.GetComponent<Button>().onClick.AddListener(ShowPictures);

            videoObject = transform.Find("Video").gameObject;
            videoButton = main.transform.Find("VideoButton").gameObject;
            videoButton.GetComponent<Button>().onClick.AddListener(ShowVideo);
            //closeVideoButton = main.transform.Find("VideoButton").gameObject;

            nextPrev = main.transform.Find("NextPrevButtons").gameObject;
            prevButton = nextPrev.transform.Find("PreviousButton").gameObject;
            nextButton = nextPrev.transform.Find("NextButton").gameObject;

            prevButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
            nextButton.GetComponent<Button>().onClick.AddListener(NextPage);
        }

        // Start is called before the first frame update
        void Start()
        {
            /*if (DisplayTrigger == DisplayTrigger.Hover ||
                DisplayTrigger == DisplayTrigger.Selection) GetComponent<Canvas>().enabled = false;
            else GetComponent<Canvas>().enabled = false;*/
            //Show();
        }

        public void ShowPictures()
        {
            main.SetActive(false);
            picturesObject.SetActive(true);
            //reset picture shown?
        }

        public void HidePicture()
        {
            main.SetActive(true);
            picturesObject.SetActive(false);
        }

        public void ShowVideo()
        {
            main.SetActive(false);
            videoObject.SetActive(true);
            videoObject.transform.Find("Video").GetComponent<VideoLoader>().Play(videoUrl);
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
            int next = GoToNextPage();
            Debug.Log("next page: " + next);
            SetBody(Info.Description[next]);
            base.NextPage();
        }

        protected override void PreviousPage()
        {
            int previous = GoToPreviousPage();
            Debug.Log("previous page: " + previous);
            SetBody(Info.Description[previous]);
        }

        protected override void EndPagination()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateContent(Info info)
        {
            base.UpdateContent(info);

            Debug.Log("UpdateContent with Info: " + Info.Pictures + ", " + Info.Video);

            if (Info.Video == "") 
            {
                Debug.Log("deactivating picture button");
                videoButton.SetActive(false);
            } 

            if (Info.Pictures.Length == 0) picturesButton.SetActive(false);
            else
            {
                Debug.Log("deactivating picture button");
                picturesButton.SetActive(true);
                if (Info.Pictures.Length > 1) picturesObject.transform.Find("NextPrevButtons").gameObject.SetActive(true);
                else picturesObject.transform.Find("NextPrevButtons").gameObject.SetActive(false);
            }
        }

        public override void UpdateColor(Color color)
        {
            base.UpdateColor(color);
            SetButtonColor(videoButton.GetComponent<Button>(), color);
            SetButtonColor(closeVideoButton.GetComponent<Button>(), color);
        }

        /*public void UpdateContent(string title, string body, string videoUrl)
        {
            this.Title.text = title;
            this.Body.text = body;
        }*/
    }
}