/************************************************************************************

Filename    :   CLogger.cs
Content     :   Logger class
Created     :   August 8, 2014
Authors     :   Lukas Pfeifhofer

Copyright   :   Copyright 2014 Cyberith GmbH

Licensed under the AssetStore Free License and the AssetStore Commercial License respectively for the Free and Pro versions of the project.

************************************************************************************/

using UnityEngine;

namespace CybSDK
{

    public class CLogger
    {

        // Logger tag
        private const string LoggerTag = "CybSDK";

        public enum LogLevel : byte
        {
            None = 0,
            Error = 5,
            Warning = 7,
            Debug = 10,
            Trace = 15
        }

        public static LogLevel logLevel = LogLevel.Debug;

        public static void LogError(object message)
        {
            if (LogLevel.Error <= logLevel)
                Debug.LogError(LoggerTag+": "+message);
        }

        public static void LogWarning(object message)
        {
            if (LogLevel.Warning <= logLevel)
                Debug.LogWarning(LoggerTag + ": " + message);
        }

        public static void Log(object message)
        {
            if (LogLevel.Debug <= logLevel)
                Debug.Log(LoggerTag + ": " + message);
        }

        public static void LogTrace(object message)
        {
            if (LogLevel.Trace <= logLevel)
                Debug.Log(LoggerTag + "[TRACE]: " + message);
        }

    }

}