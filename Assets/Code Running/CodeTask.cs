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
    //  public static readonly string ThreadCodeTaskID = "codeTask";
    //  public static readonly string ThreadID = "threadID";


    public Thread thread;
    public CodeObject codeObject;

    public void RunCode(CodeObject codeObject)
    {
        this.codeObject = codeObject;
        thread = new Thread(RunAsync);

        thread.IsBackground = true;




        thread.Start();
    }
    private async void RunAsync()
    {
        try
        {
            //todo-maybe change back
            /* var s = CSharpScript.Create(codeObject.code, ScriptManager.instance.scriptOptionsBuffer
                .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))
                );
             var s2 = s.CreateDelegate();
             await s2.Invoke();
             */
            await CSharpScript.EvaluateAsync(codeObject.code, ScriptManager.instance.scriptOptionsBuffer
               .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
               .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))
               );
          
        }
        catch (ThreadAbortException tae)
        {
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

        Destroy();
    }
    public void Destroy()
    {
        FlagLogger.LogWarning(LogFlags.SystemWarning, "Destroying CodeTask");
        ScriptManager.instance.RemoveCodeTask(this);
        if (thread != null)
        {
            thread.Abort();
            thread.Join();//todo-maybe fix
            thread = null;
          
        }
        else
        {
            FlagLogger.LogWarning(LogFlags.SystemWarning, "thread was null");

        }
    }

}
