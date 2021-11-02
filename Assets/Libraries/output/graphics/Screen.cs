
using Libraries.system.mathematics;
using System.Linq;

namespace Libraries.system.output.graphics
{
    public class Screen : BaseLibrary
    {
        public static int screenWidth = 640;
        public static int screenHeight = 480;
        //todo move!
        public static int GetCharacterIndex(char character)
        {
            int index = ScreenManager.asciiMap.ToList().FindIndex(x => x == character);
            return index == -1 ? 0 : index;
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
    }
    public class AsyncScreen : Screen
    {
        public static bool sync => no;
    }
}