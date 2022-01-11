using Libraries.system.mathematics;
using Libraries.system.output;
using ue = UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class MouseHandler : BaseLibrary
        {
            public Vector2Int lastPosition = Vector2Int.incorrectVector;
            public static System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            public static MainThreadDelegate<Vector2Int>.MTDFunction func = null;

            public Vector2Int GetScreenPosition()
            {
                if (!hardware.currentlySelected)
                {
                    return lastPosition;
                }


                Vector2Int pos =  hardware.hardwareInternal.stackExecutor.AddDelegateToStack(func);


                if (pos == Vector2Int.incorrectVector)
                {
                    return lastPosition;
                }

                lastPosition = pos;
                return pos;
            }

            public override void Init(Hardware hardware)
            {
                base.Init(hardware);
                func = GetMousePos;
            }

            private void GetMousePos(ref bool done, ref Vector2Int returnValue)
            {
                returnValue = hardware.hardwareInternal.screenManager.GetMousePos();
            }
        }
    }
}