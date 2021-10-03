using InternalLogger;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MainThreadDelegate<T> : ITryToRun
{
    public T returnValue;
    public volatile bool done = false;
    public MTDFunction function;
    public T WaitForReturn()
    {
        while (!done)
        {
            Thread.Sleep(ProcessorManager.instance.WaitRefreshRate);
        }
        return returnValue;
    }
    ~MainThreadDelegate()
    {
        FlagLogger.Log(LogFlags.DebugInfo, "Destructed" + function.Method.GetMethodBody().GetILAsByteArray().ToArrayString());

    }
    public void Speak()
    {
        FlagLogger.Log(LogFlags.DebugInfo, "i'm a: " + function.Method.GetMethodBody().GetILAsByteArray().ToArrayString());
    }

    public bool TryToRun()
    {
        if (done)
        {
            FlagLogger.Log(LogFlags.DebugInfo, "it is now done");
            Speak();
            return done;
        }
        bool buffer = true;
        function.Invoke(ref buffer, ref returnValue);
        done = buffer;
        return done;
    }


    public MainThreadDelegate(MTDFunction fd)
    {
        function = fd;
        // FlagLogger.Log(LogFlags.DebugInfo, "Made from" + fd.Method.GetMethodBody().GetILAsByteArray().ToArrayString());

    }
    public delegate void MTDFunction(ref bool done, ref T returnValue);
}
public interface ITryToRun
{
    bool TryToRun();
    void Speak();
}