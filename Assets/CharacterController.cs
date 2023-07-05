using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WindTurbineVR
{
    public class CharacterController : MonoBehaviour
    {
        public InputActionReference positionActionReference = null;

        public int speed = 0;

        Vector2 move;

        void OnMove(InputAction.CallbackContext ctx)
        {
            move = ctx.ReadValue<Vector2>();
        }

        void Move()
        {
            Vector3 movement = new Vector3(move.x, 0, move.y);

            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (true)
            {
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Move();
        }
    }
}
