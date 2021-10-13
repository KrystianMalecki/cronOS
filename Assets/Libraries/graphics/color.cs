
using System;
using System.Globalization;
using UnityEngine;

namespace Libraries.system.graphics
{
    namespace color32
    {

        public struct Color32
        {
            private UnityEngine.Color32 color;

            public static readonly short sizeOf = sizeof(int);

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
            //todo 8 move Unity Dependant stuff
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
                get
                {
                    return this[index];
                }

                set
                {
                    this[index] = value;
                }
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
                return string.Format("RGBA({0}, {1}, {2}, {3})", color.r.ToString(format, formatProvider), color.g.ToString(format, formatProvider), color.b.ToString(format, formatProvider), color.a.ToString(format, formatProvider));
            }

            public int GetDistance(Color32 color)
            {
                int redDifference;
                int greenDifference;
                int blueDifference;

                redDifference = r - color.r;
                greenDifference = g - color.g;
                blueDifference = b - color.b;

                return redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference;
            }
        }
    }
    namespace system_color
    {
        using color32;
        public static class ColorConstants
        {
           
            public static Color32 FindNearest(Color32[] colors, Color32 input)
            {
                int id = FindNearestID(colors, input);
                if (id == -1 && ProcessorManager.instance.ignoreSomeErrors)
                {
                    return default(Color32);//todo future add error
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
            //todo 1 move ToCronosColor
            internal static Color32 ToCronosColor(this UnityEngine.Color32 color)
            {
                return new Color32(color.r, color.g, color.b, color.a);
            }

            public static readonly Color32 Black32 = new Color32(0, 0, 0);
            public static readonly Color32 Blue32 = new Color32(0, 0, 170);
            public static readonly Color32 Green32 = new Color32(0, 170, 0);
            public static readonly Color32 Cyan32 = new Color32(0, 170, 170);
            public static readonly Color32 Red32 = new Color32(170, 0, 0);
            public static readonly Color32 Magenta32 = new Color32(170, 0, 170);
            public static readonly Color32 Brown32 = new Color32(170, 85, 0);
            public static readonly Color32 LightGray32 = new Color32(170, 170, 170);
            public static readonly Color32 DarkGray32 = new Color32(85, 85, 85);
            public static readonly Color32 LightBlue32 = new Color32(85, 85, 255);
            public static readonly Color32 LightGreen32 = new Color32(85, 255, 85);
            public static readonly Color32 LightCyan32 = new Color32(85, 255, 255);
            public static readonly Color32 LightRed32 = new Color32(255, 85, 85);
            public static readonly Color32 LightMagenta32 = new Color32(255, 85, 255);
            public static readonly Color32 Yellow32 = new Color32(255, 255, 85);
            public static readonly Color32 White32 = new Color32(255, 255, 255);

            public static Color32[] SystemColors = new Color32[] { Black32, Blue32, Green32, Cyan32, Red32, Magenta32, Brown32, LightGray32, DarkGray32, LightBlue32, LightGreen32, LightCyan32, LightRed32, LightMagenta32, Yellow32, White32 };

           
        }
        [Serializable]
        public struct SystemColor
        {
            [SerializeField]
            private byte _value;
            public byte value
            {
                get
                {
                    // FlagLogger.Log(_value);
                    return _value;
                }
                set
                {
                    if (value < 0)
                    {
                        value = 15;
                    }
                    if (value > 15)
                    {
                        value = 0;
                    }
                    this._value = value;

                }
            }

            public SystemColor(byte value)
            {
                this._value = value;
            }
            public static implicit operator SystemColor(byte val)
            {
                return new SystemColor(val);
            }
            public static implicit operator byte(SystemColor val)
            {
                return val.value;
            }
            public static SystemColor operator ++(SystemColor sc)
            {
                sc.value += 1;
                return sc;
            }
            public static SystemColor operator --(SystemColor sc)
            {
                sc.value -= 1;
                return sc;
            }
            public static bool operator ==(SystemColor sc, SystemColor sc2)
            {

                return sc.value == sc2.value;
            }
            public static bool operator !=(SystemColor sc, SystemColor sc2)
            {
                return !(sc == sc2);
            }
            public override string ToString()
            {
                switch (value)
                {
                    case 0: return "black";
                    case 1: return "blue";
                    case 2: return "green";
                    case 3: return "cyan";
                    case 4: return "red";
                    case 5: return "magenta";
                    case 6: return "brown";
                    case 7: return "light_gray";
                    case 8: return "dark_gray";
                    case 9: return "light_blue";
                    case 10: return "light_green";
                    case 11: return "light_cyan";
                    case 12: return "light_red";
                    case 13: return "light_magenta";
                    case 14: return "yellow";
                    case 15: return "white";

                }
                return "unknown";
            }

            public static readonly short sizeOf = sizeof(byte);

            public static readonly SystemColor black = new SystemColor(0);
            public static readonly SystemColor blue = new SystemColor(1);
            public static readonly SystemColor green = new SystemColor(2);
            public static readonly SystemColor cyan = new SystemColor(3);
            public static readonly SystemColor red = new SystemColor(4);
            public static readonly SystemColor magenta = new SystemColor(5);
            public static readonly SystemColor brown = new SystemColor(6);
            public static readonly SystemColor light_gray = new SystemColor(7);
            public static readonly SystemColor dark_gray = new SystemColor(8);
            public static readonly SystemColor light_blue = new SystemColor(9);
            public static readonly SystemColor light_green = new SystemColor(10);
            public static readonly SystemColor light_cyan = new SystemColor(11);
            public static readonly SystemColor light_red = new SystemColor(12);
            public static readonly SystemColor light_magenta = new SystemColor(13);
            public static readonly SystemColor yellow = new SystemColor(14);
            public static readonly SystemColor white = new SystemColor(15);
            public Color32 ToColor32() => ColorConstants.SystemColors[value];
        }
    }

}




