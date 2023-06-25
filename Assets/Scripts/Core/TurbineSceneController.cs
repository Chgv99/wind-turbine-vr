using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace WindTurbineVR.Core
{
    public class TurbineSceneController : SceneController
    {
        #region STATIC
        // vars that prevail when SwitchScene() is called
        static TurbineData td;
        #endregion

        /** Wind Speed in km per hour
         * Covering up to level 8 on the Beaufort scale. **/
        [SerializeField][Range(0, 74)] float wind_speed = 0f;
        /** Wind Direction in degrees
         * Taking North as 0/360º. **/
        [SerializeField][Range(0, 359)] float wind_direction = 0f;

        [SerializeField] GameObject climaticControl;

        UnityEvent taskChecked;
        UnityEvent ropeAttached;
        UnityEvent harnessAttached;
        UnityEvent harnessDetached;
        UnityEvent lifelineHalt;
        UnityEvent lifelineRelease;

        public float Wind_speed { get => wind_speed; }
        public float Wind_direction { get => wind_direction; set => wind_direction = value; }
        public UnityEvent TaskChecked { get => taskChecked; }

        public UnityEvent RopeAttached { get => ropeAttached; }
        public UnityEvent HarnessAttached { get => harnessAttached; set => harnessAttached = value; }
        public UnityEvent HarnessDetached { get => harnessDetached; set => harnessDetached = value; }
        public UnityEvent LifelineHalt { get => lifelineHalt; set => lifelineHalt = value; }
        public UnityEvent LifelineRelease { get => lifelineRelease; set => lifelineRelease = value; }

        // Start is called before the first frame update
        public override void Awake()
        {
            //Debug.Log("TurbineSceneController Start()");
            base.Awake();
            
            // TODO: QUITAR IF ANTES DE RELEASE
            /*if (SceneManager.GetActiveScene().buildIndex == 1 ||
                SceneManager.GetActiveScene().buildIndex == 2)
            {
                sceneHelper = new SceneHelper();
                sceneHelper.LoadAsync();
            }*/

            taskChecked = new UnityEvent();
            ropeAttached = new UnityEvent();
            harnessAttached = new UnityEvent();
            HarnessDetached = new UnityEvent();
            lifelineHalt = new UnityEvent();
            lifelineRelease = new UnityEvent();
            //Debug.Log(taskChecked);

            /*if (SceneManager.GetActiveScene().name == "Turbine")
                if (!SceneManager.GetSceneByName("TurbineAerial").isLoaded)
                {
                    SceneManager.LoadScene("TurbineAerial", LoadSceneMode.Additive);
                }*/

            #region STATIC dump
            wind_speed = (td != null) ? td.WindSpeed : 0;
            wind_direction = (td != null) ? td.WindDirection : 0;
            #endregion

            //Debug.Log("wind speed = " + GameObject.Find("Climate").GetComponent<ClimateController>().WindSpeed);

            //if (SceneManager.GetActiveScene().name == "Turbine")
            //StartCoroutine(SwitchSceneCo());
        }

        IEnumerator SwitchSceneCo()
        {
            yield return new WaitForSeconds(5);
            SwitchScene();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SwitchClimaticControlMenu()
        {
            if (climaticControl == null) climaticControl = GameObject.Find("ClimaticControl");

            climaticControl.SetActive(!climaticControl.activeSelf);
        }

        public void SwitchScene()
        {
            #region STATIC load
            td = new TurbineData(wind_speed, wind_direction);
            #endregion

            Debug.Log("SwitchScene from " + SceneManager.GetActiveScene().name);
            sceneHelper.AllowSceneActivation();
        }
    }
}
