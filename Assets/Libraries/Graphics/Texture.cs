using Libraries.system.graphics.color32;
using System;

namespace Libraries.system.graphics
{
    namespace texture32
    {
        [Serializable]
        public class Texture32 : RectArray<Color32>
        {
            public Texture32(int width, int height) : base(width, height)
            {

            }
            public Texture32(RectArray<Color32> array) : base(array)
            {

            }
            public byte[] ToData()
            {
                return base.ToData(Color32.sizeOf, x => new byte[4] { x.r, x.g, x.b, x.a });
            }

            public static Texture32 FromData(byte[] data)
            {
                return new Texture32(RectArray<Color32>.FromData(data, Color32.sizeOf, x => new Color32(x[0], x[1], x[2], x[3])));
            }
        }
    }
    namespace system_texture
    {
        using system_color;

        [Serializable]
        public class SystemTexture : RectArray<SystemColor>
        {
            public SystemTexture(int width, int height) : base(width, height)
            {

            }
            public SystemTexture(RectArray<SystemColor> array) : base(array)
            {

            }
            public byte[] ToData()
            {
                return base.ToData(SystemColor.sizeOf, x => x.value.ToBytes());
            }

            public static SystemTexture FromData(byte[] data)
            {
                Console.Debug("1");
                return new SystemTexture(RectArray<SystemColor>.FromData(data, SystemColor.sizeOf, x => x[0]));
            }

        }
    }
}
