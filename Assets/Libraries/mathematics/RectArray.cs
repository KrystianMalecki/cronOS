//#define DLL

using NaughtyAttributes;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            [SerializeField] [AllowNesting, ReadOnly]
            public int width;

            [SerializeField] [AllowNesting, ReadOnly]
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

            public RectArray(int width, int height, T[] array)
            {
                this.width = width;
                this.height = height;
                this.array = new T[width * height];
                Array.Copy(array, 0, this.array, 0, array.Length);
            }

            bool ignoreSomeErrors = true; //todo 9 remove, flag system?

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
            }

            public byte[] ToData(int sizeOfTInBits, Converter<T, byte[]> converter = null)
            {
                byte[] bytes = new byte[sizeof(Int32) + sizeof(Int32) +
                                        mathematics.Math.Ceil(1f * (sizeOfTInBits * array.Length) / 8)];
                int pointer = 0;
                bytes.SetByteValue(width.ToBytes(), pointer);
                pointer += (sizeof(Int32));
                bytes.SetByteValue(height.ToBytes(), pointer);
                pointer += (sizeof(Int32));
                //todo 0 better array check
                if (array == null)
                {
                    return bytes;
                }

                if (sizeOfTInBits == 1)
                {
                    if (array is not bool[] && array is byte[] bytess)
                    {
                        //convert to bool[]
                        byte[] output = Array.ConvertAll<byte, bool>(bytess, x => x != 0).ConvertAllBoolsToBytes();
                        Array.Copy(output, 0, bytes, pointer, output.Length);
                    }
                    else
                    {
                        byte[] output = (array as bool[]).ConvertAllBoolsToBytes();
                        Array.Copy(output, 0, bytes, pointer, output.Length);
                    }
                }
                else if (sizeOfTInBits <= 2)
                {
                    byte[] output = (array as byte[]).ConvertAllDibitsToBytes();
                    Array.Copy(output, 0, bytes, pointer, output.Length);
                }
                else if (sizeOfTInBits <= 4)
                {
                    byte[] output = (array as byte[]).ConvertAllNibblesToBytes();
                    Array.Copy(output, 0, bytes, pointer, output.Length);
                }
                else if (converter == null)
                {
                    Array.Copy((array as byte[]), 0, bytes, pointer, array.Length);
                }
                else
                {
                    foreach (var x1 in array)
                    {
                        byte[] output = converter(x1);
                        Array.Copy(output, 0, bytes, pointer, output.Length);
                        pointer += output.Length;
                    }
                }

                return bytes;
            }

            public static int Round(float f)
            {
                return Mathf.RoundToInt(f);
            }

            public static RectArray<T> FromData(byte[] data, int sizeOfTInBits, Func<byte[], T[]> converter = null)
            {
                int unitSize = Math.Ceil(1f * sizeOfTInBits / 8);
                int pointer = 0;
                int width = BitConverter.ToInt32(data, pointer);
                pointer += sizeof(Int32);
                int height = BitConverter.ToInt32(data, pointer);
                pointer += sizeof(Int32);
                RectArray<T> structure = new RectArray<T>(width, height);
                data = data.Skip(pointer).ToArray();
                List<T> list = new List<T>();
                if (sizeOfTInBits == 1)
                {
                    byte[] output = Array.ConvertAll(data.ConvertBytesToBools(), x => (byte)(x ? 1 : 0));
                    Array.Copy(output, 0, structure.array, 0, structure.array.Length);
                }
                else if (sizeOfTInBits <= 2)
                {
                    byte[] output = data.ConvertBytesToDibits();
                    Array.Copy(output, 0, structure.array, 0, structure.array.Length);
                }
                else if (sizeOfTInBits <= 4)
                {
                    byte[] output = data.ConvertBytesToNibbles();
                    Array.Copy(output, 0, structure.array, 0, structure.array.Length);
                }
                else if (converter == null)
                {
                    Array.Copy(data, 0, structure.array, 0, data.Length);
                }
                else
                {
                    for (int i = 0; i < data.Length; i += unitSize)
                    {
                        Debug.Log($"" +
                                  $"{i}  {data.Length} {unitSize}  ");
                        list.AddRange(converter(data.Skip(i).Take(unitSize).ToArray()));
                    }

                    structure.array = list.ToArray();
                }


                return structure;
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

            public RectArray<S> Convert<S>(Converter<T, S> converter)
            {
                RectArray<S> newArray = new RectArray<S>(width, height);
                newArray.array = Array.ConvertAll<T, S>(array, converter);
                /*  for (int currentY = 0; currentY < height; currentY++)
                  {
                      for (int currentX = 0; currentX < width; currentX++)
                      {
                          newArray.SetAt(currentX, currentY, converter.Invoke(this.GetAt(currentX, currentY)));
                      }
                  }*/

                return newArray;
            }
        }
    }
}