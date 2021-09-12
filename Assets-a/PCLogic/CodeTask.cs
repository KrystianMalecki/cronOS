using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using NaughtyAttributes;
using UnityEngine;


[Serializable]
public class CodeTask
{
    public Task task = null;

    [ResizableTextArea] public string rawCode;


    public static object RunMainFunction(Func<object> action)
    {
        MainThreadFunction mtf = new MainThreadFunction(action);
        CodeRunner.AddToStack(mtf);
        
        return mtf.WaitForAction();
    }

    public static object RunMainFunction(Action action)
    {
        return RunMainFunction(() =>
        {
            action.Invoke();
            return null;
        });
    }

    public async void RunCode(String rawCode)
    {
        this.rawCode = rawCode;
        try
        {
            task = CSharpScript.EvaluateAsync(this.rawCode, CodeRunner.scriptOptions,
                globals: new Globals() { currentCodeTask = this }
            );
            await task;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}