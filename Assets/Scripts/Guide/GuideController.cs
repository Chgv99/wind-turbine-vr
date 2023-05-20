using Codice.Client.BaseCommands.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WindTurbineVR.Object.Info;
using WindTurbineVR.UI;
using static PlasticGui.LaunchDiffParameters;

namespace WindTurbineVR.Guide
{
    public class GuideController : MonoBehaviour
    {
        public List<GuideInfoController> guideInfoControllers = new List<GuideInfoController>();
        public List<GuidePathController> guidePathControllers = new List<GuidePathController>();

        int totalCount = 0;

        //UnityEvent guidePartCompleted;

        //public UnityEvent GuidePartCompleted { get => guidePartCompleted; set => guidePartCompleted = value; }

        // Start is called before the first frame update
        void Start()
        {
            //GuidePartCompleted = new UnityEvent();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                GuideInfoController guideInfoController = child.GetComponent<GuideInfoController>();
                if (guideInfoController != null) 
                {
                    Debug.Log("GuideInfoController");
                    guideInfoControllers.Add(guideInfoController);
                    Debug.Log("Debug");
                    Debug.Log(guideInfoController.UIInstance);
                    Debug.Log(guideInfoController.UIInstance.GetComponent<UIController>());
                    Debug.Log(guideInfoController.UIInstance.GetComponent<UIController>().Completed);
                    guideInfoController.UIInstance.GetComponent<UIController>().Completed.AddListener(NextGuide);
                    totalCount++;
                }

                // Paths
                GuidePathController guidePathController = child.GetComponent<GuidePathController>();
                if (guidePathController != null)
                {
                    Debug.Log("GuidePathController");
                    guidePathControllers.Add(guidePathController);
                    guidePathController.Disable();
                }
            }

            //Set ordinals
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                GuideInfoController guideInfoController = child.GetComponent<GuideInfoController>();
                int guideInfoControllerI = 0;
                if (guideInfoController != null)
                {
                    guideInfoControllerI++;
                    guideInfoController.UpdateOrdinal(new Vector2(guideInfoControllerI, totalCount));
                }
            }

            EnableNextGuide();
            EnableNextPath(); //enables first path of all
        }

        void EnableNextGuide()
        {
            if (guideInfoControllers.Count <= 0) return;
            guideInfoControllers[0].Enable();
        }

        void DisableLastGuide()
        {
            if (guideInfoControllers.Count <= 0) return;
            guideInfoControllers[0].Disable();
            guideInfoControllers[0].UIInstance.GetComponent<UIController>().Completed.RemoveListener(NextGuide);
            guideInfoControllers.RemoveAt(0);
        }

        void EnableNextPath() => guidePathControllers[0].Enable();

        void DisableLastPath()
        {
            guidePathControllers[0].Disable();
            guidePathControllers.RemoveAt(0);
        }

        void NextGuide()
        {
            Debug.Log("NextGuide");

            DisableLastGuide();
            EnableNextGuide();
            DisableLastPath();
            EnableNextPath();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
