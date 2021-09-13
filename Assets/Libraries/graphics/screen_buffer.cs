using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace libraries.system.graphics
{
    public class screen_buffer
    {
        public int width;
        public int height;
        public color[,] texture;
        //  public Texture2D buffer;
        public screen_buffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            texture = new color[width, height];
            /* Debug.Log("before");
             try
             {
                 buffer = (Texture2D)CodeTask.RunMainFunction(() => { return new Texture2D(width, height); });
                 CodeTask.RunMainFunction(() =>
                 {
                     buffer.filterMode = FilterMode.Point;
                 });


             }
             catch (Exception e)
             {
                 Debug.Log(e);
             }
             Debug.Log("before1");

             Debug.Log("after");*/

        }

        public void SetPixel(int x, int y, graphics.color color)
        {
            texture[y, x] = color;

            /*  CodeTask.RunMainFunction(() =>
              {
                  buffer.SetPixel(x, y, color.ToUnityColor());
              }, false);*/


        }
        public graphics.color GetPixel(int x, int y)
        {
            return texture[y, x];
        }
    }
}