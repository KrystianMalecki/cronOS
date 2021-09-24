using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using InternalLogger;
[Serializable]
public class CodeTask
{
    public static readonly string ThreadCodeTaskID = "codeTask";
    public static readonly string ThreadID = "threadID";

    // public Task task = null;
    // public Task codeTask = null;
    public Thread thread;
    public string uuid;
    [ResizableTextArea] public string rawCode;

    public CodeTask()
    {
        uuid = Guid.NewGuid().ToString();
        // InputManager.instance.AddNewBlock(uuid);
    }
    public void RunCode(string rawCode)
    {
        this.rawCode = rawCode;
        thread = new Thread(RunAsync);



        thread.IsBackground = true;
        //  thread.Priority = System.Threading.ThreadPriority.Highest;
        // thread.SetApartmentState(ApartmentState.MTA);



        thread.Start();
        Debug.LogError(thread + "null?" + (thread == null ? "yes" : "no"));
    }
    private async void RunAsync()
    {
        try
        {
            var s = CSharpScript.Create(this.rawCode, CodeRunner.instance.scriptOptions);
            var s2 = s.CreateDelegate();
            await s2.Invoke();
        }
        catch (ThreadAbortException tae)
        {
            //todo add better logging
            FlagLogger.LogWarning(LogFlags.SystemWarning, "Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception e)
        {
            FlagLogger.LogError(LogFlags.SystemError, "error:", e);
        }
        Debug.Log("end");
        //  Destroy();


    }
    ~CodeTask()
    {
        // InputManager.instance.RemoveBlock(uuid);
        Debug.Log("Co dop chuja ct");

        Destroy();
    }
    public void Destroy()
    {
        FlagLogger.LogWarning(LogFlags.SystemWarning, "Destroying CodeTask");
        CodeRunner.instance.RemoveCodeTask(this);
        if (thread != null)
        {
            //  thread.Interrupt();
            thread.Abort();
            thread.Join();
            thread = null;
        }
        else
        {
            //todo add better loging
            FlagLogger.LogWarning(LogFlags.SystemWarning, "thread was null");

        }
    }

}
