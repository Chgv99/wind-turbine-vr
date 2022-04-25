using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    //[SerializeField] private GameObject pauseMenu;

    public void Open(){
        pauseMenu.SetActive(true);
    }

    public void Close(){
        pauseMenu.SetActive(false);
    }
}
