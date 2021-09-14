
using System;
using System.Globalization;
using UnityEngine;

namespace Libraries.system.graphics
{
    public struct Color
    {
        private Color32 color;
        internal Color32 GetColor32()
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
        public Color(byte r, byte g, byte b, byte a)
        {
            color = new Color32(r, g, b, a);
        }
        public Color(Color32 color32)
        {
            color = color32;
        }
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(Color32.Lerp(a.color, b.color, t));
        }
        public static Color LerpUnclamped(Color a, Color b, float t)
        {
            return new Color(Color32.Lerp(a.color, b.color, t));
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



