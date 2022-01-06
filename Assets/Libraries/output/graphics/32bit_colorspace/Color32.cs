using System;
using System.Globalization;
using UnityEngine;

namespace Libraries.system.output.graphics
{
    namespace color32
    {
        public struct Color32
        {
            private UnityEngine.Color32 color;

            public const int sizeOfInBits = sizeof(int) * 8;

            public byte r
            {
                get { return color.r; }
                set { color.r = value; }
            }

            public byte g
            {
                get { return color.g; }
                set { color.g = value; }
            }

            public byte b
            {
                get { return color.b; }
                set { color.b = value; }
            }

            public byte a
            {
                get { return color.a; }
                set { color.a = value; }
            }

            public Color32(int color)
            {
                byte[] byteArray = BitConverter.GetBytes(color);
                this.color = new UnityEngine.Color32(byteArray[0], byteArray[1], byteArray[2], byteArray[3]);
            }

            public Color32(byte[] bytes)
            {
                this.color = new UnityEngine.Color32(bytes[0], bytes[1], bytes[2], bytes[3]);
            }

            public Color32(byte r, byte g, byte b, byte a)
            {
                color = new UnityEngine.Color32(r, g, b, a);
            }

            public Color32(byte r, byte g, byte b)
            {
                color = new UnityEngine.Color32(r, g, b, 255);
            }

            public Color32(UnityEngine.Color32 unityColor)
            {
                color = unityColor;
            }

            //todo-cleanup move Unity Dependant stuff.Move to extension methods?
            public static implicit operator Color32(UnityEngine.Color32 col)
            {
                return new Color32(col);
            }

            public static implicit operator UnityEngine.Color32(Color32 col)
            {
                return col.color;
            }

            public static Color32 Lerp(Color32 a, Color32 b, float t)
            {
                return new Color32(UnityEngine.Color32.Lerp(a.color, b.color, t));
            }

            public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
            {
                return new Color32(UnityEngine.Color32.LerpUnclamped(a.color, b.color, t));
            }

            public byte this[int index]
            {
                get { return this.color[index]; }

                set { this.color[index] = value; }
            }

            public override string ToString()
            {
                return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
            }

            public string ToString(string format)
            {
                return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return string.Format("RGBA({0}, {1}, {2}, {3})", color.r.ToString(format, formatProvider),
                    color.g.ToString(format, formatProvider), color.b.ToString(format, formatProvider),
                    color.a.ToString(format, formatProvider));
            }

            public int GetDistance(Color32 color)
            {
                int redDifference;
                int greenDifference;
                int blueDifference;

                redDifference = r - color.r;
                greenDifference = g - color.g;
                blueDifference = b - color.b;

                return redDifference * redDifference + greenDifference * greenDifference +
                       blueDifference * blueDifference;
            }

            static bool ignoreSomeErrors = true; //todo 9 remove

            public static Color32 FindNearest(Color32[] colors, Color32 input)
            {
                int id = FindNearestID(colors, input);
                if (id == -1 && ignoreSomeErrors)
                {
                    return default(Color32); //todo-future add error
                }

                return colors[id];
            }

            public static int FindNearestID(Color32[] colors, Color32 input)
            {
                //  SystemColors = new Color32[] { Black32, Blue32, Green32, Cyan32, Red32, Magenta32, Brown32, LightGray32, DarkGray32, LightBlue32,
                //  LightGreen32, LightCyan32, LightRed32, LightMagenta32, Yellow32, White32 };

                int nearestID = -1;
                int nearestDistance = int.MaxValue;
                for (int i = 0; i < colors.Length; i++)
                {
                    Color32 currentColor = colors[i];
                    int distance = currentColor.GetDistance(input);
                    if (nearestDistance > distance)
                    {
                        nearestDistance = distance;
                        nearestID = i;
                    }
                    // Debug.Log(currentColor + " " + input + " " + distance);
                }

                return nearestID;
            }
        }
    }
}