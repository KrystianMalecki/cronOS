using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace c_System
{
    public class console : LibraryBase
    {
        public static void print(string text)
        {
            CodeTask.RunMainFunction(() => { ScreenManager.instance.screen.text += text; });
        }

        public static void wait(int time)
        {
            Task.Delay(time).Wait();
        }

        public static
            void debug(string text)
        {
            Debug.Log("text" + text);
        }
    }
}