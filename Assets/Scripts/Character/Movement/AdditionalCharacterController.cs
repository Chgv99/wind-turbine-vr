using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] public InputActionProperty movementDirection;

        [SerializeField] LayerMask layerMask;

        Vector2 direction;

        TurbineSceneController sceneController;

        CapsuleCollider capsuleCollider;
        Rigidbody rb;

        GameObject leftRayInteractor;
        GameObject rightRayInteractor;

        //ClimbProvider climbProvider;

        Transform mainCamera;

        [SerializeField] float jumpForce = 0.75f;

        //[SerializeField] GameObject playerFloor;

        //private CharacterController _characterController;
        private bool _grounded = false;
        private bool _jumping = false;
        private bool _climbing = false;

        [SerializeField] private bool _lifelineHalt = false;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();

            movementDirection.action.performed += OnMove;

            capsuleCollider = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();

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
            movementDirection.action.performed -= OnMove;
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

        /** Enhancement:
         * Make class for ray detection and draw
         * (to not write 800 lines every time
         * we want to test raycasts).*/
        bool CheckStep()
        {
            if (this.direction == null || this.direction == new Vector2()) return false;
            Vector3 direction = new Vector3(this.direction.x, 0, this.direction.y);
            float rayLength = 0.3f;
            
            RaycastHit hit;

            Vector3 rayOrigin = new Vector3(
                transform.position.x + capsuleCollider.center.x,
                transform.position.y + capsuleCollider.center.y - (capsuleCollider.height / 2) + 0.20f,//capsuleCollider.height / 5,
                transform.position.z + capsuleCollider.center.z);
            Debug.DrawRay(rayOrigin, direction * rayLength, Color.yellow);

            bool lowerRay = Physics.Raycast(rayOrigin, direction, out hit, rayLength, layerMask);

            rayOrigin = new Vector3(
                transform.position.x + capsuleCollider.center.x,
                transform.position.y + capsuleCollider.center.y - (capsuleCollider.height / 2) + 0.40f,//capsuleCollider.height / 5,
                transform.position.z + capsuleCollider.center.z);
            Debug.DrawRay(rayOrigin, direction * rayLength, Color.blue);

            bool upperRay = Physics.Raycast(rayOrigin, direction, out hit, rayLength, layerMask);

            // TODO: CHANGE FORWARD FOR MOVEMENT DIRECTION
            if (lowerRay && !upperRay) return true;
            return false;
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

        public void SetGravity(bool state) => rb.useGravity = state;

        public void Move(Vector3 direction)
        {
            //Debug.Log("moving to " + direction.ToString());
            //transform.Translate(direction);
            transform.position = transform.position + direction;
        }

        void OnMove(InputAction.CallbackContext ctx)
        {
            direction = ctx.ReadValue<Vector2>().normalized;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _grounded = IsGrounded();
            //direction = movementDirection.action.performed 

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

            if (_climbing || _lifelineHalt)
            {
                DisableControl();
            } else if (!_climbing && !_lifelineHalt) {
                EnableControl();
            }

            /*if (!_grounded && !_jumping)
            {
                DisableControl();
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            } else if (_grounded && !_climbing && !_lifelineHalt) {
                EnableControl();
            }*/

            if (_lifelineHalt)
            {
                rb.velocity = Vector3.zero;
            }

            /*if (_jumping && _grounded)
            {
                _jumping = false;
            }*/

            if (CheckStep() && _grounded)
            {
                Debug.Log("Step");
                //_jumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            //if (IsGrounded() && !_climbing) Debug.Log("Grounded");

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

        void DisableControl()
        {
            SetControl(false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

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