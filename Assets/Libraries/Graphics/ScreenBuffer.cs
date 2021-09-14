using System;
using System.Collections;
using System.Collections.Generic;

namespace Libraries.system.graphics
{
    public class ScreenBuffer
    {
        public int width;
        public int height;
        public Color[] texture;
        //  public Texture2D buffer;
        public ScreenBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            texture = new Color[width * height];
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

        public void SetPixel(int x, int y, Color color)
        {
            texture[y * width + x] = color;

            /*  CodeTask.RunMainFunction(() =>
              {
                  buffer.SetPixel(x, y, color.ToUnityColor());
              }, false);*/


        }
        public Color GetPixel(int x, int y)
        {
            return texture[y * width + x];
        }
    }
}