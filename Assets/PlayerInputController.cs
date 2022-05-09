using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public UnityEvent OpenPauseMenu;
    public UnityEvent ClosePauseMenu;

    private XRIDefaultInputActions xri;
    private InputAction pause;

    #region VAR
    bool paused = false;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        xri = new XRIDefaultInputActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(InputAction.CallbackContext obj){
        if (paused) ClosePauseMenu?.Invoke();
        else OpenPauseMenu?.Invoke();
        paused = !paused;
    }

    /*public void Resume(){
        ClosePauseMenu?.Invoke();
    }*/

    void OnEnable(){
        //pause = xri.XRILeftHandInteraction.Pause;
        //pause.Enable();
        
        xri.XRILeftHandInteraction.Pause.performed += Pause;
        xri.XRILeftHandInteraction.Pause.Enable();

        xri.XRIRightHandInteraction.Pause.performed += Pause;
        xri.XRIRightHandInteraction.Pause.Enable();
    }

    void OnDisable(){
        //pause = xri.XRILeftHandInteraction.Pause;
        //pause.Enable();

        xri.XRILeftHandInteraction.Pause.Disable();
        xri.XRIRightHandInteraction.Pause.Disable();
    }
}
