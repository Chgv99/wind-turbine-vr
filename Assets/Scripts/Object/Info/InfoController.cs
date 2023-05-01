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
        protected GameObject _uiInstance;

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

        protected void Enable()
        {
            Debug.Log("enable");
            if (_uiInstance != null)
            {
                _uiInstance.SetActive(true);
            }
        }

        protected void Disable(HoverExitEventArgs arg0) => Disable();

        protected void Disable()
        {
            Debug.Log("disable");
            if (_uiInstance != null)
            {
                _uiInstance.SetActive(false);
            }
        }

        protected void CreateUI()
        {
            Vector3 position = transform.position;
            CreateUI(position.y + 0.5f);
        }

        protected virtual void CreateUI(float height)
        {
            _uiInstance = Instantiate(UI);

            Vector3 position = new Vector3();
            Quaternion rotation = new Quaternion();

            if (displayMode == DisplayMode.StaticAlternative || displayMode == DisplayMode.StaticAlternativeFixed)
            {
                position = (alternativeUI != null) ? alternativeUI.position : position;
                rotation = (alternativeUI != null) ? alternativeUI.rotation : rotation;
            }
            else position = new Vector3(transform.position.x, height, transform.position.z);

            _uiInstance.transform.position = position;
            _uiInstance.transform.rotation = rotation;

            _uiInstance.GetComponent<UIController>().ContentType = ContentType.ObjectInfo;
            _uiInstance.GetComponent<UIController>().DisplayMode = displayMode;
            _uiInstance.GetComponent<UIController>().DisplayTrigger = displayTrigger;
            _uiInstance.GetComponent<UIController>().Info = Info;
            _uiInstance.GetComponent<UIController>().taskControllerList = taskList;
        }

        protected void DisposeUI(HoverExitEventArgs arg0)
        {
            if (_uiInstance != null)
            {
                DisposeUI();
            }
        }

        protected void DisposeUI()
        {
            Destroy(_uiInstance);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_uiInstance != null)
            {

            }
        }
    }
}