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
            //todo-maybe change back
            /*   script = CSharpScript.Create(codeObject.code.ToBytes().ToEncodedString(), ScriptManager.instance.scriptOptionsBuffer
                      .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                      .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace)).WithEmitDebugInformation(true)
                   );
               scriptRunner = script.CreateDelegate();
               await scriptRunner.Invoke();*/


            //  CSharpScript.Create()
            /*  codeObject.code = "try{\n" +
                  codeObject.code + "\n}catch(System.Exception e){\n" +
                  "Console.Debug(\"before\");\nConsole.Debug(e);\n}";*/
            Debug.Log(codeObject.code);
            await CSharpScript.EvaluateAsync(codeObject.code
                 , ScriptManager.instance.scriptOptionsBuffer
                .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))
                 .WithEmitDebugInformation(true)
                );
            /*  await CSharpScript.EvaluateAsync(@"string n= null; 
  n.ToString();", ScriptOptions.Default
                .WithEmitDebugInformation(false)
               );*/

        }
        catch (ThreadAbortException tae)
        {
            FlagLogger.LogWarning(LogFlags.SystemWarning, "Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception e)
        {
            // Debug.Log(e);
            //e>base>base>captured_traces>[0]>frames>[0]>this
            try
            {

                //   System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(e);

                /*tring stackIndent = "";
                 for (int i = 0; i < st.FrameCount; i++)
                 {
                     // Note that at this level, there are four
                     // stack frames, one for each method invocation.
                     System.Diagnostics.StackFrame sf = st.GetFrame(i);
                     string combine = "";
                     combine += (stackIndent + " Method: {0}",
                         sf.GetMethod()) + "\n";
                     combine += (stackIndent + " File: {0}",
                         sf.GetFileName()) + "\n";
                     combine += (stackIndent + " Line Number: {0}",
                         sf.GetFileLineNumber()) + "\n";
                     Debug.Log(combine);
                     stackIndent += "  ";
                 }
                 Debug.Log(st.ToString());
                 Debug.Log(st.GetFrames().GetValuesToString());*/
                /* BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;
                 Debug.Log(current.GetType());
                 MemberInfo[] mis = current.GetType().GetMembers(bf);
                 Debug.Log(mis.GetValuesToString());

                 FieldInfo[] fis = current.GetType().GetFields(bf);
                 Debug.Log(fis.GetValuesToString());*/
                int line = 0;
                int column = 0;

                object field = fieldInfoOfStackTrace.GetValue(e);
                string reason = "";
                if (field != null)
                {
                    //  System.Diagnostics.StackTrace[] stack = (System.Diagnostics.StackTrace[])field;
                    //  System.Diagnostics.StackTrace trace = stack[0];
                    System.Diagnostics.StackFrame frame = ((System.Diagnostics.StackTrace[])fieldInfoOfStackTrace.GetValue(e))[0].GetFrame(0);

                    //  Debug.Log(((System.Diagnostics.StackTrace[])fieldInfoOfStackTrace.GetValue(e))[0].GetFrame(0));
                    line = frame.GetFileLineNumber();
                    column = frame.GetFileColumnNumber();

                }
                else
                {
                    (line, column) = GetColumnAndLineFromExceptionMessage(e.Message);
                    reason = e.Message;
                }
                Debug.Log($"{e.GetType()} line:{line} column:{column}\n reason:{reason}");
                /* System.Diagnostics.StackTrace[] stacktrace = fi.GetValue(current) as System.Diagnostics.StackTrace[];
                 for (int i = 0; i < stacktrace.Length; i++)
                 {
                     for (int j = 0; j < stacktrace[i].FrameCount; j++)
                     {
                         System.Diagnostics.StackFrame sf = stacktrace[i].GetFrame(j);
                         Debug.Log(sf);
                     }
                 }
                 Debug.Log(stacktrace.GetValuesToString("\n"));

                 PropertyInfo[] pis = current.GetType().GetProperties(bf);
                 Debug.Log(pis.GetValuesToString());*/







                /*Debug.LogError(e.Message);

                  Debug.LogError(e.StackTrace);

                  Debug.LogError(e.InnerException);
                  Debug.LogError(e.InnerException);*/
            }
            catch (Exception ee)
            {
                Debug.LogException(ee);
            }
        }
        Debug.Log("end");
        //  Destroy();


    }
    (int line, int column) GetColumnAndLineFromExceptionMessage(string message)
    {
        int line = 0;
        int column = 0;
        int commaLocation = message.IndexOf(",");

        /*Debug.Log(
             message.Substring(message.IndexOf("(") + 1, commaLocation - message.IndexOf("(") - 1)
             );
         Debug.Log(
             message.Substring(commaLocation + 1, message.IndexOf(")") - commaLocation - 1)
             );
        */

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
