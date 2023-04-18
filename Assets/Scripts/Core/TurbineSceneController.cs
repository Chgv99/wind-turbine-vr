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

        UnityEvent taskChecked;

        public float Wind_speed { get => wind_speed; }
        public float Wind_direction { get => wind_direction; set => wind_direction = value; }
        public UnityEvent TaskChecked { get => taskChecked; }

        // Start is called before the first frame update
        public override void Start()
        {
            Debug.Log("TurbineSceneController Start()");
            base.Start();
            sceneHelper = new SceneHelper();
            taskChecked = new UnityEvent();
            Debug.Log(taskChecked);
            sceneHelper.LoadAsync();

            /*if (SceneManager.GetActiveScene().name == "Turbine")
                if (!SceneManager.GetSceneByName("TurbineAerial").isLoaded)
                {
                    SceneManager.LoadScene("TurbineAerial", LoadSceneMode.Additive);
                }*/

            #region STATIC dump
            wind_speed = td.WindSpeed;
            wind_direction = td.WindDirection;
            #endregion

            //if (SceneManager.GetActiveScene().name == "Turbine")
            //    StartCoroutine(SwitchSceneCo());
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
