using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;
using UnityEngine.Events;

namespace WindTurbineVR.Object
{
    public class LifelineSocketController : MonoBehaviour
    {
        TurbineSceneController sceneController;

        [SerializeField] UnityEvent ropeAttached;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            ropeAttached = sceneController.RopeAttached;
        }

        public void CallRopeAttached() => CallRopeAttachedAndDeactivateSelf();

        private void CallRopeAttachedAndDeactivateSelf()
        {
            Debug.Log("CallRopeAttachedAndDeactivateSelf");
            ropeAttached?.Invoke();
            StartCoroutine(Deactivate());
        }

        IEnumerator Deactivate()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            gameObject.SetActive(false);
        }
    }
}
