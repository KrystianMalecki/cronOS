
using Libraries.system.mathematics;
using ue = UnityEngine;
namespace Libraries.system
{
    public class MouseHander : BaseLibrary
    {

        public static Vector2Int GetScreenPosition()
        {
            return ScriptManager.AddDelegateToStack((ref bool done, ref Vector2Int outer) =>
            {
                outer = ((Vector2)ScreenManager.instance.GetMousePos()).ToIntLike();
            });

        }
    }
}
