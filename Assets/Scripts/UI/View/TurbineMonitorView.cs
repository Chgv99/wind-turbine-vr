using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WindTurbineVR.UI
{
    public class TurbineMonitorView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI statusText;
        [SerializeField] TextMeshProUGUI angleNacelleText;
        [SerializeField] TextMeshProUGUI angleBladesText;
        [SerializeField] TextMeshProUGUI rotorVelocityText;

        const string statusBase         = "Estado: ";
        const string angleNacelleBase   = "Ángulo Góndola: ";
        const string angleBladesBase    = "Ángulo Palas: ";
        const string rotorVelocityBase  = "Velocidad Rotor: ";

        #region Private
        //status
        float angleNacelle = -1;
        float angleBlades = -1;
        float rotorVelocity = -1;
        #endregion

        public float AngleNacelle { set => angleNacelle = value; }
        public float AngleBlades { set => angleBlades = value; }
        public float RotorVelocity { set => rotorVelocity = value; }


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //statusText = statusBase + 
            angleNacelleText.text = angleNacelleBase + angleNacelle;
            angleBladesText.text = angleBladesBase + angleNacelle;
            rotorVelocityText.text = rotorVelocityBase + angleNacelle;
        }
    }
}
