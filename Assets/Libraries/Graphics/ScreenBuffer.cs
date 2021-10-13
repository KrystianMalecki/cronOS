using Libraries.system.graphics.color32;
using Libraries.system.graphics.texture32;

namespace Libraries.system.graphics
{
    public interface IGenericScreenBuffer
    {
        public UnityEngine.Color32 GetColorAt(int x, int y);
        public int GetWidth();
        public int GetHeight();

    }
    namespace screen_buffer32
    {

        public class ScreenBuffer32 : Texture32, IGenericScreenBuffer
        {
            public ScreenBuffer32(int width, int height) : base(width, height)
            {

            }

            public UnityEngine.Color32 GetColorAt(int x, int y)
            {
                return GetAt(x, y);
            }

            public int GetHeight()
            {
                return height;
            }

            public int GetWidth()
            {
                return width;
            }

            public void SetTexture(int x, int y, Texture32 texture, bool drawPartialy = true)
            {
                if (!IsBoxInRange(x, y, texture.width, texture.height) && ProcessorManager.instance.ignoreSomeErrors && !drawPartialy)
                {
                    return;//todo-future add error
                }
                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        if (!IsPointInRange(x, y) && ProcessorManager.instance.ignoreSomeErrors)
                        {
                            return;//todo-future add error
                        }
                        SetAt(iterX + x, iterY + y, texture.GetAt(iterX, iterY));
                    }
                }
            }

        }
    }
    namespace system_screen_buffer
    {
        using system_texture;
        using system_color;
        using Libraries.system.math;

        public class SystemScreenBuffer : SystemTexture, IGenericScreenBuffer
        {
            public SystemScreenBuffer(int width, int height) : base(width, height)
            {

            }
            public int GetHeight()
            {
                return height;
            }

            public int GetWidth()
            {
                return width;
            }
            public UnityEngine.Color32 GetColorAt(int x, int y)
            {
                return GetAt(x, y).ToColor32();
            }

            public void SetTexture(int x, int y, RectArray<SystemColor> texture, bool drawPartialy = true)
            {
                if (!IsBoxInRange(x, y, texture.width, texture.height) && ProcessorManager.instance.ignoreSomeErrors && !drawPartialy)
                {
                    return;//todo-future add error
                }
                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        if (!IsPointInRange(x, y) && ProcessorManager.instance.ignoreSomeErrors)
                        {
                            return;//todo-future add error
                        }
                        SetAt(x + iterX, y + iterY, texture.GetAt(iterX, iterY));
                    }
                }
            }

        }

    }
}