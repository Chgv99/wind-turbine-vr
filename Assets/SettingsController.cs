using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    #region GAMEOBJECTS
    #region MOVEMENT CONFIG GAMEOBJECTS
    public GameObject dummy1;
    public GameObject dummy2;
    #endregion
    #region SOUND CONFIG GAMEOBJECTS
    public Slider sfx;
    public Slider music;
    #endregion
    #endregion

    #region SOUND

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        #region GAMEOBJECT REFS
        #region MOVEMENT CONFIG GAMEOBJECTS
        //dummy1 =;
        //dummy2 =;
        #endregion
        #region SOUND CONFIG GAMEOBJECTS
        sfx = transform.Find("Sound/Buttons/SFX/SliderContainer/Slider").GetComponent<Slider>();
        music = transform.Find("Sound/Buttons/MUSIC/SliderContainer/Slider").GetComponent<Slider>();

        float vol_sfx = 1;
        float vol_music = 1;
        if (PlayerPrefs.HasKey("VOL_SFX")) vol_sfx = PlayerPrefs.GetFloat("VOL_SFX");
        else PlayerPrefs.SetFloat("VOL_SFX", vol_sfx);
        if (PlayerPrefs.HasKey("VOL_MUSIC")) vol_music = PlayerPrefs.GetFloat("VOL_MUSIC");
        else PlayerPrefs.SetFloat("VOL_MUSIC", vol_music);
        sfx.value = vol_sfx;
        music.value = vol_music;

        #endregion
        #endregion
    }

    public void UpdateSFX(float value)
    {
        PlayerPrefs.SetFloat("VOL_SFX", value);
    }

    public void UpdateMusic(float value)
    {
        PlayerPrefs.SetFloat("VOL_MUSIC", value);
    }

    #region MOVEMENT

    public void SelectController(bool selection){
        if (selection) PlayerPrefs.SetString("MOVEMENT", "CONTROLLER");
    }

    public void SelectPlatform(bool selection){
        if (selection) PlayerPrefs.SetString("MOVEMENT", "PLATFORM");
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
