
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
            internal UnityEngine.Color32 GetColor32()
            {
                return color;
            }

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
            public Color32(UnityEngine.Color32 color32)
            {
                color = color32;
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
                return String.Format("RGBA({0}, {1}, {2}, {3})", color.r.ToString(format, formatProvider), color.g.ToString(format, formatProvider), color.b.ToString(format, formatProvider), color.a.ToString(format, formatProvider));
            }

        }
    }
    namespace system_color
    {
        using color32;
        public static class ColorConstants
        {
            /* public static Color32 ToColor32(this SystemColor systemColor) => systemColor switch
             {
                 0 => Black,
                 1 => Blue,
                 2 => Green,
                 3 => Cyan,
                 4 => Red,
                 5 => Magenta,
                 6 => Brown,
                 7 => LightGray,
                 8 => DarkGray,
                 9 => LightBlue,
                 10 => LightGreen,
                 11 => LightCyan,
                 12 => LightRed,
                 13 => LightMagenta,
                 14 => Yellow,
                 15 => White,
                 _ => throw new NotImplementedException(),
             };*/

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
            /*  public static byte black = 0;
              public static byte blue = 1;
              public static byte green = 2;
              public static byte cyan = 3;
              public static byte red = 4;
              public static byte magenta = 5;
              public static byte brown = 6;
              public static byte light_gray = 7;
              public static byte dark_gray = 8;
              public static byte light_blue = 9;
              public static byte light_green = 10;
              public static byte light_cyan = 11;
              public static byte light_red = 12;
              public static byte light_magenta = 13;
              public static byte yellow = 14;
              public static byte white = 15;*/
        }

        public struct SystemColor
        {
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
            public Color32 ToColor32() => value switch
            {
                0 => ColorConstants.Black32,
                1 => ColorConstants.Blue32,
                2 => ColorConstants.Green32,
                3 => ColorConstants.Cyan32,
                4 => ColorConstants.Red32,
                5 => ColorConstants.Magenta32,
                6 => ColorConstants.Brown32,
                7 => ColorConstants.LightGray32,
                8 => ColorConstants.DarkGray32,
                9 => ColorConstants.LightBlue32,
                10 => ColorConstants.LightGreen32,
                11 => ColorConstants.LightCyan32,
                12 => ColorConstants.LightRed32,
                13 => ColorConstants.LightMagenta32,
                14 => ColorConstants.Yellow32,
                15 => ColorConstants.White32,
                _ => ColorConstants.LightMagenta32,
            };
        }
    }
}




