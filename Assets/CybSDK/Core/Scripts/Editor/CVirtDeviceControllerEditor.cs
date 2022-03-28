/************************************************************************************

Filename    :   CVirtDeviceControllerEditor.cs
Content     :   ___SHORT_DISCRIPTION___
Created     :   August 8, 2014
Last Updated:	September 11, 2018
Authors     :   Lukas Pfeifhofer
				Stefan Radlwimmer

Copyright   :   Copyright 2018 Cyberith GmbH

Licensed under the AssetStore Free License and the AssetStore Commercial License respectively.

************************************************************************************/

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CybSDK
{
	[CustomEditor(typeof(CVirtDeviceController))]
	public class CVirtDeviceControllerEditor : Editor
	{
		string factoryMethod = "CVirtDeviceController.InitVirtualizer";
		private readonly GUIContent deviceTypeLabel;
		private readonly GUIContent[] deviceTypeOptions;
		private readonly GUIContent decouplingTypeLabel;
		private readonly GUIContent[] decouplingTypeOptions;
		private readonly GUIContent activateHapticLabel;
		private readonly GUIContent cabilbrateLabel;

		public CVirtDeviceControllerEditor()
		{
			deviceTypeLabel = new GUIContent("Device Type", "Select the type of device you want to use.");
			deviceTypeOptions = new []
			{
				new GUIContent("[Automatic]", "CVirtDeviceController will automatically choose the best available device for you."),
				new GUIContent("Virtualizer Device", "A native Virtualizer Device hardware device."),
				new GUIContent("Coupled/XInput (Xbox Controller)", "A virtual IVirtDevice, driven by DirectX xInput."),
				new GUIContent("Coupled/Keyboard", "A virtual IVirtDevice, driven by Keyboard input."),
				new GUIContent(string.Format("Custom/Added in '{0}()'", factoryMethod), "If you want to use your own custom IVirtDevice implementation."),
			};

			decouplingTypeLabel = new GUIContent("Decoupling Override", "Override the default decoupling behaviour of the active IVirtDevice.");
			decouplingTypeOptions = new []
			{
				new GUIContent("[Automatic]", "CVirtDeviceController will automatically set the decoupling type for the current device."),
				new GUIContent("Force Coupled", "Force all devices to be coupled, rotating the HMD with them - Default for virtual IVirtualizer devices."),
				new GUIContent("Force Decoupled", "Force all devices to be coupled - Default for a native Virtualizer device."),
			};

			activateHapticLabel = new GUIContent("Haptic Feedback", "Activates the vibration unit.");

			cabilbrateLabel = new GUIContent("Calibrate on connect", "Resets height and orientation on connect - Unused due to absolute tracking.");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			CVirtDeviceController targetScript = (CVirtDeviceController) target;
			
			targetScript.deviceType = (VirtDeviceType) EditorGUILayout.Popup(
				deviceTypeLabel, 
				(int)targetScript.deviceType,
				deviceTypeOptions);

			targetScript.decouplingType = (DecouplingType)EditorGUILayout.Popup(
				decouplingTypeLabel, 
				(int)targetScript.decouplingType,
				decouplingTypeOptions);
			
			targetScript.activateHaptic = EditorGUILayout.Toggle(activateHapticLabel, targetScript.activateHaptic);

			targetScript.calibrateOnConnect = EditorGUILayout.Toggle(
				cabilbrateLabel,
				targetScript.calibrateOnConnect);
			
			serializedObject.ApplyModifiedProperties();
			if (GUI.changed)
			{
				EditorUtility.SetDirty(targetScript);
				EditorSceneManager.MarkSceneDirty(targetScript.gameObject.scene);
			}

			if (EditorApplication.isPlaying)
				Repaint();
		}
	}
}