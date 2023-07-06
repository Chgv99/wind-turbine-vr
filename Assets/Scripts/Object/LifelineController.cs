using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.Core;

namespace WindTurbineVR.Object
{
    public class LifelineController : MonoBehaviour
    {
        Rigidbody rb;
        TurbineSceneController sceneController;

        [SerializeField] Transform harness;

        [SerializeField] bool attached = false;
        [SerializeField] bool harnessAttached = false;

        public bool Attached { get => attached; set => attached = value; }
        public bool HarnessAttached { get => harnessAttached; set => harnessAttached = value; }

        [SerializeField] bool semaphore = true;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            //Debug.Log("Event " + sceneController.RopeAttached);
            Debug.Log("event: " + sceneController.RopeAttached);
            sceneController.RopeAttached.AddListener(Attach);
            sceneController.HarnessAttached.AddListener(HarnessAttach);
            sceneController.HarnessDetached.AddListener(HarnessDetach);

            if (harness == null) Error.LogException("Harness is null");
        }

        // Update is called once per frame
        void Update()
        {
            if (Attached && HarnessAttached)
            {
                //check for harness height
                //check if distance is greater than X, so that the rope is more extended when halt
                if (harness.position.y < transform.position.y - 0.4f && semaphore)
                {
                    Debug.Log("HALT MOVEMENT. PLAYER FALLING");
                    sceneController.LifelineHalt?.Invoke();
                    semaphore = false;
                    rb.velocity = Vector3.zero;
                    rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
                }
                if (harness.position.y >= transform.position.y && !semaphore) {
                    Debug.Log("RELEASE MOVEMENT. PLAYER SAFE");
                    sceneController.LifelineRelease?.Invoke();
                    semaphore = true;
                    rb.constraints =
                        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
            }
        }

        void HarnessAttach()
        {
            HarnessAttached = true;
        }

        void HarnessDetach()
        {
            HarnessAttached = false;
        }

        void Attach()
        {
            Debug.Log("Attached");
            Attached = true;
            rb.constraints = 
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            StartCoroutine(Deactivate());
        }

        IEnumerator Deactivate()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            rb.useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
}
