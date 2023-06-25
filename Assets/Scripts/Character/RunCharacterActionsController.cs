using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace WindTurbineVR.Character
{
    public class RunCharacterActionsController : MonoBehaviour
    {
        public InputActionReference switchRunActionReference = null;

        DynamicMoveProvider moveProvider;

        [SerializeField] float runFactor = 2f;
        [SerializeField] float walkSpeed = 1f;

        // Start is called before the first frame update
        void Awake()
        {
            switchRunActionReference.action.started += SetRunSpeed;
            switchRunActionReference.action.canceled += SetWalkSpeed;
        }

        void OnDestroy()
        {
            switchRunActionReference.action.started -= SetRunSpeed;
            switchRunActionReference.action.canceled -= SetWalkSpeed;
        }

        void SetRunSpeed(InputAction.CallbackContext ctx) => SetSpeed(walkSpeed * runFactor);

        void SetWalkSpeed(InputAction.CallbackContext ctx) => SetSpeed(walkSpeed);

        void SetSpeed(float speed) => moveProvider.moveSpeed = speed;

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
