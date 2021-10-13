//#define DLL
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;

namespace Libraries.system
{
    public class Console : BaseLibrary
    {

        public static void Write(string text)
        {
#if !DLL
            ScriptManager.AddDelegateToStack(() => { ScreenManager.AddToScreen(text); }, sync);
#else
            System.Console.WriteLine("write: " + text);
#endif
        }
        public static void WriteLine(string text)
        {
            Write(text + "\n");
        }

        public static void Debug(params object[] obj)
        {
#if !DLL
            UnityEngine.Debug.Log(obj.ToArrayString());
#else
            System.Console.WriteLine("debug: " + text);

#endif
        }
        public static void Line([System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            UnityEngine.Debug.Log(line);
        }

    }
    public class AsyncConsole : Console
    {
        public static bool sync => no;
    }
}