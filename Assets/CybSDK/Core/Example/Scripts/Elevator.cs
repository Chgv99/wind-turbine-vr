using UnityEngine;
using System.Collections.Generic;
using CybSDK;

public class Elevator : MonoBehaviour
{
	[Tooltip("Rise of the elevator in unity meters")]
	public float verticalTravel = 5f;
	public float movementSpeed = 3f;

	[Tooltip("Delay in seconds before elevator moves again")]
	public float startDelay = 5f;
	private float delay = 0;

	[Tooltip("List of emitters to activate when moving")]
	public List<CVirtHapticEmitter> emitters;

	public bool isActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			if (_isActive == value)
				return;

			_isActive = value;

			foreach (CVirtHapticEmitter emitter in emitters)
			{
				if (emitter.autoStart)
					continue;

				if (isActive)
					emitter.Play();
				else
					emitter.Stop();
			}
		}
	}
	private bool _isActive = false;
	
	private bool goDown = false;
	private float startHeight;
	
	// Start is called just before any of the Update methods is called the first time
	private void Start()
	{
		startHeight = transform.position.y;
		delay = startDelay;
	}

	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled
	void FixedUpdate()
	{
		delay -= Time.deltaTime;

		if (delay > 0f)
			return;

		if (!isActive)
			isActive = true;

		float direction = (goDown ? -1f : 1f);
		Vector3 position = transform.position;
		position.y += direction * movementSpeed * Time.deltaTime;

		//reached top --> stop in place and deactivate
		if (position.y >= startHeight + verticalTravel)
		{
			position.y = startHeight + verticalTravel;
			goDown = true;
			delay = startDelay;
			isActive = false;
		}

		//reached bottom --> stop in place and deactivate
		if (position.y <= startHeight)
		{
			position.y = startHeight;
			goDown = false;
			delay = startDelay;
			isActive = false;
		}

		transform.position = position;
	}

	// This function is called when the MonoBehaviour will be destroyed
	private void OnDestroy()
	{
		isActive = false;
	}

	// This function is called when the behaviour becomes disabled or inactive
	private void OnDisable()
	{
		isActive = false;
	}



}
