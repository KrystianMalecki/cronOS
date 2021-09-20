using Libraries.system.graphics.color32;
using Libraries.system.graphics.texture32;


namespace Libraries.system.graphics
{
    namespace screen_buffer32
    {
        public class ScreenBuffer32 : Texture32
        {
            public ScreenBuffer32(int width, int height) : base(width, height)
            {

            }
            public void FillAll(Color32 color)
            {
                Fill(0, 0, width, height, color);
            }
            public void SetTexture(int x, int y, Texture32 texture)
            {
                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        //todo add check
                        SetPixel(iterX + x, iterY + y, texture.GetPixel(x, y));
                    }
                }
            }

        }
    }
    namespace system_screen_buffer
    {
        using system_texture;
        using system_color;

        public class SystemScreenBuffer : SystemTexture
        {
            public SystemScreenBuffer(int width, int height) : base(width, height)
            {

            }
            public void FillAll(SystemColor color)
            {
                Fill(0, 0, width, height, color);
            }
            public void SetTexture(int x, int y, SystemTexture texture)
            {
                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        //todo add check
                        SetPixel(iterX + x, iterY + y, texture.GetPixel(x, y));
                    }
                }
            }

        }

    }
}