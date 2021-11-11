using System;
using System.Linq;
namespace Libraries.system.output.graphics
{
    namespace system_texture
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;

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

                return header.Concat(base.ToData(SystemColor.sizeOf, x => x.value.ToBytes())).ToArray();
            }

            public static SystemTexture FromData(byte[] data)
            {
                SystemTexture texture = new SystemTexture(RectArray<SystemColor>.FromData(data.Skip(HEADER_SIZE).ToArray(), SystemColor.sizeOf, x => x[0]));
                byte[] header = data.Take(HEADER_SIZE).ToArray();

                texture.transparencyFlag = header[0];

                return texture;
            }

        }
    }
}
