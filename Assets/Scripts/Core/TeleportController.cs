using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public class TeleportController : MonoBehaviour
    {
        SceneController sceneController;

        public Transform destination;

        void Awake()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();

            //destination = transform.Find("Destination");
            if (destination == null) Error.LogException("Teleport destination is null");
        }

        public void Teleport() => sceneController.TeleportPlayer(destination.position);
    }
}
