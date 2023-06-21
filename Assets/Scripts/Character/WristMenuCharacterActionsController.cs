using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using WindTurbineVR.UI;

namespace WindTurbineVR.Character
{
    public class WristMenuCharacterActionsController : MonoBehaviour
    {
        public InputActionReference leftSwitchWristMenuActionReference = null;

        public InputActionReference rightSwitchWristMenuActionReference = null;

        [Space]
        [SerializeField] WristMenuController leftWristMenu;
        [SerializeField] WristMenuController rightWristMenu;


        bool leftHanded = false;
        //[Space]
        //public UnityEvent buttonPressed;

        // Start is called before the first frame update
        void Awake()
        {
            int tempInt = PlayerPrefs.HasKey("LEFTHANDED") ? PlayerPrefs.GetInt("LEFTHANDED") : 0;
            leftHanded = tempInt == 1 ? true : false;
            
            if (leftHanded) leftSwitchWristMenuActionReference.action.started += CallButtonPressedEvent;
            else rightSwitchWristMenuActionReference.action.started += CallButtonPressedEvent;
        }

        private void OnDestroy()
        {
            if (leftHanded) leftSwitchWristMenuActionReference.action.started -= CallButtonPressedEvent;
            else rightSwitchWristMenuActionReference.action.started -= CallButtonPressedEvent;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CallButtonPressedEvent(InputAction.CallbackContext ctx) => ButtonPressed(); //buttonPressed?.Invoke();

        void ButtonPressed()
        {
            if (leftHanded) leftWristMenu.SwitchActive();
            else rightWristMenu.SwitchActive();
        }
    }
}
