//#define DLL

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Console = Libraries.system.output.Console;

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

            public void SetAt(Vector2Int point, T value)
            {
                SetAt(point.x, point.y, value);
            }

            public void Fill(int x, int y, int width, int height, T value)
            {
                if (!IsBoxInRange(x, y, width - 1, height - 1) && ignoreSomeErrors)
                {
                    return;
                }

                for (int iterY = 0; iterY < height; iterY++)
                {
                    for (int iterX = 0; iterX < width; iterX++)
                    {
                        SetAt(x + iterX, y + iterY, value);
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

            public T GetAt(Vector2Int point)
            {
                return GetAt(point.x, point.y);
            }

            public void FillAll(T value)
            {
                Fill(0, 0, width, height, value);
            }

            public byte[] ToData(int sizeOfTInBits, Converter<T, byte[]> converter = null)
            {
                byte[] bytes = new byte[sizeof(Int32) + sizeof(Int32) +
                                        mathematics.Maths.Ceil(1f * (sizeOfTInBits * array.Length) / 8)];
                int pointer = 0;
                bytes.SetByteValue(width.ToBytes(), pointer);
                pointer += (sizeof(Int32));
                bytes.SetByteValue(height.ToBytes(), pointer);
                pointer += (sizeof(Int32));
                //todo 90 better array check
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
                int unitSize = Maths.Ceil(1f * sizeOfTInBits / 8);
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

            public bool IsPointInRange(Vector2Int v2)
            {
                return IsPointInRange(v2.x, v2.y);
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

            //todo 2 think if you want to rename draw OR maybe move this to extension class?

            public void DrawLine(int startX, int startY, int endX, int endY, T value)
            {
                float x;
                float y;
                float dx, dy, step;
                int i;

                dx = (endX - startX);
                dy = (endY - startY);
                if (Maths.Abs(dx) >= Maths.Abs(dy))
                    step = Maths.Abs(dx);
                else
                    step = Maths.Abs(dy);
                dx = dx / step;
                dy = dy / step;
                x = startX;
                y = startY;
                i = 1;
                while (i <= step)
                {
                    SetAt(Maths.Round(x), Maths.Round(y), value);
                    x = x + dx;
                    y = y + dy;
                    i = i + 1;
                }

                if (step == 0 && startX == endX && startY == endY)
                {
                    SetAt(startX, startY, value);
                }
            }

            public void DrawLine(Vector2Int start, Vector2Int end, T value)
            {
                DrawLine(start.x, start.y, end.x, end.y, value);
            }

            public virtual void DrawRectangle(int startX, int startY, int endX, int endY, T value, bool fill = false)
            {
                DrawLine(startX, startY, endX, startY, value);
                DrawLine(startX, startY, startX, endY, value);
                DrawLine(endX, startY, endX, endY + 1, value);
                DrawLine(startX, endY, endX, endY, value);
            }

            public void DrawRectangle(Vector2Int start, Vector2Int end, T value, bool fill = false)
            {
                DrawRectangle(start.x, start.y, end.x, end.y, value, fill);
            }

            public void DrawEllipseInRect(Vector2Int start, Vector2Int end, T value, bool fill = false)
            {
                Console.Debug($"{start} {end}");
                DrawEllipseInRect(start.x, start.y, end.x, end.y, value, fill);
            }

            public void DrawEllipseInRect(
                int startX, int startY, int endX, int endY, T value, bool fill = false)
            {
                //todo 3 fix this \/
                Console.Debug($"{startX} {startY} {endX} {endY}");
                Console.Debug(
                    $"{(startX + endX) / 2} {(startY + endY) / 2} {(endX - startX) / 2} {(endY - startY) / 2}");

                int minX = Maths.Min(startX, endX);
                int maxX = Maths.Max(startX, endX);
                int minY = Maths.Min(startY, endY);
                int maxY = Maths.Max(startY, endY);

                DrawEllipseFromCenter((minX + maxX) / 2, (minY + maxY) / 2, (maxX - minX) / 2,
                    (maxY - minY) / 2, value, fill);
            }

            public void DrawEllipseFromCenter(Vector2Int center, Vector2Int radius, T value, bool fill = false)
            {
                DrawEllipseFromCenter(center.x, center.y, radius.x, radius.y, value, fill);
            }

            public void DrawEllipseFromCenter(
                int centerX, int centerY, int radiusX, int radiusY, T value, bool fill = false)
            {
                int dx, dy, d1, d2, x, y;
                x = 0;
                y = radiusY;

                d1 = Maths.Round((radiusY * radiusY) - (radiusX * radiusX * radiusY) +
                                (0.25f * radiusX * radiusX));
                dx = 2 * radiusY * radiusY * x;
                dy = 2 * radiusX * radiusX * y;

                while (dx < dy)
                {
                    SetAt(x + centerX, y + centerY, value);
                    SetAt(-x + centerX, y + centerY, value);
                    SetAt(x + centerX, -y + centerY, value);
                    SetAt(-x + centerX, -y + centerY, value);

                    if (d1 < 0)
                    {
                        x++;
                        dx = dx + (2 * radiusY * radiusY);
                        d1 = d1 + dx + (radiusY * radiusY);
                    }
                    else
                    {
                        x++;
                        y--;
                        dx = dx + (2 * radiusY * radiusY);
                        dy = dy - (2 * radiusX * radiusX);
                        d1 = d1 + dx - dy + (radiusY * radiusY);
                    }
                }

                d2 = Maths.Round(((radiusY * radiusY) * ((x + 0.5f) * (x + 0.5f)))
                                + ((radiusX * radiusX) * ((y - 1) * (y - 1)))
                                - (radiusX * radiusX * radiusY * radiusY));

                while (y >= 0)
                {
                    SetAt(x + centerX, y + centerY, value);
                    SetAt(-x + centerX, y + centerY, value);
                    SetAt(x + centerX, -y + centerY, value);
                    SetAt(-x + centerX, -y + centerY, value);
                    if (d2 > 0)
                    {
                        y--;
                        dy = dy - (2 * radiusX * radiusX);
                        d2 = d2 + (radiusX * radiusX) - dy;
                    }
                    else
                    {
                        y--;
                        x++;
                        dx = dx + (2 * radiusY * radiusY);
                        dy = dy - (2 * radiusX * radiusX);
                        d2 = d2 + dx - dy + (radiusX * radiusX);
                    }
                }
            }
        }


    }
}