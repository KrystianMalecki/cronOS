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
        public screen_buffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            texture = new color[height, width];
        }

        public void SetPixel(int x, int y, graphics.color color)
        {
            texture[y, x] = color;
        }
        public graphics.color GetPixel(int x, int y)
        {
            return texture[y, x];
        }
    }
}