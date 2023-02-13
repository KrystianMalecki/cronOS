using Libraries.system.mathematics;
using System;

namespace Libraries.system
{
    namespace input
    {
        public class MouseHandler 
        {
            [ThreadStatic]
            public static Vector2Int? lastPosition = null;
            [ThreadStatic]
            public static System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            [ThreadStatic]
            public static MainThreadDelegate<Vector2Int?>.MTDFunction func = GetMousePos;

            public static Vector2Int? GetScreenPosition()
            {
                if (!Hardware.currentThreadInstance.focused)
                {
                    return lastPosition;
                }


                Vector2Int? pos = Hardware.currentThreadInstance.hardwareInternal.stackExecutor.AddDelegateToStack(func);


                if (!pos.HasValue)
                {
                    return lastPosition;
                }

                lastPosition = pos;
                return pos;

            }



            private static void GetMousePos(ref bool done, ref Vector2Int? returnValue)
            {
                returnValue = Hardware.currentThreadInstance.hardwareInternal.screenManager.GetMousePos();
            }
        }
    }
}