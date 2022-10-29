using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class Compilation
{
    [SerializeField]
    private Script<object> compiledCode;
    //todo 5 reimplement some returns
    public void RunCode(Hardware hardware)
    {
        Debug.Log("start code");
        try
        {
            compiledCode.RunAsync(hardware, catchException: HandleException);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        Debug.Log("end code");

    }
    bool HandleException(Exception exception)
    {
        //todo make handler
        Debug.Log(exception);
        return true;
    }
    public async void RunCodeAsync(Hardware hardware)
    {
        await compiledCode.RunAsync(hardware);
    }
    public Compilation(Script<object> script)
    {
        compiledCode = script;

    }
}
