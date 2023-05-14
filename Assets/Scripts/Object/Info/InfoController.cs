using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using WindTurbineVR.UI;
using WindTurbineVR.Core;
using System.ComponentModel;
using WindTurbineVR.Data;

// Cambiar a otro namespace. UI probablemente.

namespace WindTurbineVR.Object.Info
{
    /* TODO:
     * 
     * TASKS: Cambiar el modo de dispose de todos los modales y UI
     * por una desactivación simple, para almacenar los estados de
     * las tasks.
     * 
     * Add timer behaviour to hover exit?
     * Merge Info class into InfoController?
     */

    [RequireComponent(typeof(Data.Info))]
    [RequireComponent(typeof(TaskManager))]
    public abstract class InfoController : XRSimpleInteractable
    {
        protected GameObject UI;
        private GameObject uiInstance;

        Data.Info info;

        [Space]
        [SerializeField] protected DisplayMode displayMode;

        [Space]
        [SerializeField] protected DisplayTrigger displayTrigger;

        [SerializeField] protected Transform alternativeUI;

        [Space]
        //[SerializeField] string[] tasks;
        protected List<TaskController> taskList;

        //HoverEnterEvent _triggerEvent;

        public Data.Info Info { get => info; set => info = value; }
        public GameObject UiInstance { get => uiInstance; set => uiInstance = value; }

        // Start is called before the first frame update
        public void Start()
        {
            UI = Resources.Load("UI/UI") as GameObject;
            Info = GetComponent<Data.Info>();

            taskList = GetComponent<TaskManager>().Tasks;
            Debug.Log("taskList on infocontroller:" + taskList.Count);
            //GenerateTasks();
        }

        /*private void GenerateTasks()
        {
            if (tasks.Length <= 0) return;

            taskList = new Task[tasks.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                taskList[i] = new Task(tasks[i]);
                Debug.Log(taskList[i]);
            }
        }*/

        protected void Enable(HoverEnterEventArgs arg0) => Enable();

        public void Enable()
        {
            Debug.Log("enable");
            if (UiInstance != null)
            {
                //_uiInstance.SetActive(true);
                //_uiInstance.GetComponent<Canvas>().enabled = true;
                UiInstance.GetComponent<UIController>().Enable();
            }
        }

        protected void Disable(HoverExitEventArgs arg0) => Disable();

        public void Disable()
        {
            Debug.Log("disable");
            if (UiInstance != null)
            {
                //_uiInstance.SetActive(false);
                //_uiInstance.GetComponent<Canvas>().enabled = false;
                UiInstance.GetComponent<UIController>().Disable();
            }
        }

        protected void CreateUI()
        {
            Vector3 position = transform.position;
            CreateUI(position.y + 0.5f);
        }

        protected virtual void CreateUI(float height)
        {
            UiInstance = Instantiate(UI);

            Vector3 position = new Vector3();
            Quaternion rotation = transform.rotation;

            if (displayMode == DisplayMode.StaticAlternative || displayMode == DisplayMode.StaticAlternativeFixed)
            {
                position = (alternativeUI != null) ? alternativeUI.position : position;
                rotation = (alternativeUI != null) ? alternativeUI.rotation : rotation;
            }
            else position = new Vector3(transform.position.x, height, transform.position.z);

            UiInstance.transform.position = position;
            UiInstance.transform.rotation = rotation;

            UiInstance.GetComponent<UIController>().ContentType = ContentType.ObjectInfo;
            UiInstance.GetComponent<UIController>().DisplayMode = displayMode;
            UiInstance.GetComponent<UIController>().DisplayTrigger = displayTrigger;
            UiInstance.GetComponent<UIController>().Info = Info;
            
            //_uiInstance.GetComponent<UIController>().SetContent();
            if (taskList.Count == 0) Debug.Log("task list is empty");
        }

        protected void DisposeUI(HoverExitEventArgs arg0)
        {
            if (UiInstance != null)
            {
                DisposeUI();
            }
        }

        protected void DisposeUI()
        {
            Destroy(UiInstance);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (UiInstance != null)
            {

            }
        }
    }
}