using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InternalLogger
{
    [Flags]
    public enum LogFlags
    {
        None = 0,
        SystemError = 1,
        SystemWarning = 2,
        Info = 4,
        DebugInfo = 8,
        All = SystemError | SystemWarning | Info | DebugInfo

    }
    public static class FlagLogger
    {
       // public static LogFlags enabledFlags = LogFlags.DebugInfo | LogFlags.SystemWarning|LogFlags.SystemError;
          public static LogFlags enabledFlags = LogFlags.All;
        //   public static LogFlags enabledFlags = LogFlags.None;


        public static void Log(LogFlags flag, params object[] objs)
        {
            if ((enabledFlags & flag) == flag)
            {
                Debug.Log(objs.ToArrayString());
            }
        }
        public static void LogWarning(LogFlags flag, params object[] objs)
        {
            if ((enabledFlags & flag) == flag)
            {
                Debug.LogWarning(objs.ToArrayString());
            }
        }
        public static void LogError(LogFlags flag, params object[] objs)
        {
            if ((enabledFlags & flag) == flag)
            {
                Debug.LogError(objs.ToArrayString());
            }
        }
        private static string ToArrayString<T>(this IEnumerable<T> ie)
        {

            return string.Join(",", ie);
        }
        private static string ToArrayString<T>(this T[] ie)
        {

            return string.Join(",", ie);
        }
    }
}