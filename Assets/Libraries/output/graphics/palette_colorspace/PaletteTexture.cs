using System;
using System.Collections;
using System.Linq;
using Libraries.system.output.graphics.system_texture;
using UnityEngine;

namespace Libraries.system.output.graphics
{
    namespace screen_buffer32
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;
        using color32;

        [Serializable]
        public class PaletteTexture : RectArray<byte>, ICompressable
        {
            public byte transparencyFlag = 0xff;
            private int sizeOfInBits;
            public Color32[] palette;

//todo 0 check if palette is bigger than 256
            public PaletteTexture(int width, int height, Color32[] palette) : base(width, height)
            {
                SetPalette(palette);
            }

            public PaletteTexture(RectArray<byte> array, Color32[] palette) : base(array)
            {
                SetPalette(palette);
            }

            public PaletteTexture()
            {
            }

            public void SetPalette(Color32[] palette)
            {
                this.palette = palette;
                if (this.palette.Length >= 32)
                {
                    sizeOfInBits = 8;
                }
                else if (this.palette.Length >= 8)
                {
                    sizeOfInBits = 4;
                }
                else if (this.palette.Length >= 4)
                {
                    sizeOfInBits = 2;
                }
                else
                {
                    sizeOfInBits = 1;
                }
            }

            //todo 0 fix this everywhere 
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
                short headerSize = (short)(2 + 2 + (short)(palette.Length * Color32.sizeOfInBits * 8));
                byte[] header = new byte[headerSize];
                header[0] = headerSize.ToBytes()[0];
                header[1] = headerSize.ToBytes()[1];

                header[2] = transparencyFlag;
                header[3] = (byte)palette.Length;

                for (int i = 0; i < palette.Length; i++)
                {
                    for (int j = 0; j < (int)Color32.sizeOfInBits / 8; j++)
                    {
                        // Debug.Log(4 + i * 4 + j);
                        header[4 + i * 4 + j] = palette[i][j];
                    }
                }

                BitArray ba = new BitArray(header);

                return header.Concat(base.ToData(sizeOfInBits, Converter)).ToArray();
                return null;
            }

            byte[] buffer = new byte[1];

            byte counter;

//todo 0 work with variable palette size
            public byte[] Converter(byte x)
            {
                //todo 0 fix    buffer[0] <<= 8 / colorsInByte;
                buffer[0] += x;
                counter++;
                //todo 0 fix     if (counter == colorsInByte)
                {
                    counter = 0;
                    return (byte[])buffer.Clone();
                }

                return Array.Empty<byte>();
            }

//todo 0 move to interface or base Texture class
            public void TintAll(byte color, bool preserveTransparency = true)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (transparencyFlag == array[i] && preserveTransparency)
                    {
                        continue;
                    }

                    array[i] += color;
                }
            }

            public void FromData(byte[] data)
            {
                Span<byte> header2 = new Span<byte>(data);
                short headerSize = BitConverter.ToInt16(header2.Slice(0, 2).ToArray(), 0);
                Span<byte> header = header2.Slice(0, headerSize).ToArray();

                transparencyFlag = header[2];

                SetPalette(new Color32[header[3]]);
                for (int i = 0; i < palette.Length; i++)
                {
                    palette[i] = new Color32(header.Slice(i * 4 + 4, 4).ToArray());
                }

                RectArray<byte> rectArray = RectArray<byte>.FromData(
                    data.Skip(headerSize).ToArray(), sizeof(byte), x =>
                    {
                        //todo -0 fix here, data is still 0
                        if (8 / sizeOfInBits > 1)
                        {
                            BitArray bits = new BitArray(x);
                            byte[] bytes = new byte[8 / sizeOfInBits];
                            for (int i = 0; i < 8 / sizeOfInBits; i++)
                            {
                                byte b = 0;
                                for (int j = 0; j < sizeOfInBits; j++)
                                {
                                    b += (byte)(bits[7 - j] ? 1 : 0);
                                    b <<= 1;
                                }

                                bytes[i] = b;
                            }

                            return bytes;
                        }

                        return new byte[] { x[0] };
                    });
                width = rectArray.width;
                height = rectArray.height;
                array = rectArray.array;
            }
        }
    }
}