using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Libraries.system
{
    public class Input : BaseLibrary
    {
        public static List<KeyCode> WaitForAny()
        {
            return (List<KeyCode>)CodeRunner.AddFunctionToStack((ref bool done, ref object returnValue) =>
            {
                List<KeyCode> keyCodes = KeyboardInputHelper.GetCurrentKeyss();
                if (keyCodes.Count > 0)
                {
                    returnValue = keyCodes;
                    done = true;
                }
                else
                {
                    done = false;
                }
            }, sync);
        }
        public static void WaitForKey(string key)
        {
            CodeRunner.AddFunctionToStack((ref bool done, ref object returnValue) =>
           {
               done = UnityEngine.Input.GetKey(key);
           }, sync);
        }
        //todo change
        public static void WaitForKeyDown(string key)
        {
            CodeRunner.AddFunctionToStack((ref bool done, ref object returnValue) =>
            {
                done = UnityEngine.Input.GetKeyDown(key);
            }, sync);
        }
        public static bool GetKey(string key)
        {
            return CodeRunner.AddFunctionToStack<bool>((ref bool done, ref bool returnValue) =>
            {
                bool isPressed = UnityEngine.Input.GetKey(key);
                returnValue = isPressed;
                done = true;
            }, sync);
        }
        //todo change

        public static bool GetKeyDown(string key)
        {
            return CodeRunner.AddFunctionToStack((ref bool done, ref bool returnValue) =>
            {
                bool isPressed = UnityEngine.Input.GetKeyDown(key);
                returnValue = isPressed;
                done = true;
            }, sync);
        }

    }

}
