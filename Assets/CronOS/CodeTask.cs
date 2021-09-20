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

[Serializable]
public class CodeTask
{
    public static readonly string ThreadCodeTaskID = "codeTask";
    public static readonly string ThreadID = "threadID";

    // public Task task = null;
    // public Task codeTask = null;
    public Thread thread;
    [ResizableTextArea] public string rawCode;
    public bool active;
    [SerializeField]
    public Queue<MainThreadFunction> actionStack = new Queue<MainThreadFunction>();

    public void RunCode(string rawCode)
    {
        this.rawCode = rawCode;
        // task = Task.Run(RunAsync);
        thread = new Thread(RunAsync);



        thread.IsBackground = false;
        thread.Start();
    }
    private async void RunAsync()
    {
       // Thread.SetData(Thread.GetNamedDataSlot(ThreadCodeTaskID), this);
       // Thread.SetData(Thread.GetNamedDataSlot(ThreadID), thread.ManagedThreadId);
        try
        {
            await CSharpScript.EvaluateAsync(this.rawCode, CodeRunner.instance.scriptOptions);
        }
        catch (ThreadAbortException tae)
        {
            //todo add better logging
            Debug.LogWarning("Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            Destroy();
        }
        Destroy();
        
    }
    ~CodeTask()
    {
        Destroy();
    }
    public void Destroy()
    {
        Debug.LogWarning("Destroying CodeTask");
        CodeRunner.instance.RemoveCodeTask(this);
        if (thread != null)
        {
            //  thread.Interrupt();
            thread.Abort();
            thread = null;

        }
        else
        {
            //todo add better loging
            Debug.LogWarning("thread was null");

        }
    }
   
}
