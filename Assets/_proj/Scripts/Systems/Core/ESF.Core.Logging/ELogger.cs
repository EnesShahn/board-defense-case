using UnityEngine;

namespace ESF.Core.Logging
{
    public static class ELogger
    {
        public static string GetFormattedLog<T>(object message)
            => $"[{typeof(T)}] {message}";

        public static void Log<T>(object message, Object context = null)
        {
            Debug.Log(GetFormattedLog<T>(message), context);
        }
        public static void LogWarning<T>(object message, Object context = null)
        {
            Debug.LogWarning(GetFormattedLog<T>(message), context);
        }
        public static void LogError<T>(object message, Object context = null)
        {
            Debug.LogError(GetFormattedLog<T>(message), context);
        }

        public static void Log(object message, Object context = null)
        {
            Debug.Log(message, context);
        }
        public static void LogWarning(object message, Object context = null)
        {
            Debug.LogWarning(message, context);
        }
        public static void LogError(object message, Object context = null)
        {
            Debug.LogError(message, context);
        }
    }
}