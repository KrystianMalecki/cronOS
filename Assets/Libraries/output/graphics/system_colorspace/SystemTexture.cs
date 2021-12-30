using System;
using System.Linq;

namespace Libraries.system.output.graphics
{
    namespace system_texture
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;
        using UnityEngine;

        [Serializable]
        public class SystemTexture : RectArray<SystemColor>
        {
            private static readonly int HEADER_SIZE = 1;
            public byte transparencyFlag = 0xff;

            public SystemTexture(int width, int height) : base(width, height)
            {
            }

            public SystemTexture(RectArray<SystemColor> array) : base(array)
            {
            }

            public SystemTexture()
            {
            }

            public bool UseTransparency()
            {
                return transparencyFlag == 0xff;
            }

            public SystemColor GetTextureTransparencyColor()
            {
                return transparencyFlag;
            }

            public byte[] ToData()
            {
                byte[] header = new byte[HEADER_SIZE];

                header[0] = transparencyFlag;

                return header.Concat(base.ToData(SystemColor.sizeOf, Converter)).ToArray();
            }

            static byte dataInByte = (byte)Mathf.RoundToInt(1f / SystemColor.sizeOf);
            byte[] buffer = new byte[1];
            byte[] buffer2 = new byte[1];

            byte counter;

            public byte[] Converter(SystemColor x)
            {
                buffer[0] <<= 8 / dataInByte;
                buffer[0] += x.byteValue;
                counter++;
                if (counter == dataInByte)
                {
                    buffer2 = buffer.Clone() as byte[];
                    buffer[0] = 0;
                    counter = 0;
                    return buffer2;
                }

                return Array.Empty<byte>();
            }

            public void TintAll(SystemColor color, bool preserveTransparency = true)
            {
                //  Console.Debug($"{transparencyFlag } && {preserveTransparency}");

                for (int i = 0; i < array.Length; i++)
                {
                    if (transparencyFlag == array[i] && preserveTransparency)
                    {
                        Console.Debug("trans");
                        continue;
                    }

                    //  Console.Debug(array[i]);
                    array[i] += color;
                    //   Console.Debug(array[i]);
                }
            }

            public static SystemTexture FromData(byte[] data)
            {
                SystemTexture texture = new SystemTexture(RectArray<SystemColor>.FromData(
                    data.Skip(HEADER_SIZE).ToArray(), SystemColor.sizeOf, x =>
                    {
                        if (dataInByte > 1)
                        {
                            byte val0 = x[0];
                            val0 >>= 8 / dataInByte;
                            byte val1 = x[0];
                            val1 <<= 8 / dataInByte;
                            val1 >>= 8 / dataInByte;
                            return new SystemColor[] { val0, val1 };
                        }

                        return new SystemColor[] { x[0] };
                    }));
                byte[] header = data.Take(HEADER_SIZE).ToArray();

                texture.transparencyFlag = header[0];

                return texture;
            }
        }
    }
}