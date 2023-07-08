using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.UI
{
    public class DirectionController
    {
        Transform camera;
        Transform ui;

        DisplayMode displayMode;

        public Transform UI { get => ui; set => ui = value; }
        public DisplayMode DisplayMode { get => displayMode; set => displayMode = value; }

        public DirectionController(SceneController sceneController, Transform transform, DisplayMode displayMode)
        {
            this.UI = transform;
            this.DisplayMode = displayMode;
            Debug.Log("DirectionController constructor");
            camera = sceneController.Camera;
        }

        public void SetDirection()
        {
            //Debug.Log("DisplayMode: " + DisplayMode);

            //Debug.Log("Setting direction to " + (UI.position - camera.position));
            UI.rotation = Quaternion.LookRotation(UI.position - camera.position);
        }
    }
}
