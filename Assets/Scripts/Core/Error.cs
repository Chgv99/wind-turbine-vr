using System;
using UnityEngine;

namespace WindTurbineVR.Core
{
    public static class Error
    {
        public static void LogException(string message) => LogException(message, true);

        public static void LogExceptionNoBreak(string message) => LogException(message, false);

        public static void LogException(string message, bool doBreak)
        {
            Exception e = new Exception(message);
            Debug.LogException(e);
            if (doBreak) Debug.Break();
        }
    }
}
