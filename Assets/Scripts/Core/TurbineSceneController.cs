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
        static float s_wind_speed;
        static float s_wind_direction;
        #endregion

        /** Wind Speed in km per hour
         * Covering up to level 8 on the Beaufort scale. **/
        [SerializeField][Range(0, 74)] float wind_speed = 0f;
        /** Wind Direction in degrees
         * Taking North as 0/360�. **/
        [SerializeField][Range(0, 359)] float wind_direction = 0f;

        UnityEvent taskChecked;

        public float Wind_speed { get => wind_speed; }
        public float Wind_direction { get => wind_direction; set => wind_direction = value; }
        public UnityEvent TaskChecked { get => taskChecked; }

        // Start is called before the first frame update
        public override void Start()
        {
            Debug.Log("Start()");
            base.Start();
            taskChecked = new UnityEvent();

            sceneHelper.LoadAsync();

            /*if (SceneManager.GetActiveScene().name == "Turbine")
                if (!SceneManager.GetSceneByName("TurbineAerial").isLoaded)
                {
                    SceneManager.LoadScene("TurbineAerial", LoadSceneMode.Additive);
                }*/

            #region STATIC dump
            wind_speed = s_wind_speed;
            wind_direction = s_wind_direction;
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

        void SwitchScene()
        {
            #region STATIC load
            s_wind_speed = wind_speed;
            s_wind_direction = wind_direction;
            #endregion
            Debug.Log("SwitchScene from " + SceneManager.GetActiveScene().name);
            sceneHelper.LoadAsync();
        }
    }
}
