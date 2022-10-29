//#define DLL

using Libraries.system.file_system;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Libraries.system
{
    public class Runtime : BaseLibrary
    {
        public static void Wait(int time = 0)
        {
            // Task.Delay(time).Wait();
            Thread.Sleep(System.Math.Max(time, Hardware.currentThreadInstance.hardwareInternal.WaitRefreshRate));

            // test.instance.count1++;
        }

        public static void WaitUnitl(Func<bool> action)
        {
            while (action.Invoke())
            {
                Wait();
            }
        }

        //todo-future move to better library
        public static File Compile(File file)
        {
            return Hardware.currentThreadInstance.hardwareInternal.Compile(file);
        }
        public static void Execute(File compiledFile, string args = null)
        {
            Hardware.currentThreadInstance.hardwareInternal.Execute(compiledFile);
        }

        #region Just statics that can be used in any thread
        public static readonly char[] asciiMap =
        {
            ' ', '☺', '☻', '♥', '♦', '♣', '♠', '•', '◘', '○', '◙', '♂', '♀', '♪', '♫', '☼', '►', '◄', '↕', '‼', '¶',
            '§', '▬', '↨', '↑', '↓', '→', '←', '∟', '↔', '▲', '▼', ' ', '!', '"', '#', '$', '%', '&', '\'', '(', ')',
            '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>',
            '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
            'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}',
            '~', '⌂', 'Ç', 'ü', 'é', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è', 'ï', 'î', 'ì', 'Ä', 'Å', 'É', 'æ', 'Æ',
            'ô', 'ö', 'ò', 'û', 'ù', 'ÿ', 'Ö', 'Ü', '¢', '£', '¥', '₧', 'ƒ', 'á', 'í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º',
            '¿', '⌐', '¬', '½', '¼', '¡', '«', '»', '░', '▒', '▓', '│', '┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝',
            '╜', '╛', '┐', '└', '┴', '┬', '├', '─', '┼', '╞', '╟', '╚', '╔', '╩', '╦', '╠', '═', '╬', '╧', '╨', '╤',
            '╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█', '▄', '▌', '▐', '▀', 'ɑ', 'ϐ', 'ᴦ', 'ᴨ', '∑', 'ơ', 'µ',
            'ᴛ', 'ɸ', 'ϴ', 'Ω', 'ẟ', '∞', '∅', '∈', '∩', '≡', '±', '≥', '≤', '⌠', '⌡', '÷', '≈', '°', '∙', '·', '√',
            'ⁿ', '²', '■', ' '
        };
        #region char to bytes
        public unsafe static byte CharToByte(char character)
        {
            byte b = 0;
            HardwareInternal.mainEncoding.GetBytes(&character, 1, &b, 1);
            return b;
        }

        public unsafe static char ByteToChar(byte character)
        {
            char b = 'l';
            HardwareInternal.mainEncoding.GetChars(&character, 1, &b, 1);
            return b;
        }
        #endregion

        #region hex to bytes

        public static byte HexToByte(string value)
        {
            byte result = 0;
            byte.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out result);
            return result;
        }

        public static string ByteToHex(byte value, bool makeIt2)
        {
            return value.ToString("X" + (makeIt2 ? 2 : 1));
        }
        #endregion

        #region string to bytes

        public static byte[] StringToEncodedBytes(string variable)
        {
            if (string.IsNullOrEmpty(variable))
            {
                return Array.Empty<byte>();
            }

            return HardwareInternal.mainEncoding.GetBytes(variable);
        }

        public static string BytesToEncodedString(byte[] variable)
        {
            if (variable == null)
            {
                return "";
            }

            return HardwareInternal.mainEncoding.GetString(variable);
        }
        #endregion
        #endregion
    }
}