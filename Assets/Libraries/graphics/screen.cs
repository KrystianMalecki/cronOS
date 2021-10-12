using Libraries.system.graphics.system_screen_buffer;
using Libraries.system.graphics.screen_buffer32;
using Libraries.system.math;
using System.Linq;

namespace Libraries.system.graphics
{
    public class Screen : BaseLibrary
    {
        public static readonly int screenWidth = 128;
        public static readonly int screenHeight = 128;


        public static ScreenBuffer32 MakeScreenBuffer32()
        {
            return new ScreenBuffer32(screenWidth, screenHeight);
        }
        public static SystemScreenBuffer MakeSystemScreenBuffer()
        {
            return new SystemScreenBuffer(screenWidth, screenHeight);
        }
        public static void InitScreenBuffer32(ScreenBuffer32 screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.InitScreenBuffer(screenBuffer);
            });
        }
        public static void InitSystemScreenBuffer(SystemScreenBuffer screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.InitScreenBuffer(screenBuffer);
            });
        }
        public static void SetScreenBuffer(ScreenBuffer32 screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screenBuffer);
            }, sync
            );

        }
        public static void SetScreenBuffer(SystemScreenBuffer screenBuffer)
        {
            ScriptManager.AddDelegateToStack(() =>
            {
                ScreenManager.instance.SetScreenBuffer(screenBuffer);
            }, sync);

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