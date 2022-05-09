using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Transform camera;
    [SerializeField] private GameObject pauseMenu;
    //[SerializeField] private GameObject pauseMenu;

    void Start(){
        if (camera == null) camera = transform.Find("Camera Offset/Main Camera");//.GetComponent<Camera>();
    }

    void Update(){
        Debug.DrawRay(camera.position, camera.forward, Color.red, 10);
    }

    public void Open(){
        pauseMenu.transform.position = transform.position + camera.forward;
        pauseMenu.SetActive(true);
    }

    public void Close(){
        pauseMenu.SetActive(false);
    }
}
