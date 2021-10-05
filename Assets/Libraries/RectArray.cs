//#define DLL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Libraries.system
{
    [Serializable]
    public class RectArray<T>
    {
        [SerializeField]
        public T[] array;
        [SerializeField]
        public readonly int width;
        [SerializeField]
        public readonly int height;
        public int size
        {
            get
            {
                return width * height;
            }
        }

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
        public RectArray(RectArray<T> array)
        {
            width = array.width;
            height = array.height;
            this.array = new T[width * height];
            Array.Copy(array.array, 0, this.array, 0, array.size);
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
                    //todo 6 add check
                    SetAt(iterX, iterY, value);
                }
            }
        }
        public T GetAt(int x, int y)
        {
            //todo 6 add check
            return array[y * width + x];
        }
        public void FillAll(T value)
        {
            Fill(0, 0, width, height, value);
        }
        protected byte[] ToData(short sizeOfT, Func<T, byte[]> converter)
        {
            byte[] __bytes = new byte[sizeof(Int32) + sizeof(Int32) + (sizeOfT * size)];
            int __counter = 0;
            __bytes.SetByteValue(width.ToBytes(), __counter); __counter += (sizeof(Int32));
            __bytes.SetByteValue(height.ToBytes(), __counter); __counter += (sizeof(Int32));
            for (int i = 0; i < array.Length; i++)
            {
                byte[] output = converter.Invoke(array[i]);

                for (int j = 0; j < output.Length; j++, __counter += 1)
                {
                    Debug.Log($"setting at {__counter} value {output[j]}. J is {j}");
                    __bytes[__counter] = output[j];
                }

            }
            return __bytes;
        }

        protected static RectArray<T> FromData(byte[] data, short sizeOfT, Func<byte[], T> converter)
        {
            int __counter = 0;
            int width = BitConverter.ToInt32(data, __counter); __counter += (sizeof(Int32));
            int height = BitConverter.ToInt32(data, __counter); __counter += (sizeof(Int32));
            RectArray<T> __structure = new RectArray<T>(width, height);
            byte[] buffers = new byte[sizeOfT];
            for (int k = 0; __counter < data.Length; __counter += sizeOfT, k++)
            {
                //  Debug.Log($"iter {__counter} compare to {data.Length}");
                Array.Copy(data, __counter, buffers, 0, sizeOfT);
                T t = converter.Invoke(buffers);
                __structure.array[k] = t;
                //   Debug.Log($"copyied from {data.ToArrayString()} at {__counter} to {buffers.ToArrayString()} at {0} with length {sizeOfT} which gave {t}");

            }
            /*  for (int i = 0; __counter < data.Length; __counter += sizeOfT, i++)
              {
                  __structure.array[i] = converter.Invoke(data);
              }*/
            return __structure;
        }
        public bool IsPointInRange(int x, int y)
        {
            return x >= 0 && x <= width && y >= 0 && y <= height;
        }
        public bool IsBoxInRange(int x, int y, int height, int width)
        {
            return x >= 0 && x + width <= this.width && y >= 0 && y + height <= this.height;
        }
    }

}