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
        private float windSpeed = 1; // rango de 0 a 1 (de 0 a 100 km/h)

        /** Un aerogenerador opera con vientos de entre 10.8 a 90 km/h.
         * Nuestros vientos tendrán un rango mayor para demostrar los
         * protocolos de seguridad.*/

        public Vector3 Wind { get => wind; }
        public Quaternion WindDirection { get => windDirection; set => windDirection = value; }
        public float WindSpeed { get => windSpeed; set => windSpeed = value * 100; } //devuelve de 0 a 100 km/h

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
            Debug.DrawRay(windTransform.position, WindDirection * Vector3.forward * 10, Color.yellow);
            wind = WindDirection * Vector3.forward * WindSpeed;
            //Debug.Log("Climate Wind (Climate Controller) = " + wind);
        }
    }
}
