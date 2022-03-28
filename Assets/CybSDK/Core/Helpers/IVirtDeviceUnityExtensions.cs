using UnityEngine;

namespace CybSDK
{
	/// <summary>
	/// 
	/// </summary>
    public static class IVirtDeviceUnityExtensions
	{
		/// <summary>
		/// Returns the movement direction as a speed scaled vector relative to the current player orientation.
		/// </summary>
		public static Vector3 GetMovementVector(this IVirtDevice device)
		{
			return device.GetMovementDirectionVector() * device.GetMovementSpeed();
		}

		/// <summary>
		/// Returns the movement direction as vector relative to the current player orientation.
		/// </summary>
		/// <remarks>The origin is the GetPlayerOrientation method and increases clockwise.</remarks>
		public static Vector3 GetMovementDirectionVector(this IVirtDevice device)
	    {
		    float movementDirection = device.GetMovementDirection() * Mathf.PI;
			return new Vector3(
				Mathf.Sin(movementDirection),
				0.0f,
				Mathf.Cos(movementDirection)).normalized;
		}

		/// <summary>
		/// Returns the orientation of the player as vector.
		/// </summary>
		/// <remarks>The origin is set by the ResetPlayerOrientation method and increases clockwise.</remarks>
		public static Vector3 GetPlayerOrientationVector(this IVirtDevice device)
	    {
		    float playerOrientation = device.GetPlayerOrientation() * 2.0f * Mathf.PI;
		    return new Vector3(
			    Mathf.Sin(playerOrientation),
			    0.0f,
			    Mathf.Cos(playerOrientation)).normalized;
		}

		/// <summary>
		/// Returns the orientation of the player as quaternion.
		/// </summary>
		/// <remarks>The origin is set by the ResetPlayerOrientation method and increases clockwise.</remarks>
		public static Quaternion GetPlayerOrientationQuaternion(this IVirtDevice device)
		{
			float playerOrientation = device.GetPlayerOrientation() * 360.0f;
			return Quaternion.Euler(0.0f, playerOrientation, 0.0f);
		}
	}
}
