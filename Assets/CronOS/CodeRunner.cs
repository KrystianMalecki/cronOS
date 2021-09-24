using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using System.Collections;
using InternalLogger;


[SaveDuringPlay]
public class CodeRunner : MonoBehaviour
{

    public int WaitRefreshRate = 100;
    public int TasksPerCPULoop = -1;


    public ScriptOptions scriptOptions = null;
    //todo remove task
    //  public CancellationTokenSource tokenSource = new CancellationTokenSource();
    //   public CancellationToken ctoken;


    public static readonly Type[] allLibraries = new Type[] { typeof(Libraries.system.Console),
    typeof(Libraries.system.graphics.Screen),
    typeof(Libraries.system.graphics.texture32.Texture32),  typeof(Libraries.system.graphics.system_texture.SystemTexture),
        typeof(Libraries.system.graphics.color32.Color32),  typeof(Libraries.system.graphics.system_color.ColorConstants),
                typeof(Libraries.system.graphics.screen_buffer32.ScreenBuffer32),  typeof(Libraries.system.graphics.system_screen_buffer.SystemScreenBuffer),

    };
    public Type[] enabledLibraries = allLibraries;

    public static CodeRunner instance;
    //todo make special customdrawer to get and add code blocks
    [AllowNesting]
    public CodeBasement codeBasement;

    [SerializeField]
    //  public Queue<MainThreadFunction> actionStack = new Queue<MainThreadFunction>();
    private Queue<ITryToRun> actionQueue = new Queue<ITryToRun>();
    private Queue<ITryToRun> actionQueueBuffer = new Queue<ITryToRun>();

    public new string tag;


    [ResizableTextArea] public string code;
    public List<CodeTask> codeTasks = new List<CodeTask>();



    [Button("Add to basement")]
    void AddToBasement()
    {
        codeBasement.codeBlocks.Add(new CodeBlock(tag, code));
    }


    public void TryToInitScriptOptions()
    {
        if (scriptOptions != null)
        {
            return;
        }
        scriptOptions = ScriptOptions.Default.WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Debug);

        Type[] enabledLibraries = allLibraries;

        scriptOptions = scriptOptions.AddReferences(Array.ConvertAll(enabledLibraries, x => x.GetTypeInfo().Assembly));

        scriptOptions = scriptOptions.AddImports(
           Array.ConvertAll(enabledLibraries, x => x.GetTypeInfo().Namespace)



        );
        scriptOptions = scriptOptions.AddReferences(
             typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly
            );

