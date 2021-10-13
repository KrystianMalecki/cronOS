using Libraries.system.graphics.system_screen_buffer;
using Libraries.system.graphics.screen_buffer32;
using Libraries.system.math;
using System.Linq;

namespace Libraries.system.graphics
{
    public class Screen : BaseLibrary
    {
        public static readonly int defaultScreenWidth = 640;
        public static readonly int defaultScreenHeight = 480;


        public static ScreenBuffer32 MakeScreenBuffer32(int width = -1, int height = -1)
        {
            if (width == -1)
            {
                width = defaultScreenWidth;
            }
            if (height == -1)
            {
                height = defaultScreenHeight;
            }
            return new ScreenBuffer32(width, height);
        }
        public static SystemScreenBuffer MakeSystemScreenBuffer(int width = -1, int height = -1)
        {
            if (width == -1)
            {
                width = defaultScreenWidth;
            }
            if (height == -1)
            {
                height = defaultScreenHeight;
            }
            return new SystemScreenBuffer(width, height);
        }
        public static void InitScreenBuffer(IGenericScreenBuffer screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.InitScreenBuffer(screenBuffer);
            });
        }
        public static void SetScreenBuffer(IGenericScreenBuffer screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screenBuffer);
            }, sync
            );

        }
        public static int GetCharacterIndex(char character)
        {
            return ScreenManager.asciiMap.ToList().FindIndex(x => x == character);
        }
    }
    public class AsyncScreen : Screen
    {
        public static bool sync => no;
    }
}