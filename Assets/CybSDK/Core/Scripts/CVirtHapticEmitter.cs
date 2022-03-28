/************************************************************************************

Filename    :   CVirtHapticEmitter.cs
Content     :   The HapticEmitter emitts an animated haptic effect to be recieved from a HapticListener.
Created     :   August 8, 2014
Last Updated:	September 11, 2018
Authors     :   Lukas Pfeifhofer
				Stefan Radlwimmer

Copyright   :   Copyright 2018 Cyberith GmbH

Licensed under the AssetStore Free License and the AssetStore Commercial License respectively.

************************************************************************************/

using UnityEngine;

namespace CybSDK
{

	public class CVirtHapticEmitter : MonoBehaviour
	{
		[Tooltip("Reference to the Haptic Listener receiving Haptic Feedback. If not set will find one in scene")]
		public CVirtHapticListener hapticListener;

		[Tooltip("AutoStart haptic effect when enabled.")]
		[Rename("AutoStart Playing")]
		public bool autoStart = false;
		[Tooltip("Is currently playing haptic feedback.")]
		protected bool playing = false;

		[Tooltip("Restart the feedback after it ended.")]
		public bool loop = false;

		[Tooltip("Duration in seconds for a feedback loop.")]
		public float duration = 3.0f;

		[Tooltip("Radius in meter the haptic feedback should spread.")]
		[Rename("Range")]
		public float distance = 4.0f;

		[Tooltip("Animation Curve for feedback intensity over time [Normalized, volume/s].")]
		[Rename("Volume over Time")]
		public AnimationCurve forceOverTime = AnimationCurve.EaseInOut(0, 1, 1, 0);

		[Tooltip("Animation Curve for feedback intensity distance [Normalized, volume/m].")]
		public AnimationCurve forceOverDistance = AnimationCurve.Linear(0, 1, 1, 0);

		private float timeStart;

		// Use this for initialization
		protected virtual void Start()
		{
			if (hapticListener == null)
			{
				hapticListener = FindObjectOfType<CVirtHapticListener>();

				if (hapticListener == null)
				{
					CLogger.LogWarning(string.Format("No CVirtHapticListener-Object set in CVirtHapticEmitter@'{0}'.", gameObject.name));
					return;
				}
			}

			if (autoStart)
				Play();
		}

		public void Play()
		{
			timeStart = Time.time;

			if (playing)
				return;

			playing = true;
			if (hapticListener != null)
				hapticListener.AddEmitter(this);
		}

		public void Stop()
		{
			if (!playing)
				return;

			playing = false;
			if (hapticListener != null)
				hapticListener.RemoveEmitter(this);
		}

		public virtual float EvaluateForce(Vector3 listenerPosition)
		{
			if (!isActiveAndEnabled || !playing)
				return 0f;

			float timeDelta = Time.time - timeStart;
			float timeInLoop = Mathf.Repeat(timeDelta, duration);

			//Evaluate time
			float forceTimeP = forceOverTime.Evaluate(timeDelta / duration);

			//Evaluate distance
			float dist = Vector3.Distance(transform.position, listenerPosition) / distance;
			float forceDistP = forceOverDistance.Evaluate(Mathf.Clamp01(dist));

			return forceTimeP * forceDistP;
		}

		public float GetDuration()
		{
			return duration;
		}

		public float GetRange()
		{
			return distance;
		}

		private void Update()
		{
			if (!playing)
				return;

			if (!loop)
			{
				float timeDelta = Time.time - timeStart;

				if (timeDelta > GetDuration())
				{
					Stop();
				}
			}
		}

		// Use this for deinitialization
		void OnDestroy()
		{
			Stop();
		}
	}

}
