using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WindTurbineVR
{
    public class VersionTextController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<TextMeshProUGUI>().text = Application.version;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
