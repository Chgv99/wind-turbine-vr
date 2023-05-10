using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindTurbineVR.Core;

namespace WindTurbineVR.Character.Movement
{
    public class CharacterControllerGravity : MonoBehaviour
    {
        TurbineSceneController sceneController;

        [SerializeField] GameObject playerFloor;

        private CharacterController _characterController;
        private bool _climbing = false;

        [SerializeField] private bool _lifelineHalt = false;

        // Start is called before the first frame update
        void Start()
        {
            sceneController = GameObject.Find("SceneController").GetComponent<TurbineSceneController>();
            //rb = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
            ClimbProvider.ClimbActive += ClimbActive;
            ClimbProvider.ClimbInActive += ClimbInActive;

            sceneController.LifelineHalt.AddListener(HaltActive);
            sceneController.LifelineRelease.AddListener(HaltInactive);
        }
        private void OnDestroy()
        {
            ClimbProvider.ClimbActive -= ClimbActive;
            ClimbProvider.ClimbInActive -= ClimbInActive;
        }

        float floorTimer = 0;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_climbing || _lifelineHalt)
            {
                //Debug.Log("floorTimer: " + floorTimer);
                floorTimer += Time.deltaTime;
                if (floorTimer > 2)
                {
                    Debug.Log("show floor");
                    playerFloor.SetActive(true);
                }
                if (floorTimer > 2.1f)
                {
                    Debug.Log("hide floor");
                    floorTimer = 0;
                    playerFloor.SetActive(false);
                }
            } else {
                floorTimer = 0;
            }

            if (!_characterController.isGrounded && !_climbing && !_lifelineHalt)
            {
                Debug.Log("applying gravity");
                _characterController.SimpleMove(new Vector3());
            }
            if (_lifelineHalt)
            {
                //rb.velocity = Vector3.zero;
            }
        }

        private void HaltActive()
        {
            Debug.Log("HaltActive");
            _lifelineHalt = true;
        }

        private void HaltInactive()
        {
            Debug.Log("HaltInactive");
            _lifelineHalt = false;
        }

        private void ClimbActive()
        {
            _climbing = true;
        }

        private void ClimbInActive()
        {
            _climbing = false;
        }
    }
}