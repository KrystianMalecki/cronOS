//#define DLL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Libraries.system
{
    public class RectArray<T>
    {
        protected T[] array;
        public int width { get; }
        public int height { get; }

        public T[] GetArray()
        {
            return array;
        }
        public RectArray(int width, int height)
        {
            array = new T[width * height];
            this.width = width;
            this.height = height;
        }
        public void SetAt(int x, int y, T value)
        {
            array[y * width + x] = value;
        }
        public void Fill(int x, int y, int width, int height, T value)
        {
            for (int iterY = y; iterY < height; iterY++)
            {
                for (int iterX = x; iterX < width; iterX++)
                {
                    //todo add check
                    SetAt(iterX, iterY, value);
                }
            }
        }
        public T GetAt(int x, int y)
        {
            //todo add check
            return array[y * width + x];
        }
        public void FillAll(T value)
        {
            Fill(0, 0, width, height, value);
        }
    }

}