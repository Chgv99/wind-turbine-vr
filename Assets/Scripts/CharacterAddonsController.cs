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

        #region Movement
        currentGravity -= gravityFactor * Time.deltaTime;
        cc.Move(new Vector3(0, currentGravity, 0));
        if (cc.isGrounded) currentGravity = 0;
        #endregion
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
    }
}
#endif
