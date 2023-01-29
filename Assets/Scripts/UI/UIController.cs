using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WindTurbineVR.UI
{
    public enum Mode
    {
        None = 0,
        ObjectInfo,
        Menu
    }

    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject infoModal;

        Mode mode = Mode.None;

        bool initializated = false;

        public Mode Mode { get => mode; set => mode = value; }

        void Awake()
        {
            infoModal = Resources.Load("InfoModal") as GameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (!initializated)
            {
                if (mode == Mode.None) return;

                initializated = true;

                switch (mode)
                {
                    case Mode.ObjectInfo:
                        ShowObjectInfo();
                        break;
                }
            }
        }

        private void ShowObjectInfo()
        {
            GameObject infoModalInstance = Instantiate(infoModal, transform);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}