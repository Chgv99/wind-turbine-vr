using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class LoadingScreenController : MonoBehaviour
    {
        static LoadingScreenController _instance;

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);

            if (_instance == null)
            {
                _instance = this;
            } 
            else
            {
                Destroy(gameObject);
            }

            SetCamera(SceneController.instance.Camera.GetComponent<Camera>());
        }

        public void SetCamera(Camera camera)
        {
            GetComponent<Canvas>().worldCamera = camera;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Destroy()
        {
            if (_instance == null) return;

            // change background alpha
            // and then destroy
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            if (_instance != null) _instance = null;
        }
    }
}
