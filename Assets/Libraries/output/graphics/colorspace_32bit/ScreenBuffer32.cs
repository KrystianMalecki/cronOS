namespace Libraries.system.output.graphics
{
    namespace screen_buffer32
    {
        using Libraries.system.output.graphics.texture32;
        using Libraries.system.output.graphics.color32;
        using Libraries.system.mathematics;
        using Libraries.system.output.graphics;

        public class ScreenBuffer32 : Texture32, IGenericScreenBuffer
        {
            private ScreenBuffer32(int width, int height) : base(width, height)
            {
            }

            public ScreenBuffer32() : base(Screen.screenWidth, Screen.screenHeight)
            {
            }

            public UnityEngine.Color32 GetUnityColorAt(int x, int y)
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
                if (!IsBoxInRange(x, y, texture.width, texture.height) && ProcessorManager.instance.ignoreSomeErrors &&
                    !drawPartialy)
                {
                    return; //todo-future add error
                }

                for (int iterY = 0; iterY < texture.height; iterY++)
                {
                    for (int iterX = 0; iterX < texture.width; iterX++)
                    {
                        if (!IsPointInRange(x, y) && ProcessorManager.instance.ignoreSomeErrors)
                        {
                            return; //todo-future add error
                        }

                        SetAt(iterX + x, iterY + y, texture.GetAt(iterX, iterY));
                    }
                }
            }

            public void DrawLine(int startX, int startY, int endX, int endY, Color32 color)
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
                    Console.Debug(x + ", " + y);

                    SetAt(x, y, color);
                    x = x + Math.Round(dx);
                    y = y + Math.Round(dy);
                    i = i + 1;
                }
            }
        }
    }
}