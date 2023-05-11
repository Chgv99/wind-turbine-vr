using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WindTurbineVR.Core;

namespace WindTurbineVR
{
    public class ButtonController : MonoBehaviour
    {
        SceneController sceneController;

        Button button;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogExceptionNoBreak("SceneController is null");

            button = GetComponent<Button>();
            button.onClick.AddListener(PlayClickSound);
        }

        void PlayClickSound() => sceneController.PlaySound("Click");
        
        /* Por si el sonido funcionase mal con botones que desaparecen.
        void PlayClickSound() => StartCoroutine(PlaySoundRoutine());

        IEnumerator PlaySoundRoutine()
        {
            yield return null;
            sceneController.PlaySound("Click");
        }*/
    }
}
