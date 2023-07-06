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
        bool status = false;
        float angleNacelle = -1;
        float angleBlades = -1;
        float rotorVelocity = -1;
        #endregion

        public bool Status { get => status; set => status = value; }
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
            statusText.text = statusBase + (status ? "Encendido" : "Apagado");
            angleNacelleText.text = angleNacelleBase + angleNacelle.ToString("n2") + "º";
            angleBladesText.text = angleBladesBase + Mathf.RoundToInt(angleBlades) + "º";
            rotorVelocityText.text = rotorVelocityBase + rotorVelocity.ToString("n2");
        }
    }
}
