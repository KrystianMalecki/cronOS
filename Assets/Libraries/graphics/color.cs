
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
            public static Color32 ToColor32(this SystemColor systemColor) => systemColor switch
            {
                SystemColor.black => Black,
                SystemColor.blue => Blue,
                SystemColor.green => Green,
                SystemColor.cyan => Cyan,
                SystemColor.red => Red,
                SystemColor.magenta => Magenta,
                SystemColor.brown => Brown,
                SystemColor.light_gray => LightGray,
                SystemColor.dark_gray => DarkGray,
                SystemColor.light_blue => LightBlue,
                SystemColor.light_green => LightGreen,
                SystemColor.light_cyan => LightCyan,
                SystemColor.light_red => LightRed,
                SystemColor.light_magenta => LightMagenta,
                SystemColor.yellow => Yellow,
                SystemColor.white => White,
            };

            public static readonly Color32 Black = new Color32(0, 0, 0);
            public static readonly Color32 Blue = new Color32(0, 0, 170);
            public static readonly Color32 Green = new Color32(0, 170, 0);
            public static readonly Color32 Cyan = new Color32(0, 170, 170);
            public static readonly Color32 Red = new Color32(170, 0, 0);
            public static readonly Color32 Magenta = new Color32(170, 0, 170);
            public static readonly Color32 Brown = new Color32(170, 85, 0);
            public static readonly Color32 LightGray = new Color32(170, 170, 170);
            public static readonly Color32 DarkGray = new Color32(85, 85, 85);
            public static readonly Color32 LightBlue = new Color32(85, 85, 255);
            public static readonly Color32 LightGreen = new Color32(85, 255, 85);
            public static readonly Color32 LightCyan = new Color32(85, 255, 255);
            public static readonly Color32 LightRed = new Color32(255, 85, 85);
            public static readonly Color32 LightMagenta = new Color32(255, 85, 255);
            public static readonly Color32 Yellow = new Color32(255, 255, 85);
            public static readonly Color32 White = new Color32(255, 255, 255);

        }
        public enum SystemColor : byte
        {

            black = 0,
            blue = 1,
            green = 2,
            cyan = 3,
            red = 4,
            magenta = 5,
            brown = 6,
            light_gray = 7,
            dark_gray = 8,
            light_blue = 9,
            light_green = 10,
            light_cyan = 11,
            light_red = 12,
            light_magenta = 13,
            yellow = 14,
            white = 15
        }
    }
}




