using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WindTurbineVR.VR;

namespace WindTurbineVR.Character.Movement
{
    public class ClimbProvider : MonoBehaviour
    {
        public static event Action ClimbActive;
        public static event Action ClimbInActive;

        public CharacterController characterController;
        public InputActionProperty velocityRight;
        public InputActionProperty velocityLeft;

        [SerializeField] private bool _rightActive = false;
        [SerializeField] private bool _leftActive = false;

        private void Start()
        {
            XRDirectClimbInteractor.ClimbHandActivated += HandActivated;
            XRDirectClimbInteractor.ClimbHandDeactivated += HandDeactivated;
        }

        private void OnDestroy()
        {
            XRDirectClimbInteractor.ClimbHandActivated -= HandActivated;
            XRDirectClimbInteractor.ClimbHandDeactivated -= HandDeactivated;
        }

        private void HandActivated(string _controllerName)
        {
            Debug.Log("HandActivated");
            if (_controllerName == "LeftHand Controller")
            {
                _leftActive = true;
                _rightActive = false;
            }
            else
            {
                _leftActive = false;
                _rightActive = true;
            }

            ClimbActive?.Invoke();
        }

        private void HandDeactivated(string _controllerName)
        {
            Debug.Log("HandDeactivated");
            if (_rightActive && _controllerName == "RightHand Controller")
            {
                _rightActive = false;
                ClimbInActive?.Invoke();
            }
            else if (_leftActive && _controllerName == "LeftHand Controller")
            {
                _leftActive = false;
                ClimbInActive?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (_rightActive || _leftActive)
            {
                Climb();
            }
        }

        private void Climb()
        {
            Vector3 velocity = _leftActive ? velocityLeft.action.ReadValue<Vector3>() : velocityRight.action.ReadValue<Vector3>();
            Debug.Log("Climbing. Velocity: " + velocity.ToString());
            characterController.Move(characterController.transform.rotation * -velocity * Time.fixedDeltaTime);
        }
    }
}