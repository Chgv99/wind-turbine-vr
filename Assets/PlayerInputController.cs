using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : MonoBehaviour
{
    public UnityEvent OpenPauseMenu;
    public UnityEvent ClosePauseMenu;

    #region VAR
    [SerializeField] bool paused = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(){
        if (paused) ClosePauseMenu?.Invoke();
        else OpenPauseMenu?.Invoke();
        paused = !paused;
    }

    /*public void Resume(){
        ClosePauseMenu?.Invoke();
    }*/
}
