using System;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public static class Error
    {
        public static void LogException(string message)
        {
            Exception e = new Exception(message);
            Debug.LogException(e);
            Debug.Break();
        }
    }
}
