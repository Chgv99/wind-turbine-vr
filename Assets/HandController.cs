using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Hands {

    public enum HandMode {
        UI,
        WORLD
    }

    public class HandController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public IEnumerator SetMode(HandMode mode){
            switch (mode){
                case HandMode.UI:
                    if (GetComponent<XRDirectInteractor>() != null) Destroy(GetComponent<XRDirectInteractor>());
                    yield return new WaitForSecondsRealtime(0.2f);
                    gameObject.AddComponent<XRRayInteractor>();
                    gameObject.AddComponent<XRInteractorLineVisual>();
                    gameObject.AddComponent<LineRenderer>();
                    break;
                case HandMode.WORLD:
                    if (GetComponent<XRRayInteractor>() != null) Destroy(GetComponent<XRRayInteractor>());
                    if (GetComponent<XRInteractorLineVisual>() != null) Destroy(GetComponent<XRInteractorLineVisual>());
                    if (GetComponent<LineRenderer>() != null) Destroy(GetComponent<LineRenderer>());
                    yield return new WaitForSecondsRealtime(0.2f);
                    gameObject.AddComponent<XRDirectInteractor>();
                    break;
            }
        }
    }
}

