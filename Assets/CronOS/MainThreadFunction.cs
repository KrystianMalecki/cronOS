using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class MainThreadFunction
{
    public static readonly int WaitRefreshRate = 100;
   // [SerializeField]
    public object returnValue = null;
    public Func<object> function;
    public bool done = false;

    public/* unsafe*/ void Run()
    {
       /* Debug.Log("r- started running");*/

        returnValue = function.Invoke();
       /* TypedReference tr = __makeref(returnValue);
        IntPtr ptr = **(IntPtr**)(&tr);
       
        Debug.Log("r- pointer" + ptr);

        Debug.Log("r- finished running");
        Debug.Log("r- is return null? " + (returnValue == null ? "is null" : "not null"));
        Debug.Log("r- return '" + returnValue + "'");*/

        done = true;
    }


    public /*unsafe*/ object WaitForAction()
    {
     //   Debug.Log("t- started waiting");
        while (!done)
        {
             Thread.Sleep(WaitRefreshRate);
        //    Debug.Log("t- loop is return null? " + (returnValue == null ? "is null" : "not null"));
         //   Debug.Log("t- loop return '" + returnValue + "'");

         //   TypedReference tr = __makeref(returnValue);
         //   IntPtr ptr = **(IntPtr**)(&tr);
         //   Debug.Log("t- loop pointer" + ptr);
        }
       // Debug.Log("t- finished ");
       // Debug.Log("t- is return null? " + (returnValue == null ? "is null" : "not null"));
     //   Debug.Log("t- return '" + returnValue + "'");

        return returnValue;
    }
    ~MainThreadFunction()
    {
        Dispose();
    }
    public void Dispose()
    {
        returnValue = null;
        function = null;
       //ii Debug.Log("d- started running");
    }

    public MainThreadFunction(Func<object> a)
    {
        function = a;
        done = false;
    }


}