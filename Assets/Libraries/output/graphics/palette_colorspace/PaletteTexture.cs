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
        public class PaletteTexture : RectArray<byte>
        {
            public byte transparencyFlag = 0xff;
            public int sizeOfInBits;
            protected Color32[] palette;

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


                sizeOfInBits = ColorCountToSOIB(this.palette.Length);
            }

            public static int ColorCountToSOIB(int count)
            {
                if (count > 16)
                {
                    return 8;
                }
                else if (count > 4)
                {
                    return 4;
                }
                else if (count > 2)
                {
                    return 2;
                }
                else
                {
                    return 1;
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
                short headerSize = (short)(2 + 2 + (short)(palette.Length * Color32.sizeOfInBits / 8));
                byte[] header = new byte[headerSize];
                header[0] = headerSize.ToBytes()[0];
                header[1] = headerSize.ToBytes()[1];

                header[2] = transparencyFlag;
                header[3] = (byte)palette.Length;

                for (int i = 0; i < palette.Length; i++)
                {
                    for (int j = 0; j < (int)(Color32.sizeOfInBits / 8); j++)
                    {
                        // Debug.Log(4 + i * 4 + j);
                        header[4 + i * 4 + j] = palette[i][j];
                        Debug.Log($"i{i} j{j} sum{4 + i * 4 + j} val{palette[i][j]}");
                    }
                }


                return header.Concat(base.ToData(sizeOfInBits)).ToArray();
            }

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

/*
            public void FromData(byte[] data)
            {
                Span<byte> wholeData = new Span<byte>(data);
                short headerSize = BitConverter.ToInt16(wholeData.Slice(0, 2).ToArray(), 0);
                Span<byte> header = wholeData.Slice(0, headerSize).ToArray();

                transparencyFlag = header[2];

                SetPalette(new Color32[header[3]]);
                for (int i = 0; i < palette.Length; i++)
                {
                    palette[i] = new Color32(header.Slice(i * 4 + 4, 4).ToArray());
                }

                RectArray<byte> rectArray = RectArray<byte>.FromData(
                    data.Skip(headerSize).ToArray(), sizeOfInBits
                );
                width = rectArray.width;
                height = rectArray.height;
                array = rectArray.array;
            }
*/
            public static PaletteTexture FromData(byte[] data, int sizeOfInBits)
            {
                PaletteTexture pt = new PaletteTexture();
                Span<byte> wholeData = new Span<byte>(data);
                short headerSize = BitConverter.ToInt16(wholeData.Slice(0, 2).ToArray(), 0);
                Span<byte> header = wholeData.Slice(0, headerSize).ToArray();

                pt.transparencyFlag = header[2];

                pt.SetPalette(new Color32[header[3]]);
                for (int i = 0; i < pt.palette.Length; i++)
                {
                    pt.palette[i] = new Color32(header.Slice(i * 4 + 4, 4).ToArray());
                }

                RectArray<byte> rectArray = RectArray<byte>.FromData(
                    data.Skip(headerSize).ToArray(), sizeOfInBits
                );

                pt.width = rectArray.width;
                pt.height = rectArray.height;
                pt.array = rectArray.array;
                return pt;
            }

            public static PaletteTexture FromDataUsingColorCount(byte[] data, int colorCount)
            {
                return FromData(data, ColorCountToSOIB(colorCount));
            }
        }
    }
}