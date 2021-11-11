

namespace Libraries.system.output.graphics
{

    namespace system_screen_buffer
    {
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics.system_colorspace;
        using Libraries.system.output.graphics.system_texture;

        public class SystemScreenBuffer : SystemTexture, IGenericScreenBuffer
        {
            public SystemScreenBuffer(int width, int height) : base(width, height)
            {

            }
            public SystemScreenBuffer() : base(Screen.screenWidth, Screen.screenHeight)
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
            public UnityEngine.Color32 GetUnityColorAt(int x, int y)
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
            public void DrawLine(int startX, int startY, int endX, int endY, SystemColor color)
            {
                float x;
                float y;
                float dx, dy, step;
                int i;

                dx = (endX - startX);
                dy = (endY - startY);
                if (Math.Abs(dx) >= Math.Abs(dy))
                    step = Math.Abs(dx);
                else
                    step = Math.Abs(dy);
                dx = dx / step;
                dy = dy / step;
                x = startX;
                y = startY;
                i = 1;
                while (i <= step)
                {

                    SetAt(Math.Round(x), Math.Round(y), color);
                    x = x + dx;
                    y = y + dy;
                    i = i + 1;
                }
            }
        }

    }
}