        /*   scriptOptions = scriptOptions.AddImports(
               "UnityEngine"
          );*/


    }
    public void RemoveCodeTask(CodeTask codeTask)
    {
        codeTasks.Remove(codeTask);
    }

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
        TryToInitScriptOptions();
        // StartCoroutine(executeQueue());
    }




    [Button("Run code")]
    void RunCode()
    {
        FlagLogger.Log(LogFlags.Info, "making new codeTask");

        TryToInitScriptOptions();
        CodeTask codeTask = new CodeTask();
        codeTasks.Add(codeTask);
        codeTask.RunCode(CodeParser(code));
        FlagLogger.Log(LogFlags.Info, "after running code");


    }

    [Button("Display stack")]
    void DisplayStack()
    {
        try
        {
            foreach (ITryToRun runTry in actionQueue)
            {
                //   Debug.Log(runTry.function.ToString());
                runTry.Speak();
            }
            if (actionQueue.Count == 0)
            {
                Debug.Log("actionStack is empty");
            }
        }
        catch (Exception)
        {

        }
    }
    [Button("Check Threads")]
    void CheackThreads()
    {

        foreach (CodeTask ct in codeTasks)
        {
            Debug.Log("thread null?" + (ct.thread == null ? "yes" : "no") + ct.thread);




            //Debug.Log("is alive?" + ct.thread.IsAlive);
            //Debug.Log("IsThreadPoolThread?" + ct.thread.IsThreadPoolThread);

        }

    }
    [Button("Collect")]
    void TRYFIX()
    {

        GC.Collect();

    }
    public string CodeParser(string code)
    {
        return
            //  "console console = new console();console.init(currentCodeTask);\n" +
            code;

    }

    public void Update()
    {


        ExecuteFromStack();

        // flag: task
        //  FlagLogger.Log("token source" + tokenSource.IsCancellationRequested);
        //  FlagLogger.Log("token" + ctoken.IsCancellationRequested);
    }
    /*  public void LateUpdate()
      {
          //     ExecuteFromStack();
      }*/
    IEnumerator executeQueue()
    {
        while (true)
        {
            for (int i = 0; (i < TasksPerCPULoop) || (TasksPerCPULoop == -1); i++)
            {
                if (actionQueue.Count > 0)
                {

                    tryBuffer = actionQueue.Dequeue();
                    if (tryBuffer != null && !tryBuffer.TryToRun())//not done!
                    {
                        actionQueue.Enqueue(tryBuffer);

                    }
                    else
                    {
                        Debug.Log("killing null:");

                        tryBuffer.Speak();
                    }
                    yield return null;
                    tryBuffer = null;

                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
    }
    [Button("Wipe")]

    public void Wipe()
    {
        // flag: task
        //  tokenSource.Cancel();



        FlagLogger.Log(LogFlags.Info, "wiping");
        for (int i = 0; i < codeTasks.Count; i++)
        {
            CodeTask ct = codeTasks[i];
            if (ct != null)
            {
                ct.Destroy();
            }
            else
            {
                codeTasks.Remove(ct);
            }
            i--;
        }

        scriptOptions = null;

        GC.Collect();


    }
    public void OnDestroy()
    {
        Wipe();

    }
    public void OnApplicationQuit()
    {
        Wipe();
    }
    // private MainThreadFunction mtfbuffer = null;
    private ITryToRun tryBuffer = null;
    private int buffer;
    private void ExecuteFromStack()
    {
        buffer = TasksPerCPULoop == -1 ? actionQueue.Count : TasksPerCPULoop;
        for (int i = 0; (i < buffer); i++)
        {
            if (actionQueue.Count > 0)
            {
                try
                {
                    tryBuffer = actionQueue.Dequeue();

                    if (tryBuffer != null)
                    {
                        if (!tryBuffer.TryToRun())//not done!
                        {
                            actionQueue.Enqueue(tryBuffer);
                        }
                        else
                        {
                            Debug.Log("killing null:");

                            tryBuffer.Speak();
                        }
                        //
                        //  FlagLogger.Log(LogFlags.DebugInfo, "queueMore");

                        // FlagLogger.Log("nope");
                        // actionStack.Dequeue();
                    }
                    else
                    {
                        Debug.Log("killing null:");

                        tryBuffer.Speak();
                    }
                    tryBuffer = null;
                }
                catch (Exception e)
                {
                    Debug.Log($"{e.Message}");
                }
            }
            else
            {
                break;
            }
        }
        while (actionQueueBuffer.Count > 0)
        {
            actionQueue.Enqueue(actionQueueBuffer.Dequeue());
        }
    }

    /*  private MainThreadFunction AddFunctionToStackInternal(FunctionDelegate action)
      {
          MainThreadFunction mtf = new MainThreadFunction(action);
          actionStack.Enqueue(mtf);

          return mtf;
      }
      public static object AddFunctionToStack(FunctionDelegate action, bool sync = true)
      {

          MainThreadFunction mtf = instance.AddFunctionToStackInternal(action);
          if (sync)
          {
              return mtf.WaitForAction();
          }
          return null;

      }
      public static object AddFunctionToStack(MainThreadFunction mtf, bool sync = true)
      {
          instance.actionStack.Enqueue(mtf);
          if (sync)
          {
              return mtf.WaitForAction();
          }
          return null;
      }*/

    public static T AddFunctionToStack<T>(MainThreadDelegate<T>.MTDFunction action, bool sync = true)
    {

        return AddFunctionToStack(new MainThreadDelegate<T>(action), sync);

    }
    public static T AddFunctionToStack<T>(MainThreadDelegate<T> mtf, bool sync = true)
    {
        instance.actionQueueBuffer.Enqueue(mtf);
        if (sync)
        {
            return mtf.WaitForReturn();
        }
        return default(T);
    }
    public static void AddFunctionToStack(Action action, bool sync = true)
    {
        AddFunctionToStack((ref bool done, ref object returnValue) =>
       {
           action.Invoke();
       }, sync);
    }
    /* public static object AddFunctionToStack(Action action, bool sync = true)
     {
         return AddFunctionToStack((ref bool done, ref object returnValue) =>
         {
             action.Invoke();
         }, sync);
     }*/

}
