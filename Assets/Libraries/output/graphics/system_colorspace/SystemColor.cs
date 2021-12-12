using System;
using System.Globalization;
using UnityEngine;

namespace Libraries.system.output.graphics
{
    namespace system_colorspace
    {
        using color32;

        public static class ColorConstants
        {
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

            public static Color32[] SystemColors = new Color32[]
            {
                Black32, Blue32, Green32, Cyan32, Red32, Magenta32, Brown32, LightGray32, DarkGray32, LightBlue32,
                LightGreen32, LightCyan32, LightRed32, LightMagenta32, Yellow32, White32
            };
        }

        [Serializable]
        public struct SystemColor
        {
            [SerializeField] private byte _value;

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
            public static implicit operator SystemColor(int val)
            {
                return new SystemColor((byte)val);
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

            public const float sizeOf = 1f * sizeof(byte) / 1;

            public static readonly SystemColor black =  (0);
            public static readonly SystemColor blue =  (1);
            public static readonly SystemColor green =  (2);
            public static readonly SystemColor cyan =  (3);
            public static readonly SystemColor red =  (4);
            public static readonly SystemColor magenta =  (5);
            public static readonly SystemColor brown =  (6);
            public static readonly SystemColor light_gray =  (7);
            public static readonly SystemColor dark_gray =  (8);
            public static readonly SystemColor light_blue =  (9);
            public static readonly SystemColor light_green = (10);
            public static readonly SystemColor light_cyan = (11);
            public static readonly SystemColor light_red = (12);
            public static readonly SystemColor light_magenta = (13);
            public static readonly SystemColor yellow = (14);
            public static readonly SystemColor white = (15);
            public Color32 ToColor32() => ColorConstants.SystemColors[value];
        }
    }
}