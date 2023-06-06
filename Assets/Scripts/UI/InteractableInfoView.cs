using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.UI
{
    public class InteractableInfoView : InfoView
    {
        // Start is called before the first frame update
        void Start()
        {
            if (DisplayTrigger == DisplayTrigger.Hover) GetComponent<Canvas>().enabled = false;
            //Show();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void Show()
        {
            //base.Show();
        }
    }
}