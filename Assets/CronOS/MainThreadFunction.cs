using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class MainThreadFunction
{
    public static readonly int WaitRefreshRate = 100;
    public object returnValue = null;
    public Func<object> function;
    public bool done = false;

    public void Run()

    {
        returnValue = function.Invoke();
        done = true;
    }


    public object WaitForAction()
    {
        while (!done)
        {
            /* Debug.Log("'" +
                       "wait'"
             );*/
          //  Task.Delay(WaitRefreshRate).Wait();
            Thread.Sleep(WaitRefreshRate);
        }

        return returnValue;
    }

    public MainThreadFunction(Func<object> a)
    {
        function = a;
    }

    public MainThreadFunction()
    {
    }
}