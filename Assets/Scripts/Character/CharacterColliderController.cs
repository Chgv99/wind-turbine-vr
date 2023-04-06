using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindTurbineVR.Character
{
    public class CharacterColliderController : MonoBehaviour
    {
        Transform characterCamera;

        CapsuleCollider collider;

        // Start is called before the first frame update
        void Start()
        {
            characterCamera = transform.parent.parent.Find("Main Camera");
            collider = GetComponent<CapsuleCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            collider.height = characterCamera.position.y;
            transform.position = new Vector3(
                characterCamera.position.x, 
                characterCamera.position.y / 2, 
                characterCamera.position.z);
        }
    }
}
