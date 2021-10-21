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
        using mathematics;
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
            public void DrawLine(int startX, int startY, int endX, int endY, Color32 color)
            {
                int dx, dy, p, x, y;
                dx = endX - startX;
                dy = endY - startY;
                x = startX;
                y = startY;
                p = 2 * dy - dx;
                while (x < endX)
                {
                    if (p >= 0)
                    {
                        SetAt(x, y, color);
                        y = y + 1;
                        p = p + 2 * dy - 2 * dx;
                    }
                    else
                    {
                        SetAt(x, y, color);
                        p = p + 2 * dy;
                    }
                    x = x + 1;
                }
            }
            public void DrawLine2(int startX, int startY, int endX, int endY, Color32 color)
            {
                int x;
                int y;
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
                    SetAt(x, y, color);
                    x = x + Math.Round(dx);
                    y = y + Math.Round(dy);
                    i = i + 1;
                }
            }
        }
    }
    namespace system_screen_buffer
    {
        using system_texture;
        using system_color;
        using Libraries.system.mathematics;

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
            public void DrawLine(int startX, int startY, int endX, int endY, SystemColor color)
            {
                int dx = endX - startX;
                int dy = endY - startY;
                for (int i = startX; i < endX; i++)
                {
                    int y = startY + dy * (i - startX) / dx;
                    SetAt(i, y, color);

                }



                /*  int dx, dy, p, x, y;
                  dx = endX - startX;
                  dy = endY - startY;
                  x = startX;
                  y = startY;
                  p = 2 * dy - dx;
                  while (x < endX)
                  {
                      if (p >= 0)
                      {
                          SetAt(x, y, color);
                          y = y + 1;
                          p = p + 2 * dy - 2 * dx;
                      }
                      else
                      {
                          SetAt(x, y, color);
                          p = p + 2 * dy;
                      }
                      x = x + 1;
                  }*/
            }
            public void DrawLine2(int startX, int startY, int endX, int endY, SystemColor color)
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