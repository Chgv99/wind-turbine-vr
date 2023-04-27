using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WindTurbineVR.Character
{
    public class WristMenuCharacterActionsController : MonoBehaviour
    {
        public InputActionReference switchMenuActionReference = null;

        public UnityEvent buttonPressed;

        // Start is called before the first frame update
        void Start()
        {
            /* ISSUE: Refactor Assembly Definitions
            if (switchMenuActionReference == null)
                Core.Error.LogException("InputAction reference is null.");
            */
            switchMenuActionReference.action.started += CallButtonPressedEvent;
        }

        private void OnDestroy()
        {
            switchMenuActionReference.action.started -= CallButtonPressedEvent;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CallButtonPressedEvent(InputAction.CallbackContext ctx)
        {
            Debug.Log("CallSwitchToAerialEvent");
            buttonPressed?.Invoke();
        }
    }
}
