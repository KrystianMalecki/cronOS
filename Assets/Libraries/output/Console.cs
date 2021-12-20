//#define DLL

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;
using Libraries.system.shell;

namespace Libraries.system
{
    namespace output
    {
        public class Console : BaseLibrary
        {

            public static void Debug(params object[] obj)
            {
                UnityEngine.Debug.Log(obj.ToFormatedString());
            }

            public static void Debug(object obj = null,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
            {
                UnityEngine.Debug.Log($"{caller} at {lineNumber}. Logs: '{obj}'");
            }

            public static void Line([CallerLineNumber] int line = 0)
            {
                UnityEngine.Debug.Log(line);
            }

        }


    }
}