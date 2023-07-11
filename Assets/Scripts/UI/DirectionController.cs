using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class DirectionController : MonoBehaviour
    {
        Transform camera;
        Transform ui;

        DisplayMode displayMode;

        public bool active = false;

        public Transform UI { get => ui; set => ui = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

        void Awake()
        {
            camera = GameObject.Find("SceneController").GetComponent<SceneController>().Camera;

            //enabled = false;
        }

        void Update()
        {
            if (active) SetDirection();
        }

        /*public DirectionController(SceneController sceneController, Transform transform, DisplayMode displayMode)
        {
            this.UI = transform;
            this.DisplayMode = displayMode;
            Debug.Log("DirectionController constructor");
            camera = sceneController.Camera;
        }*/

        public void SetDirection()
        {
            //Debug.Log("DisplayMode: " + DisplayMode);

            //Debug.Log("Setting direction to " + (UI.position - camera.position));
            //Debug.Log("DIRECTIONCONTROLLER: " + camera);
            transform.rotation = Quaternion.LookRotation(transform.position - camera.position);
        }
    }
}
