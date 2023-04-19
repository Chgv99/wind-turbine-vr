using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class ClimateController : MonoBehaviour
    {
        static GameObject _instance;

        Vector2 wind;

        public Vector2 Wind { get => wind; set => wind = value; }

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

            Wind = Vector2.left;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
