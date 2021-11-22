using InternalLogger;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Collections.Concurrent;
using Libraries.system.file_system;
using System.Linq;
using System.Text.RegularExpressions;

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
    private const string LINE_NUMBER_PREPROCESSOR_CODE = "__LINE__";
    private const string FILE_NUMBER_PREPROCESSOR_CODE = "__FILE__";

    private const bool ONLY_UPPERCASE_REDEFINE = false;
    public ScriptOptions scriptOptionsBuffer = null;

    public static readonly List<Type> allLibraries = new List<Type>() { typeof(Libraries.system.output.Console),typeof(Libraries.system.Runtime),
    typeof(Libraries.system.output.graphics.Screen),
    typeof(Libraries.system.output.graphics.texture32.Texture32),  typeof(Libraries.system.output.graphics.system_texture.SystemTexture),
    typeof(Libraries.system.output.graphics.color32.Color32),  typeof(Libraries.system.output.graphics.system_colorspace.ColorConstants),
    typeof(Libraries.system.output.graphics.screen_buffer32.ScreenBuffer32),  typeof(Libraries.system.output.graphics.system_screen_buffer.SystemScreenBuffer),
    typeof(Libraries.system.file_system.File),typeof(Libraries.system.shell.IShellProgram),
        typeof(Libraries.system.mathematics.Vector2),
            typeof(Libraries.system.input.KeyHandler),
                        typeof(System.Collections.Generic.Dictionary<string,string>),
            typeof(System.Linq.Enumerable),typeof(helper.GlobalHelper),



    };
    public static readonly List<LibraryData> allLibraryDatas = allLibraries.ConvertAll(x => x.ToLibraryData());

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
        scriptOptionsBuffer = ScriptOptions.Default/*.WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Release)*/;

        scriptOptionsBuffer = scriptOptionsBuffer.AddReferences(
             typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly
            //,typeof(System.Exception).GetTypeInfo().Assembly


            );

        scriptOptionsBuffer = scriptOptionsBuffer.AddImports(
          // "UnityEngine", "System"
          );


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
                    Debug.Log($"{e.StackTrace}");

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
    static Regex includeRegex = new Regex("\\s*#\\s*include\\s*\".*\"\\s*;*");
    static Regex includeRegex2 = new Regex("\\s*#\\s*include\\s*\".*\"\\s*;*");

    static Regex redefineRegex = new Regex("\\s*#\\s*redefine\\s*.*\\s*.*;*");



    public static string CodeParser(string code)
    {
        List<string> lines = new List<string>(code.Split('\n'));

        List<string> importedLibraries = new List<string>();
        int positionToExpectNextInlcude = 0;
        bool checkIncludes = true;
        bool checkRedefines = true;

        for (int index = 0; index < lines.Count; index++)
        {
            string buffer = lines[index];
            if ((positionToExpectNextInlcude == index && !includeRegex.IsMatch(buffer)))
            {
                if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || buffer.StartsWith("//") || buffer.StartsWith("/*"))
                {
                    positionToExpectNextInlcude++;
                }
                checkIncludes = false;
            }

            if (checkIncludes)
            {
                if (includeRegex.IsMatch(buffer))
                {
                    Debug.Log("found");

                    string between = buffer.GetRangeBetweenFirstLast("\"");
                    Debug.Log(between);
                    if (string.IsNullOrEmpty(between))
                    {
                        between = buffer.GetRangeBetweenFirstLast("\'");
                    }
                    if (!importedLibraries.Contains(between))
                    {
                        importedLibraries.Add(between);
                        File f = FileSystemInternal.instance.drive.GetFileByPath(between);
                        if (f == null)
                        {
                            lines[index] = $"//There would be imported \"{between}\" but it couldn't be found!";
                            continue;
                        }
                        List<string> importedLines = new List<string>(f.data.ToEncodedString().Split('\n'));
                        lines[index] = "//" + buffer;
                        for (int iL = 0; iL < importedLines.Count; iL++)
                        {
                            string importedLine = importedLines[iL];
                            lines.Insert(index + iL, importedLine);

                        }
                        positionToExpectNextInlcude = index + importedLines.Count;
                        index--;
                    }
                    else
                    {
                        lines[index] = $"//There would be imported \"{between}\" but it was already imported!";

                        //todo-future add compilation error
                    }


                }
            }
            if (checkRedefines)
            {
                if (redefineRegex.IsMatch(buffer))
                {


                    //  string definition = buffer.GetRangeBetweenFirstNext(" ");
                    string[] lineParts = buffer.Split(' ');
                    string definition = lineParts[1];
                    if (ONLY_UPPERCASE_REDEFINE)
                    {
                        if (definition != definition.ToUpper())
                        {
                            //todo-future throw error
                            continue;
                        }
                    }
                    //  int indexOfFirst = buffer.IndexOf(" ") + 1;
                    //  int indexOfSecond = buffer.IndexOf(" ", indexOfFirst) + 1;
                    Debug.Log(lineParts.GetValuesToString());

                    string definitionReplacor = string.Join(" ", lineParts.Skip(2).Take(lineParts.Length - 2).ToArray());

                    // string definitionReplacor = buffer.Substring(indexOfSecond + 1);
                    Debug.Log(/*$"indexOfFirst{indexOfFirst}' indexOfSecond{indexOfSecond}'*/$"  definition'{definition}' definitionReplacor'{definitionReplacor}'");
                    if (definition == null || definitionReplacor == null)
                    {

                    }
                    lines[index] = "//" + buffer.Substring(1);
                    for (int i = index + 1; i < lines.Count; i++)
                    {
                        Debug.Log($"line:{lines[i]}. replace {definition} for {definitionReplacor} so now it looks: { lines[i].Replace(definition, definitionReplacor)}");

                        lines[i] = lines[i].Replace(definition, definitionReplacor).Replace(LINE_NUMBER_PREPROCESSOR_CODE, (i + 1).ToString()).Replace(FILE_NUMBER_PREPROCESSOR_CODE, "unknown");//todo-future change unknown
                    }
                }
            }
        }
        Debug.Log(lines.GetValuesToString("\n"));
        return string.Join("\n", lines);
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
