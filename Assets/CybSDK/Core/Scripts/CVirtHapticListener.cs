/************************************************************************************

Filename    :   CVirtHapticListener.cs
Content     :   The HapticListener maintains a list of all playing HapticEmitters and evaluates the resulting haptic feedback.
Created     :   August 8, 2014
Last Updated:	September 11, 2018
Authors     :   Lukas Pfeifhofer
				Stefan Radlwimmer

Copyright   :   Copyright 2018 Cyberith GmbH

Licensed under the AssetStore Free License and the AssetStore Commercial License respectively.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace CybSDK
{
	[RequireComponent(typeof(CVirtDeviceController))]
	public class CVirtHapticListener : MonoBehaviour
	{
		private List<CVirtHapticEmitter> emitters = new List<CVirtHapticEmitter>();

		private CVirtDeviceController deviceController;
		public int maxRange = 60;

		void Awake()
		{
			//Check if this object has a CVirtDeviceController attached
			deviceController = GetComponent<CVirtDeviceController>();
			if (deviceController == null)
			{
				CLogger.LogError("CVirtHapticListener gameobject does not have a CVirtDeviceController attached.");
				this.enabled = false;
				return;
			}

			deviceController.OnCVirtDeviceControllerCallback += OnCVirtDeviceControllerCallback;
		}

		public void OnCVirtDeviceControllerCallback(IVirtDevice virtDevice, CVirtDeviceController.CVirtDeviceControllerCallbackType callbackType)
		{
			if (deviceController == null || !deviceController.IsHapticEnabled())
				return;

			if (!virtDevice.HasHaptic())
				return;

			switch (callbackType)
			{
				case CVirtDeviceController.CVirtDeviceControllerCallbackType.Connect:
					virtDevice.HapticSetGain(3);
					virtDevice.HapticSetFrequency(100);
					virtDevice.HapticSetVolume(0);
					virtDevice.HapticPlay();
					break;

				case CVirtDeviceController.CVirtDeviceControllerCallbackType.Disconnect:
					virtDevice.HapticStop();
					break;
			}
		}

		// Update is called once per frame
		void Update()
		{
			IVirtDevice virtDevice = deviceController.GetDevice();

			if (virtDevice == null || !virtDevice.IsOpen())
				return;

			if (!virtDevice.HasHaptic())
				return;

				float sumForce = 0;

			foreach (CVirtHapticEmitter emitter in emitters)
			{
				float distance = Vector3.Distance(transform.position, emitter.transform.position);
				if (distance < maxRange && distance < emitter.GetRange())
				{
					float force = emitter.EvaluateForce(transform.position) * 100;
					sumForce = SumUpDecibel(sumForce, force);
				}
			}

			virtDevice.HapticSetVolume((int)sumForce);
		}

		// This function is called when the behaviour becomes disabled or inactive
		void OnDisable()
		{
			deviceController.OnCVirtDeviceControllerCallback -= OnCVirtDeviceControllerCallback;
		}

		void OnDestroy()
		{
			if (deviceController == null)
				return;

			IVirtDevice virtDevice = deviceController.GetDevice();

			if (virtDevice == null || !virtDevice.IsOpen())
				return;

			if (!virtDevice.HasHaptic())
				return;

			virtDevice.HapticStop();
		}

		private float SumUpDecibel(float a, float b)
		{
			float powA = Mathf.Pow(a / 10.0f, 10.0f);
			float powB = Mathf.Pow(b / 10.0f, 10.0f);

			float powSum = powA + powB;
			return 10.0f * Mathf.Log10(powSum);
		}

		public void AddEmitter(CVirtHapticEmitter emitter)
		{
			emitters.Add(emitter);
		}

		public void RemoveEmitter(CVirtHapticEmitter emitter)
		{
			emitters.Remove(emitter);
		}
	}
}
