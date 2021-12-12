using Libraries.system.mathematics;
using ue = UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class MouseHander : BaseLibrary
        {
            public static Vector2Int GetScreenPosition()
            {
                return ScriptManager.AddDelegateToStack((ref bool done, ref Vector2Int outer) =>
                {
                    outer = ScreenManager.instance.GetMousePos();
                });
            }
        }
    }
}