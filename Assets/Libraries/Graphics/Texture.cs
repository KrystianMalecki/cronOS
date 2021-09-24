using Libraries.system.graphics.color32;

namespace Libraries.system.graphics
{
    namespace texture32
    {
        public class Texture32 : RectArray<Color32>
        {
            public Texture32(int width, int height) : base(width, height)
            {

            }
        }
    }
    namespace system_texture
    {
        using system_color;
        public class SystemTexture : RectArray<SystemColor>
        {
            public SystemTexture(int width, int height) : base(width, height)
            {

            }
        }
    }
}
