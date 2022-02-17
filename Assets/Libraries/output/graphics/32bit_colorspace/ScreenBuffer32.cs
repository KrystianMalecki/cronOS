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


            public int GetHeight()
            {
                return height;
            }

            UnityEngine.Color32 IGenericScreenBuffer.GetUnityColorAt(int x, int y)
            {
                return GetAt(x, y).ToUnityColor();
            }

            public int GetWidth()
            {
                return width;
            }

            bool ignoreSomeErrors = true; //todo 9 remove

            public void SetTexture(int x, int y, Texture32 texture, bool drawPartialy = true)
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
                        if (!IsPointInRange(x, y) && ignoreSomeErrors)
                        {
                            return; //todo-future add error
                        }

                        SetAt(iterX + x, iterY + y, texture.GetAt(iterX, iterY));
                    }
                }
            }
        }
    }
}