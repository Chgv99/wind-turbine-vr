using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Core;
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

        protected GameObject prevPicButton;
        protected GameObject nextPicButton;

        [SerializeField] GameObject videoButton;
        [SerializeField] GameObject closeVideoButton;

        GameObject videoObject; //show on button click

        int picture = 0;

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

            #region Pictures
            picturesObject = transform.Find("Pictures").gameObject;
            picturesButton = main.transform.Find("PicturesButton").gameObject;
            picturesButton.GetComponent<Button>().onClick.AddListener(ShowPictures);

            prevPicButton = picturesObject.transform.Find("NextPrevButtons/PreviousButton").gameObject;
            nextPicButton = picturesObject.transform.Find("NextPrevButtons/NextButton").gameObject;

            prevPicButton.GetComponent<Button>().onClick.AddListener(PreviousPicture);
            nextPicButton.GetComponent<Button>().onClick.AddListener(NextPicture);
            #endregion

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

        public override void Disable()
        {
            base.Disable();
            HideVideo();
        }

        protected override void Show()
        {
            //base.Show();
        }

        protected override void ResetPagination()
        {
            if (Info != null) SetBody(Info.Description[ResetPage()]);
            PreviousPage();
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
            base.NextPage();
        }

        protected override void EndPagination()
        {
            throw new System.NotImplementedException();
        }

        protected void NextPicture()
        {
            int next = GoToNextPicture();
            Debug.Log("next page: " + next);
            SetPicture(Info.Pictures[next]);
            RefreshFitter();
        }

        protected int GoToNextPicture()
        {
            prevPicButton.GetComponent<Button>().interactable = true;
            if (picture + 1 >= Info.Pictures.Length - 1)
            {
                nextPicButton.GetComponent<Button>().interactable = false;
                try
                {
                    EndPagination();
                }
                catch (Exception ex) { Error.LogExceptionNoBreak(ex.Message); }
            }

            if ((picture + 1) < Info.Pictures.Length)
            {
                return ++picture;
            }
            else return -1;
        }

        protected void PreviousPicture()
        {
            int prev = GoToPreviousPicture();
            Debug.Log("next page: " + prev);
            SetPicture(Info.Pictures[prev]);
            RefreshFitter();
        }

        protected int GoToPreviousPicture()
        {
            nextPicButton.GetComponent<Button>().interactable = true;
            if ((picture - 1) <= 0) prevPicButton.GetComponent<Button>().interactable = false;

            if ((picture - 1) >= 0)
            {
                return --picture;
            }
            else return -1;
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