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
using Libraries.system;

[Serializable]
public class CodeTask
{
    //  public static readonly string ThreadCodeTaskID = "codeTask";
    //  public static readonly string ThreadID = "threadID";


    public Thread thread;
    public CodeObject codeObject;
    private static FieldInfo fieldInfoOfStackTrace = typeof(Exception).GetField("captured_traces", BindingFlags.NonPublic | BindingFlags.Instance);

    public CodeTask()
    {
        // Debug.LogError("Creation of code task");
    }
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


            await CSharpScript.EvaluateAsync(codeObject.code
                 , ScriptManager.instance.scriptOptionsBuffer
                .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))
                .WithFilePath("debugpath/")

                );


        }
        catch (ThreadAbortException tae)
        {
            Debug.LogWarning("Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception e)
        {

            try
            {


                string line = "";
                string file = "";
                int linePos = 0;
                int columnPos = 0;

                object field = fieldInfoOfStackTrace.GetValue(e);
                string reason = "";
                try
                {
                    string s = e.StackTrace;
                    (linePos, columnPos) = GetLineAndColumnFromExceptionMessage(e.Message);
                    reason = e.Message;
                    file = e.Source;

                    Debug.Log("NormalException");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.StackFrame frame = ((System.Diagnostics.StackTrace[])fieldInfoOfStackTrace.GetValue(e))[0].GetFrame(0);
                    linePos = frame.GetFileLineNumber();
                    columnPos = frame.GetFileColumnNumber();
                    file = frame.GetFileName();
                    reason = e.Message;
                    Debug.Log("CheatedException");
                }
                try
                {
                    string[] lines = codeObject.code.Split('\n');

                    line = lines[linePos - 1] + "\n" + lines[linePos] + "\n" + lines[linePos + 1] + "\n" + lines[linePos + 2];
                }
                catch (Exception ex)
                { }
                Debug.Log($"{e.GetType()}\n{file}\n line:{linePos} column:{columnPos}\nline: {line} \n reason:{reason}");

            }
            catch (Exception ee)
            {
                Debug.LogException(ee);
            }
        }
        Debug.Log("end");


    }
    (int line, int column) GetLineAndColumnFromExceptionMessage(string message)
    {
        int line = 0;
        int column = 0;
        int commaLocation = message.IndexOf(",");


        line = int.Parse(
            message.Substring(message.IndexOf("(") + 1, commaLocation - message.IndexOf("(") - 1)
            );
        column = int.Parse(
            message.Substring(commaLocation + 1, message.IndexOf(")") - commaLocation - 1)
            );


        return (line, column);
    }
    ~CodeTask()
    {

        Destroy();
    }
    public void Destroy()
    {
        Debug.LogWarning("Destroying CodeTask");
        ScriptManager.instance.RemoveCodeTask(this);
        if (thread != null)
        {
            thread.Abort();
            thread.Join();//todo-maybe fix
            thread = null;

        }
        else
        {
            Debug.LogWarning("thread was null");

        }
    }

}
