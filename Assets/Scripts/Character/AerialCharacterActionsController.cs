using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WindTurbineVR.Character
{
    public class AerialCharacterActionsController : CharacterActionsController
    {
        public InputActionReference switchAerialActionReference = null;

        public UnityEvent switchToAerial;

        // Start is called before the first frame update
        void Start()
        {
            if (switchAerialActionReference == null)
                Core.Error.LogException("InputAction reference is null.");

            switchAerialActionReference.action.started += CallSwitchToAerialEvent;
        }

        private void OnDestroy()
        {
            switchAerialActionReference.action.started -= CallSwitchToAerialEvent;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void CallSwitchToAerialEvent(InputAction.CallbackContext ctx)
        {
            Debug.Log("CallSwitchToAerialEvent");
            switchToAerial?.Invoke();
        }
    }
}
