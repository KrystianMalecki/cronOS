using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

[Serializable]
public class MainThreadDelegate<T> : ITryToRun
{
    public T returnValue;


    public volatile bool done = false;
    public MTDFunction function;
    public int waitRefreshRate;
    private string data;

    public T WaitForReturn()
    {
        while (!done)
        {
            Thread.Sleep(waitRefreshRate);
        }

        return returnValue;
    }

    ~MainThreadDelegate()
    {
        //  Debug.Log( "Destructed" + function.Method.GetMethodBody().GetILAsByteArray().ToFormatedString());
    }

    public void Speak()
    {
        Debug.Log(data /*"I'm a: " + function.Method.GetMethodBody().GetILAsByteArray().ToFormatedString()*/);
    }

    public bool TryToRun()
    {
        if (done)
        {
            Debug.Log("it is now done");
            Speak();
            return done;
        }

        bool buffer = true;


        function.Invoke(ref buffer, ref returnValue);


        done = buffer;

        return done;
    }


    public MainThreadDelegate(MTDFunction fd, int waitRefreshRate)
    {
        function = fd;
        this.waitRefreshRate = waitRefreshRate;
        StackTrace stackTrace = new StackTrace();


        data = stackTrace.ToString();
        // FlagLogger.Log(LogFlags.DebugInfo, "Made from" + fd.Method.GetMethodBody().GetILAsByteArray().ToArrayString());
    }

    public delegate void MTDFunction(ref bool done, ref T returnValue);
}

public interface ITryToRun
{
    bool TryToRun();
    void Speak();
}