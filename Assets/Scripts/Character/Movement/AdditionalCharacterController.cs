using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using WindTurbineVR.Core;

namespace WindTurbineVR.Character.Movement
{
    /* TODO:
     * Make this class implement CharacterController
     * and fuse behaviours into one class? 
     */

    public class AdditionalCharacterController : MonoBehaviour
    {
        TurbineSceneController sceneController;

        CapsuleCollider capsuleCollider;

        GameObject leftRayInteractor;
        GameObject rightRayInteractor;

        //ClimbProvider climbProvider;

        public Transform mainCamera;

        [SerializeField] GameObject playerFloor;

        //private CharacterController _characterController;
        private bool _climbing = false;

        [SerializeField] private bool _lifelineHalt = false;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();

            capsuleCollider = GetComponent<CapsuleCollider>();
            mainCamera = transform.Find("CameraOffset/Main Camera");

            leftRayInteractor = transform.Find("CameraOffset/LeftHand Controller/Ray Interactor").gameObject;
            rightRayInteractor = transform.Find("CameraOffset/RightHand Controller/Ray Interactor").gameObject;

            //ClimbProvider.ClimbActive += DisableGravity;
            ClimbProvider.ClimbActive += ClimbActive;
            ClimbProvider.ClimbInActive += ClimbInActive;

            sceneController.LifelineHalt.AddListener(HaltActive);
            sceneController.LifelineRelease.AddListener(HaltInactive);

            capsuleCollider.center = new Vector3(0, (mainCamera.localPosition.y / 2) + 0.2f, 0);
            capsuleCollider.height = mainCamera.localPosition.y;
        }
        private void OnDestroy()
        {
            //ClimbProvider.ClimbActive -= DisableGravity;
            ClimbProvider.ClimbActive -= ClimbActive;
            ClimbProvider.ClimbInActive -= ClimbInActive;
        }

        float floorTimer = 0;

        void Update()
        {
            capsuleCollider.center = new Vector3(0, (mainCamera.localPosition.y / 2) + 0.2f, 0);
            capsuleCollider.height = mainCamera.localPosition.y;
        }

        bool IsGrounded()
        {
            Vector3 rayOrigin = new Vector3(
                transform.position.x + capsuleCollider.center.x, 
                transform.position.y + capsuleCollider.center.y - (capsuleCollider.height / 2) + capsuleCollider.height / 4, 
                transform.position.z + capsuleCollider.center.z);
            float rayLength = (capsuleCollider.height / 3);
            Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);
            Debug.DrawRay(rayOrigin, Quaternion.Euler(45, 0, 0) * Vector3.down * rayLength * 1.25f, Color.green);
            Debug.DrawRay(rayOrigin, Quaternion.Euler(-45, 0, 0) * Vector3.down * rayLength * 1.25f, Color.blue);
            Debug.DrawRay(rayOrigin, Quaternion.Euler(0, 0, 45) * Vector3.down * rayLength * 1.25f, Color.yellow);
            Debug.DrawRay(rayOrigin, Quaternion.Euler(0, 0, -45) * Vector3.down * rayLength * 1.25f, Color.white);
            RaycastHit hit;
            /*if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, LayerMask.GetMask("Default", "Environment")))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }*/

            //LayerMask.GetMask("Default", "Environment")
            return (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, Physics.AllLayers) ||
                Physics.Raycast(rayOrigin, Quaternion.Euler(45, 0, 0) * Vector3.down, out hit, rayLength * 1.25f, Physics.AllLayers) || 
                Physics.Raycast(rayOrigin, Quaternion.Euler(-45, 0, 0) * Vector3.down, out hit, rayLength * 1.25f, Physics.AllLayers) ||
                Physics.Raycast(rayOrigin, Quaternion.Euler(0, 0, 45) * Vector3.down, out hit, rayLength * 1.25f, Physics.AllLayers) ||
                Physics.Raycast(rayOrigin, Quaternion.Euler(0, 0, -45) * Vector3.down, out hit, rayLength * 1.25f, Physics.AllLayers));
        }

        public void EnableGravity() => SetGravity(true);

        public void DisableGravity() => SetGravity(false);

        public void SetGravity(bool state) => GetComponent<Rigidbody>().useGravity = state;

        public void Move(Vector3 direction)
        {
            Debug.Log("moving to " + direction.ToString());
            //transform.Translate(direction);
            transform.position = transform.position + direction;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //transform.rotation = mainCamera.rotation;
            if (!_climbing && !_lifelineHalt)
            {
                EnableGravity();
            } else {
                DisableGravity();
            }

            if (_climbing)
            {
                DisableRays();
            } else {
                EnableRays();
            }

            if (!IsGrounded())
            {
                DisableControl();
                GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            } else if (IsGrounded() && !_climbing && !_lifelineHalt) {
                EnableControl();
            }

            if (_lifelineHalt)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            if (IsGrounded() && !_climbing) Debug.Log("Grounded");

            /*if (_climbing || _lifelineHalt)
            {
                //Debug.Log("floorTimer: " + floorTimer);
                floorTimer += Time.deltaTime;
                if (floorTimer > 2)
                {
                    Debug.Log("show floor");
                    //playerFloor.SetActive(true);
                }
                if (floorTimer > 2.1f)
                {
                    Debug.Log("hide floor");
                    floorTimer = 0;
                    //playerFloor.SetActive(false);
                }
            } else {
                floorTimer = 0;
            }*/

            

            //if (_characterController.is)
        }

        private void HaltActive()
        {
            Debug.Log("HaltActive");
            _lifelineHalt = true;
        }

        private void HaltInactive()
        {
            Debug.Log("HaltInactive");
            _lifelineHalt = false;
        }

        private void ClimbActive()
        {
            _climbing = true;
            
        }

        private void ClimbInActive()
        {
            _climbing = false;
            GetComponent<LocomotionSystem>().enabled = true;
        }

        void EnableRays() => SetRays(true);

        void DisableRays() => SetRays(false);

        void SetRays(bool enabled)
        {
            leftRayInteractor.SetActive(enabled);
            rightRayInteractor.SetActive(enabled);
        }

        void EnableControl() => SetControl(true);

        void DisableControl() => SetControl(false);

        void SetControl(bool state)
        {
            GetComponent<SnapTurnProviderBase>().enabled = state;
            GetComponent<LocomotionSystem>().enabled = state;
            GetComponent<TeleportationProvider>().enabled = state;
            GetComponent<ContinuousTurnProviderBase>().enabled = state;
            GetComponent<DynamicMoveProvider>().enabled = state;
            GetComponent<ContinuousTurnProviderBase>().enabled = state;
        }
    }
}