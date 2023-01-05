//#define DLL

using System.Runtime.CompilerServices;

namespace Libraries.system
{
    namespace output
    {
        public class Console : BaseLibrary
        {
            public static void Debug(params object[] obj)
            {
                UnityEngine.Debug.Log(obj.ToFormattedString());
            }

            public static void Debug(object obj = null,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
            {
                UnityEngine.Debug.Log($"{caller} at {lineNumber + 1}. Logs: '{obj}'");//todo 99 warning, +1 is here because on top is "#if false added".
            }

            public static void Line([CallerLineNumber] int line = 0)
            {
                UnityEngine.Debug.Log(line);
            }
        }
    }
}