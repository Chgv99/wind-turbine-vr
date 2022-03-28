/************************************************************************************

Filename    :   CVirtPlayerController.cs
Content     :   PlayerController takes input from a VirtDevice and moves the player respectively.
Created     :   August 8, 2014
Last Updated:	September 11, 2018
Authors     :   Lukas Pfeifhofer
				Stefan Radlwimmer

Copyright   :   Copyright 2018 Cyberith GmbH

Licensed under the AssetStore Free License and the AssetStore Commercial License respectively.

************************************************************************************/

using UnityEngine;
using CybSDK;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CVirtDeviceController))]
public class CVirtPlayerController : MonoBehaviour
{
	private CVirtDeviceController deviceController;
	private CharacterController characterController;

	[Tooltip("Reference to a GameObject that will be rotated according to the player’s orientation in the device. If not set will search for 'ForwardDirection' attached to GameObject.")]
	public Transform forwardDirection;

	[Tooltip("Movement Speed Multiplier, to fine tune the players speed.")]
	[Range(0, 10)]
	public float movementSpeedMultiplier = 1.2f;

	// Use this for initialization
	void Start()
	{
		// Find the forward direction        
		if (forwardDirection == null)
		{
			forwardDirection = transform.Find("ForwardDirection");
		}

		//Check if this object has a CVirtDeviceController attached
		deviceController = GetComponent<CVirtDeviceController>();
		if (deviceController == null)
		{
			CLogger.LogError(string.Format("CVirtPlayerController requires a CVirtDeviceController attached to gameobject '{0}'.", gameObject.name));
			enabled = false;
			return;
		}

		//Check if this object has a CharacterController attached
		characterController = GetComponent<CharacterController>();
		if (characterController == null)
		{
			CLogger.LogError(string.Format("CVirtPlayerController requires a CharacterController attached to gameobject '{0}'.", gameObject.name));
			enabled = false;
			return;
		}
	}

	// Update is called once per frame
	void Update()
	{
		IVirtDevice device = deviceController.GetDevice();

		if (device == null || !device.IsOpen())
			return;

		// MOVE
		///////////
		Vector3 movement = device.GetMovementVector() * movementSpeedMultiplier;

		// ROTATION
		///////////
		Quaternion localOrientation = device.GetPlayerOrientationQuaternion();

		// Determine global orientation for characterController Movement
		Quaternion globalOrientation;

		// For decoupled movement we do not rotate the pawn --> HMD does that
		if (deviceController.IsDecoupled())
		{
			if (forwardDirection != null)
			{
				forwardDirection.transform.localRotation = localOrientation;
				globalOrientation = forwardDirection.transform.rotation;
			}
			else
			{
				// Quaternions are applied right to left
				globalOrientation = localOrientation * gameObject.transform.rotation;
			}
		}
		// For coupled movement we rotate the pawn and HMD
		else
		{
			gameObject.transform.rotation = localOrientation;
			globalOrientation = localOrientation;
		}

		Vector3 motionVector = globalOrientation * movement;
		characterController.SimpleMove(motionVector);
	}
}