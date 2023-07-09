﻿using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
//using WindTurbineVR.Core.Scriptable;

namespace WindTurbineVR.Data
{
    public class Info : MonoBehaviour
    {
        public bool useInfoCard = false;

        [SerializeField] Scriptable.InfoCard infoCard;

        [SerializeField] string title;
        [SerializeField] string[] description;
        [SerializeField] Sprite[] pictures;
        [SerializeField] string video;
        [SerializeField] string congratsMessage;
        public string Title 
        { 
            get
            {
                return infoCard != null ? infoCard.title : title;
            }
            set => title = value; 
        }

        public string[] Description 
        { 
            get 
            {
                return infoCard != null ? infoCard.description : description;
                //return arr == new string[] { } ? new string[1] { "empty" } : arr;
            } 
            set => description = value; 
        }

        public Sprite[] Pictures
        {
            get
            {
                return infoCard != null ? infoCard.pictures : pictures;
            }
            set => pictures = value;
        }

        public string Video
        {
            get
            {
                string str = infoCard != null ? infoCard.video : video;
                return ConvertString(str);
            }
            set => video = value;
        }
        public string CongratsMessage //yes, this.congratsMessage has priority
        {
            get
            {
                return congratsMessage == "" ? infoCard.congratsMessage : congratsMessage;
            }
        }

        string ConvertString(string input)
        {
            string newStr = Regex.Replace(input, "//[w]{3}", "//dl");
            return Regex.Replace(newStr, "\\?dl=0", "\\?dl=1");
        }

        // https://dl.dropbox.com/s/dcs00rjqx2u0zpl/Grand%20Theft%20Auto%20V%202019.01.16%20-%2022.35.51.02.mp4?dl=1
        // https://www.dropbox.com/s/anc896dcraq0qnm/SnapSave.io-%C2%BFC%C3%B3mo%20funciona%20un%20aerogenerador_%20%20_%20Sostenibilidad%20-%20ACCIONA%28720p%29.mp4?dl=0 
    }
}
