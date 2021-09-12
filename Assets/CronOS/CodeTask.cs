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
    // public Task task = null;
    // public Task codeTask = null;
    public Thread thread;
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
    public void RunCode(string rawCode)
    {
        this.rawCode = rawCode;

        // task = Task.Run(RunAsync);
        thread = new Thread(RunAsync);
        thread.Start();
    }
    private void RunAsync()
    {
        /*  using (task)
          {*/
        try
        {
            /* CancellationTokenSource cts = new CancellationTokenSource();
             CancellationToken ct = cts.Token;
             var v = CSharpScript.Create(this.rawCode, CodeRunner.instance.scriptOptions
              /* globals: new Globals() { }, cancellationToken: CodeRunner.instance.ctoken*/
            /* );
            var origTree = CSharpSyntaxTree.ParseText(this.rawCode);
            //  origTree.GetRoot().DescendantNodes().ToList()[0].Kind==SyntaxKind
            var root = origTree.GetRoot();
            int position = 0;
            Microsoft.CodeAnalysis.Text.TextSpan spaner = root.FullSpan;
            SyntaxNode sn = null;
            do
            {
                sn = root.FindNode(span: spaner);
            } while (sn != null);
            //    trees[0].GetRoot().GetText;
            Debug.Log(trees[0].GetText().Lines.Count);
            trees.ForEach(x => Debug.Log(x.GetRoot().GetText()));*
            //            codeTask = v.RunAsync(/*globals: new Globals() { },*//* cancellationToken: ct);*/
            //  cts.Cancel();
            //  await codeTask;
            CSharpScript.EvaluateAsync(this.rawCode, CodeRunner.instance.scriptOptions
              /* globals: new Globals() { }, cancellationToken: CodeRunner.instance.ctoken*/);

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

        // }
        // flag: task
        //   CodeRunner.instance.tokenSource.Cancel();
        Destroy();
    }
    public void Destroy()
    {
        // flag: task
        //  Debug.Log("CodeRunner.instance.ctoken" + CodeRunner.instance.ctoken.IsCancellationRequested);
        //  Debug.Log("task.IsCanceled " + task.IsCanceled);
        //    Debug.Log("codeTask.IsCanceled " + codeTask.IsCanceled);

        CodeRunner.instance.codeTasks.Remove(this);
        //  task.Dispose();
        if (thread != null)
        {
            thread.Abort();
        }
        else
        {
            //todo add better loging
            Debug.LogWarning("thread was null");

        }
    }


}
