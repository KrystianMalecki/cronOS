using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cinemachine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using UnityEngine;

[SaveDuringPlay]
public class CodeRunner : MonoBehaviour
{
    public static readonly ScriptOptions scriptOptions = ScriptOptions.Default;

    static CodeRunner()
    {
        Assembly mscorlib = typeof(System.Object).GetTypeInfo().Assembly;
        Assembly engine = typeof(UnityEngine.Transform).GetTypeInfo().Assembly;
        Assembly editor = typeof(UnityEditor.ArrayUtility).GetTypeInfo().Assembly;
        Assembly screen = typeof(c_System.console).GetTypeInfo().Assembly;
        Assembly systemCore = typeof(System.Linq.Enumerable).GetTypeInfo().Assembly;


        Assembly[] references = new[] { mscorlib, systemCore, engine, editor, screen };
        scriptOptions = scriptOptions.AddReferences(references);
        scriptOptions = scriptOptions.AddImports(
            //   typeof(UnityEngine.Transform).Assembly.GetType(),
            "System",
            // "UnityEngine",
            "c_System"
        );
    }


    public static CodeRunner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [ResizableTextArea] public string code;
    public List<CodeTask> codeTasks = new List<CodeTask>();
    private Queue<MainThreadFunction> actionStack = new Queue<MainThreadFunction>();

    [Button("Run code")]
    void RunCode()
    {
        CodeTask codeTask = new CodeTask();
        codeTask.RunCode(CodeParser(code));
        codeTasks.Add(codeTask);
    }

    public string CodeParser(string code)
    {
        return
            "console console = new console();console.init(currentCodeTask);\n" +
            code;
        
    }

    public void Update()
    {
        ExecuteFromStack();
    }

    private void ExecuteFromStack(int count = -1)
    {
        for (int i = 0; i < count; i++)
        {
            if (actionStack.Count > 0)
            {
                actionStack.Dequeue().Run();
            }
            else
            {
                return;
            }
        }
    }

    public static void AddToStack(MainThreadFunction function)
    {
        instance.actionStack.Enqueue(function);
    }

    public void OnDestroy()
    {
        Debug.Log("destryoying");
        foreach (CodeTask ct in codeTasks)
        {
            ct.task.Dispose();
        }
    }
}