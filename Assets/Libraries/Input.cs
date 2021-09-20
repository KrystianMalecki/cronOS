using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
namespace Libraries.system
{
    public class Input : BaseLibrary
    {
        public static List<KeyCode> WaitForAny()
        {
            List<KeyCode> kes = new List<KeyCode>();
            while (kes.Count <= 0)
            {
                Thread.Sleep(1000);//todo change to some static value
                kes = (List<KeyCode>)CodeRunner.AddFunctionToStack(() =>
                {
                    return KeyboardInputHelper.GetCurrentKeyss();
                }, true);

            }

            return kes;
        }

    }

}
