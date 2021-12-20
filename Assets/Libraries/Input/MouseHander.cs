using Libraries.system.mathematics;
using ue = UnityEngine;

namespace Libraries.system
{
    namespace input
    {
        public class MouseHander : BaseLibrary
        {
            private ScreenManager screenManager;
            public void Init(ScreenManager screenManager)
            {
                this.screenManager = screenManager;
            }
            public Vector2Int GetScreenPosition()
            {
                return ScriptManager.AddDelegateToStack((ref bool done, ref Vector2Int outer) =>
                {
                    outer = screenManager.GetMousePos();
                });
            }
        }
    }
}