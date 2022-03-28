﻿using UnityEngine;
using UnityEditor;

namespace CybSDK
{
    [CustomEditor(typeof(CVirtHapticEmitter))]
    public class CVirtHapticEmitterEditor : Editor
    {
	    private static Color orange = new Color(1f, 0.549f, 0f);

        public void OnSceneGUI()
        {
            CVirtHapticEmitter targetScript = (CVirtHapticEmitter)target;

            Handles.color = orange;
			Handles.DrawWireDisc(targetScript.transform.position, targetScript.transform.up, targetScript.GetRange());
		}

	}

}
