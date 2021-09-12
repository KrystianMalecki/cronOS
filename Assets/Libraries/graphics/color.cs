using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace libraries.system.graphics
{
    public struct color
    {
        public byte r;
        public byte g;
        public byte b;
        public System.Boolean a;
        public color(byte r, byte g, byte b, bool a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public color(byte r, byte g, byte b, int a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a > 0;
        }
        public color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = true;
        }
        public override string ToString()
        {
            return "[" + r + "," + g + "," + b + "," + a + "," + "]";
        }
        public Color32 ToUnityColor()
        {

            return new Color32(r, g, b, (byte)(a ? 255 : 0));
        }
    }
}
