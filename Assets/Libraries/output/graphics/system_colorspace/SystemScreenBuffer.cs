using UnityEngine;

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

            Color32 IGenericScreenBuffer.GetUnityColorAt(int x, int y)
            {
                return ((SystemColor)GetAt(x, y)).ToColor32().ToUnityColor();
            }

            public int GetWidth()
            {
                return width;
            }

            /* internal UnityEngine.Color32 GetUnityColorAt(int x, int y)
             {
                 return ((SystemColor)GetAt(x, y)).ToColor32();
             }*/

            bool ignoreSomeErrors = true; //todo 9 remove

            public void DrawTexture(int x, int y, SystemTexture texture, byte transparencyFlag = 0xff,
                bool drawPartialy = true)
            {
                if (!IsBoxInRange(x, y, texture.width, texture.height) && ignoreSomeErrors &&
                    !drawPartialy)
                {
                    return; //todo-future add error
                }

                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        if (transparencyFlag != 0xff && transparencyFlag == texture.GetAt(iterX, iterY))
                        {
                            //  SetAt(x + iterX, y + iterY, SystemColor.red);
                        }
                        else
                        {
                            SetAt(x + iterX, y + iterY, texture.GetAt(iterX, iterY));
                        }
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

            public void DrawLine(Vector2Int start, Vector2Int end, SystemColor color)
            {
                if (start == Vector2Int.incorrectVector || end == Vector2Int.incorrectVector)
                {
                    return;
                }

                DrawLine(start.x, start.y, end.x, end.y, color);
            }
        }
    }
}