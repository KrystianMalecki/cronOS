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
            [SerializeField] private sbyte _value;

            public byte byteValue
            {
                get { return (byte)value; }
                set
                {
                    FixRange(ref value);
                    this.value = (sbyte)value;
                }
            }

            public sbyte value
            {
                get { return _value; }
                set
                {
                    FixRange(ref value);

                    this._value = value;
                }
            }

            public const bool loop = true;
            public void FixRange(ref byte value)
            {
                if (loop)
                {
                    value = (byte)(value % 16);
                    if (value < 0)
                    {
                        value += 16;
                    }
                }
                else
                {
                    value = (byte)Math.Clamp((byte)value, (byte)0, (byte)16);
                }
            }

            public void FixRange(ref sbyte value)
            {
                if (loop)
                {
                    value = (sbyte)(value % 16);
                    if (value < 0)
                    {
                        value += 16;
                    }
                }
                else
                {
                    value = (sbyte)Math.Clamp((sbyte)value, (sbyte)0, (sbyte)16);
                }
            }

            public void Add(sbyte valueToAdd)
            {
                this.value += valueToAdd;
            }

            public void Add(byte valueToAdd)
            {
                this.value += (sbyte)valueToAdd;
            }

            public void Multiply(sbyte valueToMultiply)
            {
                this.value *= valueToMultiply;
            }

            public void Divide(sbyte valueToMultiply)
            {
                this.value /= valueToMultiply;
            }

            public SystemColor(sbyte value)
            {
                this._value = 0;
                FixRange(ref value);
                this._value = value;
            }

            public SystemColor(byte value)
            {
                this._value = 0;
                FixRange(ref value);
                this._value = (sbyte)value;
            }

            public static implicit operator SystemColor(sbyte val)
            {
                return new SystemColor(val);
            }

            public static implicit operator SystemColor(byte val)
            {
                return new SystemColor(val);
            }

            public static implicit operator SystemColor(int val)
            {
                return new SystemColor((sbyte)val);
            }

            public static implicit operator sbyte(SystemColor val)
            {
                return val.value;
            }

            public static implicit operator byte(SystemColor val)
            {
                return (byte)val.value;
            }

            public static SystemColor operator ++(SystemColor sc)
            {
                sc.Add(1);
                return sc;
            }

            public static SystemColor operator --(SystemColor sc)
            {
                sc.Add(-1);
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

            public static SystemColor operator +(SystemColor sc, SystemColor sc2)
            {
                SystemColor ret = sc.Copy();
                ret.Add(sc2.value);
                return ret;
            }

            public static SystemColor operator -(SystemColor sc, SystemColor sc2)
            {
                SystemColor ret = sc.Copy();
                ret.Add((sbyte)-sc2.value);
                return ret;
            }

            public static SystemColor operator *(SystemColor sc, SystemColor sc2)
            {
                SystemColor ret = sc.Copy();
                ret.Multiply((sbyte)sc2.value);
                return ret;
            }

            public static SystemColor operator /(SystemColor sc, SystemColor sc2)
            {
                SystemColor ret = sc.Copy();
                ret.Divide((sbyte)sc2.value);
                return ret;
            }

            public override string ToString()
            {
                return value switch
                {
                    0 => "black",
                    1 => "blue",
                    2 => "green",
                    3 => "cyan",
                    4 => "red",
                    5 => "magenta",
                    6 => "brown",
                    7 => "light_gray",
                    8 => "dark_gray",
                    9 => "light_blue",
                    10 => "light_green",
                    11 => "light_cyan",
                    12 => "light_red",
                    13 => "light_magenta",
                    14 => "yellow",
                    15 => "white",
                    _ => "unknown"
                };

                //return value.ToString();
            }

            public SystemColor ChangeShade()
            {
                value -= 8;
                return this;
            }

            public SystemColor Darken()
            {
                if (value >= 8)
                {
                    value -= 8;
                }

                return this;
            }

            public SystemColor Lighten()
            {
                if (value < 8)
                {
                    value += 8;
                }

                return this;
            }

            public SystemColor Copy()
            {
                return new SystemColor(value);
            }

            public const int sizeOfInBits = 4;

            public static readonly SystemColor black = (0);
            public static readonly SystemColor blue = (1);
            public static readonly SystemColor green = (2);
            public static readonly SystemColor cyan = (3);
            public static readonly SystemColor red = (4);
            public static readonly SystemColor magenta = (5);
            public static readonly SystemColor brown = (6);
            public static readonly SystemColor light_gray = (7);
            public static readonly SystemColor dark_gray = (8);
            public static readonly SystemColor light_blue = (9);
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