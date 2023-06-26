using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**TODO:
 * MOVER EL DUMP DE LA TURBINA AQUÍ
 */

namespace WindTurbineVR.Core
{
    public class ClimateController : MonoBehaviour
    {
        static GameObject _instance;

        [SerializeField] Transform windTransform;

        private Vector3 wind;
        private Quaternion windDirection;
        [SerializeField] [Range(0,100)] private float windSpeed = 0; // de 0 a 100 km/h

        /** Un aerogenerador opera con vientos de entre 10.8 a 90 km/h.
         * Nuestros vientos tendrán un rango mayor para demostrar los
         * protocolos de seguridad.*/

        public Vector3 Wind { get => wind; }
        public Quaternion WindDirection { get => windDirection; set => windDirection = value; }
        public float WindSpeed { get => windSpeed; set => windSpeed = value; } //devuelve de 0 a 100 km/h

        // Start is called before the first frame update
        void Start()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            _instance = gameObject;

            WindDirection = windTransform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("--------------------");
            //Debug.Log("WindTransform eulerAngles = " + windTransform.eulerAngles);
            WindDirection = windTransform.rotation; // * Vector3.forward;
            wind = WindDirection * Vector3.forward * WindSpeed;
            Debug.Log("wind: " + wind);
            Debug.DrawRay(windTransform.position, wind, Color.yellow);
            
            //Debug.Log("Climate Wind (Climate Controller) = " + wind);
        }

        public void SetWind(Quaternion rotation) => windTransform.rotation = rotation;
    }
}
