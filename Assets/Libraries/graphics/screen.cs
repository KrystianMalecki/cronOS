using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Libraries.system.graphics
{
    public static class Screen
    {
        public static readonly int screenWidth = 4;
        public static readonly int screenHeight = 4;
        public static ScreenBuffer MakeScreenBuffer()
        {
            CodeRunner.AddFunctionToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screenBuffer);
            });
            return new ScreenBuffer(screenWidth, screenHeight);
        }
        public static void SetScreenBuffer(ScreenBuffer screenBuffer)
        {

            CodeRunner.AddFunctionToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screenBuffer);
            });

        }
    }
}