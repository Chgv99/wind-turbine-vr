using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class ClimateController : MonoBehaviour
    {
        static GameObject _instance;

        private Vector2 wind;
        private Vector2 windDirection;
        private float windSpeed; // de 0 a 100 km/h

        /** Un aerogenerador opera con vientos de entre 10.8 a 90 km/h.
         * Nuestros vientos tendrán un rango mayor para demostrar los
         * protocolos de seguridad.*/

        public Vector2 Wind { get => wind; }
        public Vector2 WindDirection { get => windDirection; set => windDirection = value; }
        public float WindSpeed { get => windSpeed; set => windSpeed = value * 100; }

        //public Vector2 Wind { get => wind; set => wind = value; }

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

            WindDirection = Vector2.left;
        }

        // Update is called once per frame
        void Update()
        {
            wind = WindDirection * WindSpeed;
        }
    }
}
