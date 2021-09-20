using Libraries.system.graphics.color32;

namespace Libraries.system.graphics
{
    namespace texture32
    {
        public class Texture32
        {
            public int width;
            public int height;
            public Color32[] texture;
            public Texture32(int width, int height)
            {
                this.width = width;
                this.height = height;
                texture = new Color32[width * height];
            }

            public void SetPixel(int x, int y, Color32 color)
            {
                texture[y * width + x] = color;
            }
            public void Fill(int x, int y, int width, int height, Color32 color)
            {
                for (int iterY = y; iterY < height; iterY++)
                {
                    for (int iterX = x; iterX < width; iterX++)
                    {
                        //todo add check
                        SetPixel(iterX, iterY, color);
                    }
                }
            }
            public Color32 GetPixel(int x, int y)
            {
                //todo add check
                return texture[y * width + x];
            }
        }
    }
    namespace system_texture
    {
        using system_color;
        public class SystemTexture
        {
            public int width;
            public int height;
            public SystemColor[] texture;
            public SystemTexture(int width, int height)
            {
                this.width = width;
                this.height = height;
                texture = new SystemColor[width * height];
            }

            public void SetPixel(int x, int y, SystemColor color)
            {
                texture[y * width + x] = color;
            }
            public void Fill(int x, int y, int width, int height, SystemColor color)
            {
                for (int iterY = y; iterY < height; iterY++)
                {
                    for (int iterX = x; iterX < width; iterX++)
                    {
                        //todo add check
                        SetPixel(iterX, iterY, color);
                    }
                }
            }
            public SystemColor GetPixel(int x, int y)
            {
                //todo add check
                return texture[y * width + x];
            }
        }
    }
}