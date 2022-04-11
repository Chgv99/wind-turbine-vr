using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionTextController : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "v" + Application.version;
    }
}
