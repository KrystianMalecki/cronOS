using System.Text.RegularExpressions;
using UnityEngine;

namespace Libraries.system.output.graphics
{
    public interface IGenericScreenBuffer
    {
        internal UnityEngine.Color32 GetUnityColorAt(int x, int y);
        public int GetWidth();
        public int GetHeight();
    }
    //to be replaced
    //again

    //    public static readonly char[] asciiMap = {' ','☺','☻','♥','♦','♣','♠','•','◘','○','◙','♂','♀','♪','♫','☼','►','◄','↕','‼','¶','§','▬','↨','↑','↓','→','←','∟','↔','▲','▼',' ','!','"','#','$','%','&','\'','(',')','*','+',',','-','.','/','0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?','@','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','[','\\',']','^','_','`','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','{','|','}','~','⌂','Ç','ü','é','â','ä','à','å','ç','ê','ë','è','ï','î','ì','Ä','Å','É','æ','Æ','ô','ö','ò','û','ù','ÿ','Ö','Ü','¢','£','¥','₧','ƒ','á','í','ó','ú','ñ','Ñ','ª','º','¿','⌐','¬','½','¼','¡','«','»','░','▒','▓','│','┤','╡','╢','╖','╕','╣','║','╗','╝','╜','╛','┐','└','┴','┬','├','─','┼','╞','╟','╚','╔','╩','╦','╠','═','╬','╧','╨','╤','╥','╙','╘','╒','╓','╫','╪','┘','┌','█','▄','▌','▐','▀','ɑ','ϐ','ᴦ','ᴨ','∑','ơ','µ','ᴛ','ɸ','ϴ','Ω','ẟ','∞','∅','∈','∩','≡','±','≥','≤','⌠','⌡','÷','≈','°','∙','·','√','ⁿ','²','■',' '};

    public class Screen : BaseLibrary
    {
        public static int screenWidth = 640;
        public static int screenHeight = 480;

        public static void InitScreenBuffer(IGenericScreenBuffer screenBuffer)
        {
            //  Hardware hardware = Hardware.currentThreadInstance;
            Hardware.currentThreadInstance.stackExecutor.AddDelegateToStack((hardware) =>
            {
                Debug.Log(screenBuffer);
                hardware.screenManager.InitScreenBuffer(screenBuffer);
            });
        }

        public static void SetScreenBuffer(IGenericScreenBuffer screenBuffer)
        {
            //Hardware hardware = Hardware.currentThreadInstance;

            Hardware.currentThreadInstance.stackExecutor.AddDelegateToStack(
                (hardware) => {
                    Debug.Log(screenBuffer);
                    hardware.screenManager.SetScreenBuffer(screenBuffer); }, sync
            );
        }
        //write regex to match spaces between words BUT NOT IN QUOTES

      //  public static Regex  
        
    }

}