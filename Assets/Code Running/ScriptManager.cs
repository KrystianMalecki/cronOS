using InternalLogger;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Collections.Concurrent;
public class ScriptManager : MonoBehaviour
{
    #region singleton logic
    public static ScriptManager instance;
    public void Awake()
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
    }
    #endregion
    public ScriptOptions scriptOptionsBuffer = null;

    public static readonly List<Type> allLibraries = new List<Type>() { typeof(Libraries.system.Console),
    typeof(Libraries.system.graphics.Screen),
    typeof(Libraries.system.graphics.texture32.Texture32),  typeof(Libraries.system.graphics.system_texture.SystemTexture),
    typeof(Libraries.system.graphics.color32.Color32),  typeof(Libraries.system.graphics.system_color.ColorConstants),
    typeof(Libraries.system.graphics.screen_buffer32.ScreenBuffer32),  typeof(Libraries.system.graphics.system_screen_buffer.SystemScreenBuffer),
    typeof(Libraries.system.filesystem.File), typeof(Libraries.system.math.Vector2)
    };


    [SerializeField]
    private ConcurrentQueue<ITryToRun> actionQueue = new ConcurrentQueue<ITryToRun>();
    [SerializeField]

    public List<CodeTask> scriptsRunning = new List<CodeTask>();

    public void TryToInitScriptOptions()
    {
        if (scriptOptionsBuffer != null)
        {
            return;
        }
        scriptOptionsBuffer = ScriptOptions.Default.WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Debug);

        scriptOptionsBuffer = scriptOptionsBuffer.AddReferences(
             typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly

            );

        /*   scriptOptions = scriptOptions.AddImports(
               "UnityEngine"
          );*/


    }
    public void RemoveCodeTask(CodeTask codeTask)
    {
        scriptsRunning.Remove(codeTask);
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

        foreach (CodeTask ct in scriptsRunning)
        {
            Debug.Log("thread null?" + (ct.thread == null ? "yes" : "no") + ct.thread);
        }
    }
    public void Update()
    {
        ExecuteFromQueue();
    }

    [Button("Kill All")]

    public void KillAll()
    {
        FlagLogger.Log(LogFlags.DebugInfo, "Kill All");
        for (int i = 0; i < scriptsRunning.Count; i++)
        {
            CodeTask ct = scriptsRunning[i];
            if (ct != null)
            {
                ct.Destroy();
            }
            else
            {
                scriptsRunning.Remove(ct);
            }
            i--;
        }

        //  scriptOptionsBuffer = null;

        GC.Collect();


    }
    public void OnDestroy()
    {
        KillAll();
    }
    public void OnApplicationQuit()
    {
        KillAll();
    }
    private ITryToRun _delegateBuffer = null;
    private int _maxTasksBuffer;
    private void ExecuteFromQueue()
    {
        _maxTasksBuffer = ProcessorManager.instance.TasksPerCPULoop == -1 ? actionQueue.Count : ProcessorManager.instance.TasksPerCPULoop;
        for (int i = 0; (i < _maxTasksBuffer); i++)
        {
            if (actionQueue.Count > 0)
            {
                try
                {
                    if (actionQueue.TryDequeue(out _delegateBuffer))
                    {

                        if (_delegateBuffer != null && !_delegateBuffer.TryToRun())
                        {
                            actionQueue.Enqueue(_delegateBuffer);
                        }
                        else
                        {
                            FlagLogger.Log(LogFlags.DebugInfo, "killing null:");
                            _delegateBuffer.Speak();
                        }
                    }
                    _delegateBuffer = null;
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
    }
    public void RunCode(CodeObject codeObject)
    {
        FlagLogger.Log(LogFlags.DebugInfo, "Making new codeTask");
        CodeTask codeTask = new CodeTask();
        scriptsRunning.Add(codeTask);
        codeObject.code = CodeParser(codeObject.code);
        codeTask.RunCode(codeObject);
        FlagLogger.Log(LogFlags.DebugInfo, "After running code");


    }
    public static string CodeParser(string code)
    {
        //todo future add special usings
        //using native_system = System;
        //using native_ue = UnityEngine;
        //not needed??
        return
            //  "console console = new console();console.init(currentCodeTask);\n" +
            code;
    }
    public static T AddDelegateToStack<T>(MainThreadDelegate<T>.MTDFunction action, bool sync = true)
    {

        return AddDelegateToStack(new MainThreadDelegate<T>(action), sync);

    }
    public static T AddDelegateToStack<T>(MainThreadDelegate<T> mtf, bool sync = true)
    {
        instance.actionQueue.Enqueue(mtf);
        if (sync)
        {
            return mtf.WaitForReturn();
        }
        return default(T);
    }
    public static void AddDelegateToStack(Action action, bool sync = true)
    {
        AddDelegateToStack((ref bool done, ref object returnValue) =>
        {
            action.Invoke();
        }, sync);
    }
}
