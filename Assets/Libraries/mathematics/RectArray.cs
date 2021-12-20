//#define DLL

using NaughtyAttributes;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Libraries.system
{
    namespace mathematics
    {
        [Serializable]
        public class RectArray<T>
        {
            [SerializeField] public T[] array;

            [SerializeField]
            [AllowNesting, ReadOnly]
            public int width;

            [SerializeField]
            [AllowNesting, ReadOnly]
            public int height;

            public int size
            {
                get { return width * height; }
            }

            public T[] GetArray()
            {
                return array;
            }

            public void SetArray(T[] array)
            {
                this.array = array;
            }

            public RectArray(int width = 0, int height = 0)
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

            bool ignoreSomeErrors = true;//todo 9 remove
            public void SetAt(int x, int y, T value)
            {
                if (!IsPointInRange(x, y) && ignoreSomeErrors)
                {
                    return;
                }

                array[y * width + x] = value;
            }

            public void Fill(int x, int y, int width, int height, T value)
            {
                if (!IsBoxInRange(x, y, width, height) && ignoreSomeErrors)
                {
                    return;
                }

                for (int iterY = y; iterY < height; iterY++)
                {
                    for (int iterX = x; iterX < width; iterX++)
                    {
                        SetAt(iterX, iterY, value);
                    }
                }
            }

            public T GetAt(int x, int y)
            {
                if (!IsPointInRange(x, y) && ignoreSomeErrors)
                {
                    return default(T);
                }

                return array[y * width + x];
            }

            public void FillAll(T value)
            {
                Fill(0, 0, width, height, value);
                //  Func<int, int> f = null;
                // Func<int,out int,out int> ff = null;
            }

            //todo 0 think about making another function that will use byte instead of byte[] cuz new byte[]{}; impossible, can't make something like null byte. Ok `out`? delegates? Make it return byte and bool isNull
            protected byte[] ToData(float sizeOfT, Func<T, byte[]> converter)
            {
                int fullSize = 8 - (array.Length % 8) + array.Length;
                byte[] __bytes = new byte[sizeof(Int32) + sizeof(Int32) + Round(sizeOfT * fullSize)];
                int __counter = 0;
                __bytes.SetByteValue(width.ToBytes(), __counter);
                __counter += (sizeof(Int32));
                __bytes.SetByteValue(height.ToBytes(), __counter);
                __counter += (sizeof(Int32));
                for (int i = 0; i < fullSize; i++)
                {
                    T val = default(T);
                    if (array.Length > i)
                    {
                        val = array[i];
                    }

                    byte[] output = converter.Invoke(val);

                    for (int j = 0; j < output.Length; j++)
                    {


                        // Debug.Log($"setting at {__counter} value {output[j]}. J is {j}");
                        __bytes[__counter] = output[j];
                        __counter += 1;
                    }
                }

                return __bytes;
            }

            public static int Round(float f)
            {
                return Mathf.RoundToInt(f);
            }

            protected static RectArray<T> FromData(byte[] data, float sizeOfT, Func<byte[], T[]> converter)
            {
                float __counter = 0;
                int minSizeOfT = Round(sizeOfT);
                minSizeOfT += minSizeOfT < 1 ? 1 : 0;
                int width = BitConverter.ToInt32(data, Round(__counter));
                __counter += (sizeof(Int32));
                int height = BitConverter.ToInt32(data, Round(__counter));
                __counter += (sizeof(Int32));
                RectArray<T> __structure = new RectArray<T>(width, height);
                byte[] buffers = new byte[minSizeOfT];
                for (int k = 0; __counter < data.Length; __counter += minSizeOfT)
                {
                    //  Debug.Log($"iter {__counter} compare to {data.Length}");
                    Array.Copy(data, Round(__counter), buffers, 0, minSizeOfT);
                    T[] t = converter.Invoke(buffers);
                    for (int j = 0; j < t.Length; j++)
                    {
                        if (j > k || k >= __structure.array.Length)
                        {
                            break;
                        }

                        // Debug.Log($"setting at {__counter} value {output[j]}. J is {j}");
                        __structure.array[k] = t[j];
                        k++;
                    }
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
                return x >= 0 && x < width && y >= 0 && y < height;
            }

            public bool IsBoxInRange(int x, int y, int width, int height)
            {
                return x >= 0 && x + width <= this.width && y >= 0 && y + height <= this.height;
            }

            public RectArray<T> GetRect(int x, int y, int width, int height)
            {
                RectArray<T> newArray = new RectArray<T>(width, height);
                for (int currentY = 0; currentY < height; currentY++)
                {
                    for (int currentX = 0; currentX < width; currentX++)
                    {
                        newArray.SetAt(currentX, currentY, this.GetAt(x + currentX, y + currentY));
                    }
                }

                return newArray;
            }

            public RectArray<S> Convert<S>(Func<T, S> converter)
            {
                RectArray<S> newArray = new RectArray<S>(width, height);
                for (int currentY = 0; currentY < height; currentY++)
                {
                    for (int currentX = 0; currentX < width; currentX++)
                    {
                        newArray.SetAt(currentX, currentY, converter.Invoke(this.GetAt(currentX, currentY)));
                    }
                }

                return newArray;
            }
        }
    }
}