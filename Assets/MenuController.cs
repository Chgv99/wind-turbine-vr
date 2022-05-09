using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Transform camera;
    [SerializeField] private GameObject pauseMenu;
    
    #region VARS
    [SerializeField] [Range(1f, 4f)] private float distance = 2.5f;
    #endregion

    void Start(){
        if (camera == null) camera = transform.Find("Camera Offset/Main Camera");//.GetComponent<Camera>();
    }

    void Update(){
        Debug.DrawRay(camera.position, camera.forward, Color.red, 10);
    }

    public void Open(){
        //Debug.Break();
        pauseMenu.transform.position = camera.position + camera.forward * distance;
        pauseMenu.transform.forward = camera.forward;
        pauseMenu.SetActive(true);
    }

    public void Close(){
        pauseMenu.SetActive(false);
    }
}
