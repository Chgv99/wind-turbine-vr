using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using Hands;
using CybSDK;

/** 
* Implement movement functions
* Must consider devices selected in configuration
* */
namespace Player{

    public enum MovementMode{
        CONTROLLER,
        PLATFORM
    }

    public class CharacterAddonsController : MonoBehaviour
    {
        #region OBJECT REFS
        #region external
        GameObject ls; //Locomotion System
        #endregion
        #region internal
        [SerializeField] HandController leftHand;
        [SerializeField] HandController rightHand;
        #endregion
        #endregion
        CharacterController cc;

        #region Camera
        [SerializeField] Transform cameraOffset;
        [SerializeField] Transform camera;
        #endregion

        #region Vars
        private bool hmEnabled = false;

        public bool hMovement;
        /*public bool HMovement { 
            get => hMovement; 
            set
            {
                if (value == hMovement) return;
                hMovement = value;

                
            } 
        }*/
        [HideInInspector] public MovementMode controlMode;
        [HideInInspector] public bool gravity = false;
        [HideInInspector] public float gravityFactor = 9.8f;
        float currentGravity;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            ls = GameObject.Find("Locomotion System");
            cc = GetComponent<CharacterController>();
            //ccd = GetComponent<CharacterControllerDriver>();
            
            cameraOffset = transform.Find("Camera Offset");
            camera = cameraOffset.Find("Main Camera");

            leftHand = transform.Find("Camera Offset/LeftHand Controller").GetComponent<HandController>();
            rightHand = transform.Find("Camera Offset/RightHand Controller").GetComponent<HandController>();

            //Player preferences for movement
            if (PlayerPrefs.HasKey("MOVEMENT")){
                if (PlayerPrefs.GetString("MOVEMENT") == "CONTROLLER") SetMode(MovementMode.CONTROLLER);
                if (PlayerPrefs.GetString("MOVEMENT") == "PLATFORM") SetMode(MovementMode.PLATFORM);
            }
        }

        // Update is called once per frame
        void Update()
        {
            cc.height = camera.position.y - cameraOffset.position.y;
            cc.center = new Vector3(cc.center.x, cc.height/2, cc.center.z);

            #region Gravity
            currentGravity -= gravityFactor * Time.deltaTime;
            cc.Move(new Vector3(0, currentGravity, 0) * Time.deltaTime);
            if (cc.isGrounded)
            {
                //print("grounded");
                currentGravity = 0;
            }
            #endregion

            #region Movement
            if (hMovement && !hmEnabled)
            {
                //Enable horizontal movement
                EnableHMovement();
                hmEnabled = true;
            }
            else if (!hMovement && hmEnabled)
            {
                //Disable horizontal movement
                DisableHMovement();
                hmEnabled = false;
            }
            #endregion
        }

        #region MOVEMENT

        #region MODE

        public void SetMode(MovementMode mode){
            switch (mode){
                case MovementMode.CONTROLLER:
                    ls.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                    GetComponent<CVirtPlayerController>().enabled = false;
                    GetComponent<CVirtHapticListener>().enabled = false;
                    break;
                case MovementMode.PLATFORM:
                    ls.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                    GetComponent<CVirtPlayerController>().enabled = true;
                    GetComponent<CVirtHapticListener>().enabled = true;
                    break;
            }
        }

        #endregion

        public void EnableHMovement()
        {
            print("enabling hmovement");
            ls.SetActive(true);
        }

        public void DisableHMovement()
        {
            print("disabling hmovement");
            ls.SetActive(false);
            //ccd.enabled = false;
        }

        #endregion

        #region UI

        public void EnableUIInteraction(){
            print("enabling ui interaction");
            StartCoroutine(leftHand.SetMode(HandMode.UI));
            StartCoroutine(rightHand.SetMode(HandMode.UI));
        }

        public void DisableUIInteraction(){
            print("disabling ui interaction");
            StartCoroutine(leftHand.SetMode(HandMode.WORLD));
            StartCoroutine(rightHand.SetMode(HandMode.WORLD));
        }

        #endregion
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(CharacterAddonsController))]
    public class CharacterAddonsController_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CharacterAddonsController script = (CharacterAddonsController)target;

            script.gravity = EditorGUILayout.Toggle("Gravity", script.gravity);
            if (script.gravity)
            {
                script.gravityFactor = EditorGUILayout.FloatField("Gravity Factor", script.gravityFactor);
            }

            /*GUILayoutOption[] options = new GUILayoutOption[]{ MovementMode.CONTROLLER, MovementMode.PLATFORM};
            script.controlMode = GUILayout.SelectionGrid(new Rect(0,0,100,100), MovementMode.CONTROLLER, options, options.Length, GUI.skin.toggle);
script.SetMode(MovementMode.CONTROLLER);*/
            //script.hMovement = EditorGUILayout.Toggle("Horizontal Movement", script.hMovement);
            
        }
    }
    #endif
}