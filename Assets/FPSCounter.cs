using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WindTurbineVR
{
    public class FPSCounter : MonoBehaviour
    {
        TextMeshProUGUI tmp;

        // Start is called before the first frame update
        void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            tmp.text = (1 / Time.deltaTime).ToString("F2") + " FPS";
        }
    }
}
