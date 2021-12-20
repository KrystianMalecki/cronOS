using Libraries.system.mathematics;
using ue = UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class MouseHandler : BaseLibrary
        {

            public Vector2Int GetScreenPosition()
            {
                return hardware.hardwareInternal.stackExecutor.AddDelegateToStack((ref bool done, ref Vector2Int outer) =>
                {
                    outer = hardware.hardwareInternal.screenManager.GetMousePos();
                });
            }
        }
    }
}