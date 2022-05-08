using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/** 
* Implement movement functions
* Must consider devices selected in configuration
* */

public class CharacterAddonsController : MonoBehaviour
{
    CharacterController cc;

    #region Camera
    [SerializeField] Transform cameraOffset;
    [SerializeField] Transform camera;
    #endregion

    #region Vars
    [HideInInspector] public bool hMovement;
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
        cc = GetComponent<CharacterController>();
        
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
        cc.Move(new Vector3(0, currentGravity, 0));
        if (cc.isGrounded) currentGravity = 0;
        #endregion

        #region Movement
        /*if (horizontalMovement)
        {

        }*/
        #endregion
    }

    public void EnableHMovement()
    {
        print("enabling hmovement");
    }

    public void DisableHMovement()
    {
        print("disabling hmovement");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterAddonsController))]
public class CharacterAddonsController_Editor : Editor
{
    private bool hmEnabled = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterAddonsController script = (CharacterAddonsController)target;

        script.gravity = EditorGUILayout.Toggle("Gravity", script.gravity);
        if (script.gravity)
        {
            script.gravityFactor = EditorGUILayout.FloatField("Gravity Factor", script.gravityFactor);
        }

        script.hMovement = EditorGUILayout.Toggle("Horizontal Movement", script.hMovement);
        if (script.hMovement && !hmEnabled)
        {
            //Enable horizontal movement
            script.EnableHMovement();
            hmEnabled = true;
        } 
        else if (!script.hMovement && hmEnabled)
        {
            //Disable horizontal movement
            script.DisableHMovement();
            hmEnabled = false;
        }
    }
}
#endif
