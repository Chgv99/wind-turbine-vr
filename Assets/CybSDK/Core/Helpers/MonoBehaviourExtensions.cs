using System;
using UnityEngine;

namespace CybSDK
{
	public static class MonoBehaviourExtensions
	{
		/// <summary>
		///   <para>Invokes the method method in time seconds.</para>
		/// </summary>
		/// <param name="script"></param>
		/// <param name="method"></param>
		/// <param name="time"></param>
		public static void Invoke(this MonoBehaviour script, Action method, float time)
		{
			script.Invoke(method.Method.Name, time);
		}
	}
}
