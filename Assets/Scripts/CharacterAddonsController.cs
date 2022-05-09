using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

/** 
* Implement movement functions
* Must consider devices selected in configuration
* */

public class CharacterAddonsController : MonoBehaviour
{
    #region OBJECT REFS
    GameObject ls; //Locomotion System
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

        //script.hMovement = EditorGUILayout.Toggle("Horizontal Movement", script.hMovement);
        
    }
}
#endif
