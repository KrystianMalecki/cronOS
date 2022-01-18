using System;
using System.Linq;
using Libraries.system.output.graphics.screen_buffer32;
using Libraries.system.output.graphics.system_colorspace;

namespace Libraries.system.output.graphics
{
    namespace mask_texture
    {
        using Libraries.system.mathematics;
        using System.Collections;
        using System.ComponentModel;
        using UnityEngine;

        [Serializable]
        public class MaskTexture : PaletteTexture
        {
            private static readonly int HEADER_SIZE = 1;

            private static readonly new color32.Color32[] palette = new color32.Color32[]
                { ColorConstants.Black32, ColorConstants.White32 };

            public MaskTexture(int width, int height) : base(width, height, palette)
            {
            }

            public MaskTexture(RectArray<bool> array)
            {
                this.array = Array.ConvertAll<bool, byte>(array.array, x => (byte)(x ? 1 : 0));
            }

            public MaskTexture(PaletteTexture paletteTexture)

            {
                array = (byte[])paletteTexture.array.Clone();
                transparencyFlag = paletteTexture.transparencyFlag;
                SetPalette(palette);
            }

            public MaskTexture()
            {
            }

            public new void SetPalette(Libraries.system.output.graphics.color32.Color32[] palette)
            {
                Debug.Log("You can't change palette of mask texture!");
                return;
            }


            public new byte[] ToData()
            {
                byte[] header = new byte[HEADER_SIZE];
                header[0] = transparencyFlag;

                return header.Concat(base.ToData(sizeOfInBits)).ToArray();
            }

            public static MaskTexture FromData(byte[] data)
            {
                MaskTexture pt = new MaskTexture();
                Span<byte> wholeData = new Span<byte>(data);

                Span<byte> header = wholeData.Slice(0, HEADER_SIZE).ToArray();

                pt.transparencyFlag = header[0];


                RectArray<byte> rectArray = RectArray<byte>.FromData(
                    data.Skip(header.Length).ToArray(), 1
                );

                pt.width = rectArray.width;
                pt.height = rectArray.height;
                pt.array = rectArray.array;
                return pt;
            }
        }
    }
}