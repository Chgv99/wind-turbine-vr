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

namespace WindTurbineVR.Object
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
    [RequireComponent(typeof(TintController))]
    [RequireComponent(typeof(Data.Info))]
    public abstract class InfoController : XRSimpleInteractable
    {
        #region PARAMS
        protected float playerHeightMargin = 0f; // % less from player height for panel height from the "ground"
        #endregion


        protected SceneController sceneController;

        [SerializeField] protected GameObject prefabUI;

        [SerializeField] private GameObject uiInstance;

        [SerializeField] Transform parent;

        Data.Info info;

        [Space]
        [SerializeField] protected DisplayMode displayMode;

        [Space]
        [SerializeField] protected DisplayTrigger displayTrigger;

        protected TintController tintController;

        [SerializeField] protected Transform alternativeUI;

        [SerializeField] protected Color color;

        public Data.Info Info { get => info; set => info = value; }
        public GameObject UIInstance { get => uiInstance; set => uiInstance = value; }

        // Start is called before the first frame update
        public virtual void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
            if (sceneController == null) Error.LogException("SceneController is null");
            // Base UI
            //prefabUI = Resources.Load("UI/UI") as GameObject;
            if (prefabUI == null) Error.LogException("PrefabUI is null");
            Info = GetComponent<Data.Info>();
        }

        protected void SetRenderersAndColliders(Transform transform)
        {
            //if (tintController.tintRenderers.Count > 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.childCount > 0) SetRenderersAndColliders(child);

                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null) tintController.tintRenderers.Add(mr);

                Collider collider = child.gameObject.GetComponent<Collider>();
                if (collider != null) colliders.Add(collider);
                //if (meshCollider == null) meshCollider = child.gameObject.AddComponent<MeshCollider>();
            }
        }

        protected bool IsActive()
        {
            return UIInstance.GetComponent<InfoView>().IsActive();
        }

        protected void Enable(HoverEnterEventArgs arg0) => Enable();

        public void Enable()
        {
            //Debug.Log("enable");
            if (UIInstance != null)
            {
                //_uiInstance.SetActive(true);
                //_uiInstance.GetComponent<Canvas>().enabled = true;
                UIInstance.GetComponent<InfoView>().Enable();
            }
        }

        protected void Disable(HoverExitEventArgs arg0) => Disable();

        public void Disable()
        {
            //Debug.Log("disable");
            if (UIInstance != null)
            {
                //_uiInstance.SetActive(false);
                //_uiInstance.GetComponent<Canvas>().enabled = false;
                UIInstance.GetComponent<InfoView>().Disable();
            }
        }

        protected void CreateUITrack() => CreateUI(true);

        protected void CreateUI() => CreateUI(false);

        protected void CreateUI(bool trackHeight)
        {
            float height = 0;
            if (trackHeight)
            {
                height = sceneController.xrOrigin.Find("CameraOffset").localPosition.y;
                height -= height * playerHeightMargin;
            }
            //Vector3 position = transform.position;
            CreateUI(height);
        }

        protected virtual void CreateUI(float height)
        {
            Debug.Log("prefabUI: " + prefabUI);
            UIInstance = Instantiate(prefabUI);

            StartCoroutine(SetParent());

            Vector3 position = new Vector3();
            Quaternion rotation = transform.rotation;

            if (displayMode == DisplayMode.StaticAlternative || 
                displayMode == DisplayMode.StaticAlternativeFixed ||
                displayMode == DisplayMode.StaticAlternativePivot)
            {
                position = (alternativeUI != null) ? alternativeUI.position : position;
                position = new Vector3(position.x, position.y + height, position.z);
                rotation = (alternativeUI != null) ? alternativeUI.rotation : rotation;
            }
            else position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);

            UIInstance.transform.position = position;
            UIInstance.transform.rotation = rotation;

            Debug.Log("Instance: " + UIInstance);
            Debug.Log("InfoView" + UIInstance.GetComponent<InfoView>());
            UIInstance.GetComponent<InfoView>().ContentType = ContentType.ObjectInfo;
            UIInstance.GetComponent<InfoView>().DisplayMode = displayMode;
            UIInstance.GetComponent<InfoView>().DisplayTrigger = displayTrigger;
            UIInstance.GetComponent<InfoView>().Info = Info;
            
            //_uiInstance.GetComponent<UIController>().SetContent();
        }

        IEnumerator SetParent()
        {
            yield return new WaitForSeconds(0.1f);
            UIInstance.transform.parent = parent;
        }

        protected void DisposeUI(HoverExitEventArgs arg0)
        {
            if (UIInstance != null)
            {
                DisposeUI();
            }
        }

        protected void DisposeUI()
        {
            Destroy(UIInstance);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (UIInstance != null)
            {

            }
        }
    }
}