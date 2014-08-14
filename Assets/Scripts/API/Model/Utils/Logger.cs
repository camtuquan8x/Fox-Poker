using System;
using UnityEngine;

namespace Esimo
{
    public class Logger
    {
        public static void Info(object message, params object[] list)
        {
            Log(message.ToString(), list);
        }

        public static void Log(string message, params object[] list)
        {
            Debug.Log(string.Format(message, list));
        }

        public static void LogError(string message, params object[] list)
        {
            Debug.LogError(string.Format(message, list));
        }

        public static void LogWarning(string message, params object[] list)
        {
            Debug.LogWarning(string.Format(message, list));
        }

        public static void LogException(System.Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
