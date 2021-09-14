//#define DLL
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEditor;

namespace Libraries.system
{
    public static class Console
    {
        public static void Write(string text)
        {
#if !DLL
            CodeRunner.AddFunctionToStack(() => { ScreenManager.AddToScreen(text); }, false);
#else
            System.Console.WriteLine("write: " + text);
#endif
        }
        public static void WriteLine(string text)
        {
            Write(text + "\n");
        }
        public static void Debug(string text)
        {
#if !DLL
            UnityEngine.Debug.Log(text);
#else
            System.Console.WriteLine("debug: " + text);

#endif
        }


    }
}