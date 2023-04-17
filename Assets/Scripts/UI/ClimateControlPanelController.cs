using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Core;
using TMPro;

namespace WindTurbineVR.UI
{
    public class ClimateControlPanelController : MonoBehaviour
    {
        ClimateController climate;

        [SerializeField] Slider windSlider;
        [SerializeField] TextMeshProUGUI windSliderText;

        // Start is called before the first frame update
        void Start()
        {
            climate = GameObject.Find("ClimateController").GetComponent<ClimateController>();
            if (climate == null) Error.LogException("ClimateController not found");
            if (windSlider == null) Error.LogException("windSlider not found");
            if (windSliderText == null) Error.LogException("windSliderText not found");
        }

        // Update is called once per frame
        void Update()
        {
            climate.WindSpeed = windSlider.value;
            windSliderText.text = windSlider.value.ToString("F2") + " km/h";
        }
    }
}
