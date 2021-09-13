using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace libraries.system.graphics
{
    public static class screen
    {
        public static readonly int screenWidth = 4;
        public static readonly int screenHeight = 4;
        /* public static void SetPixel(int x, int y, graphics.color color)
         {
 #if !DLL
             CodeTask.RunMainFunction(() =>
             {
                 ScreenManager.instance.SetPixel(x, y, color);
             });
 #else
             System.Console.WriteLine("making pixel at: " + x+" "+y+"with color:"+color.ToString());

 #endif
         }
         public static void SetScreenBuffer()
         {
 #if !DLL
             CodeTask.RunMainFunction(ScreenManager.instance.SetScreenBuffer);
 #else
             System.Console.WriteLine("making pixel at: " + x+" "+y+"with color:"+color.ToString());

 #endif
         }*/
        public static screen_buffer MakeScreenBuffer()
        {
            return new screen_buffer(screenWidth, screenHeight);
        }
        public static void SetScreenBuffer(screen_buffer screen_buffer)
        {

            CodeRunner.AddFunctionToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screen_buffer);
            }, false);

        }
    }
}