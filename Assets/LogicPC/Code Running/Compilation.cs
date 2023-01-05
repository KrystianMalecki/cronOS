using Helpers;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class Compilation
{
    [SerializeField]
    private Script<object> compiledCode;
    [SerializeField]
    public string mainFunctionRun;
    public bool mainFunctionHasArgsRun;
    public Task<ScriptState<object>> task;
    //todo 5 reimplement some returns
    public object RunCode(Hardware hardware, string[] args)
    {
        Debug.Log("start code");
        // ScriptState<object> scriptState = null;
        try
        {
            Debug.Log($"running by:{ArgsToMainLine(args)}");
            task = compiledCode.RunAsync(hardware, catchException: HandleException).Result.ContinueWithAsync(ArgsToMainLine(args), Hardware.currentThreadInstance.hardwareInternal.scriptOptionsBuffer);
            Debug.Log("after compleated task");
            //   scriptState=task.Re;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            Hardware.currentThreadInstance.stackExecutor.AddDelegateToStack(x =>
            {
                GlobalDebugger.instance.WriteToDebugFileWrapedInIf(compiledCode.Code);
            });
        }
        task.Dispose();
        Debug.Log("end code");
        return null;// scriptState?.ReturnValue;

    }
    public string ArgsToMainLine(string[] args)
    {
        //make string builder in c#

        string argsJoined = "";

        if (!mainFunctionHasArgsRun)
        {
            argsJoined = "";
        }
        else
        {
            if (args != null)
            {
                argsJoined = args.ToConvertedString(", ", x => $"\"{x}\"");
            }
            if (string.IsNullOrWhiteSpace(argsJoined))
            {
                argsJoined = "";
            }
        }
        return mainFunctionRun + "(" + argsJoined + ")";
    }
    bool HandleException(Exception exception)
    {
        //todo make handler
        Debug.Log(exception);
        return true;
    }
    //todo 1 fix both
    public async void RunCodeAsync(Hardware hardware, string[] args = null)
    {
        //todo 1 test
        await compiledCode.RunAsync(hardware);
    }
    public object RunCodeAsyncButReturn(Hardware hardware, string[] args = null)
    {
        return Task.Run(() => { return RunCode(hardware, args); }).Result;
    }
    public Compilation(Script<object> script, string mainFunctionRun, bool mainFunctionHasArgsRun)
    {
        compiledCode = script;
        this.mainFunctionRun = mainFunctionRun;
        this.mainFunctionHasArgsRun = mainFunctionHasArgsRun;
    }
    ~Compilation()
    {
        compiledCode = null;
        mainFunctionRun = null;
        mainFunctionHasArgsRun = false;
    }

}
