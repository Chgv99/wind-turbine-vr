using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WindTurbineVR.UI
{
    public class InteractableInfoView : InfoView
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI bodyText;

        [SerializeField] GameObject video;

        void Awake()
        {
            base.Awake(); //unnecessary?

            titleText = transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
            bodyText = transform.Find("BodyText").GetComponent<TextMeshProUGUI>();
            video = transform.Find("Video").gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (DisplayTrigger == DisplayTrigger.Hover) GetComponent<Canvas>().enabled = false;
            //Show();
        }

        protected override void Show()
        {
            //base.Show();
        }

        public void UpdateContent(string title, string body, string videoUrl)
        {
            titleText.text = title;
            bodyText.text = body;
        }
    }
}