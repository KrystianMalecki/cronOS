using System;
using System.Linq;
using Libraries.system.output.graphics.screen_buffer32;

namespace Libraries.system.output.graphics
{
    namespace system_texture
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;
        using UnityEngine;

        [Serializable]
        public class SystemTexture : PaletteTexture
        {
            private static readonly int HEADER_SIZE = 1;

            public SystemTexture(int width, int height) : base(width, height, ColorConstants.SystemColors)
            {
            }

            public SystemTexture(RectArray<SystemColor> array)
            {
                this.array = Array.ConvertAll<SystemColor, byte>(array.array, x => x);
            }

            public SystemTexture(PaletteTexture paletteTexture)

            {
                array = (byte[])paletteTexture.array.Clone();
                transparencyFlag = paletteTexture.transparencyFlag;
                SetPalette(ColorConstants.SystemColors);
            }

            private static readonly new int sizeOfInBits =
                PaletteTexture.ColorCountToSOIB(ColorConstants.SystemColors.Length);

            public new byte[] ToData()
            {
                byte[] header = new byte[HEADER_SIZE];
                header[0] = transparencyFlag;

                return header.Concat(base.ToData(sizeOfInBits)).ToArray();
            }

            public SystemTexture()
            {
            }

            public new void SetPalette(Libraries.system.output.graphics.color32.Color32[] palette)
            {
                Debug.Log("You can't change palette of system texture!");
                //todo 4 think baout this, maybe you could make it like 4 color palette per sprite optional?
                return;
            }

            public void TintAll(SystemColor color, bool preserveTransparency = true)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (transparencyFlag == array[i] && preserveTransparency)
                    {
                        Console.Debug("trans");
                        continue;
                    }

                    array[i] += color;
                }
            }

            public new SystemColor GetTransparencyColor()
            {
                return base.GetTransparencyColor();
            }

            public static SystemTexture FromData(byte[] data)
            {
                SystemTexture pt = new SystemTexture();
                Span<byte> wholeData = new Span<byte>(data);

                Span<byte> header = wholeData.Slice(0, HEADER_SIZE).ToArray();

                pt.transparencyFlag = header[0];


                RectArray<byte> rectArray = RectArray<byte>.FromData(
                    data.Skip(header.Length).ToArray(), sizeOfInBits // sizeOfInBits
                );

                pt.width = rectArray.width;
                pt.height = rectArray.height;
                pt.array = rectArray.array;
                return pt;
            }
        }
    }
}