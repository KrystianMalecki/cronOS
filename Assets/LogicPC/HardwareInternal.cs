using Helpers;
using Libraries.system;
using Libraries.system.file_system;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

[Serializable]
public class FileCompilationDictionary : SerializableDictionary<File, Compilation> { }

[Serializable]
public class HardwareInternal
{
    internal Hardware hardware;


    [SerializeField] internal DriveSO mainDrive;
    [SerializeField] internal InputManager inputManager;
    [SerializeField] internal ScreenManager screenManager;
    [SerializeField] internal StackExecutor stackExecutor;
    [SerializeField] internal AudioManager audioManager;
    public long CurrentMilliseconds => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;


    //todo maybe some events for focusting and unfocusting
    public bool focused = false;


    internal void Init()
    {

        mainDrive.GenerateCacheData();
        inputManager.hardwareInternal = this;
        stackExecutor.hardwareInternal = this;
        //  scriptAppDomain = AppDomain.CreateDomain("ScriptDomain");

        hardware = new Hardware(this);
        hardware.Init();

    }
    public void LoadAsseblies()
    {

        HashSet<String> assemblies = allLibraries.ConvertAll(x => x.GetTypeInfo().Assembly.GetName().FullName.ToString()).ToHashSet();
        foreach (var assembly in assemblies)
        {
            scriptAppDomain.Load(assembly);
        }
    }
    #region Processor

    public Thread mainPCThread;


    //  [SerializeField]
    [AllowNesting, OnValueChanged("UpdateWRR")]
    public float FPSCap = 100;

    //  [SerializeField]
    [AllowNesting, OnValueChanged("UpdateFPSC")]
    public int WaitRefreshRate = 100;

    [SerializeField] internal int TasksPerCPULoop = -1;
    public static readonly Encoding mainEncoding = Encoding.GetEncoding("437");

    private void UpdateWRR()
    {
        FPSCap = Math.Clamp(FPSCap, 0.1f, 1000f);
        WaitRefreshRate = (int)((1f / FPSCap) * 1000f);
        Debug.Log($"WaitRefreshRate:{WaitRefreshRate} FPSCap:{FPSCap}");
    }

    private void UpdateFPSC()
    {
        WaitRefreshRate = Math.Clamp(WaitRefreshRate, 1, 10000);
        FPSCap = ((1f / WaitRefreshRate) * 1000f);
        Debug.Log($"WaitRefreshRate:{WaitRefreshRate} FPSCap:{FPSCap}");
    }

    #endregion

    #region Running pc
    public void SystemInit()
    {

        mainPCThread = new Thread(StartUp);
        // runningThreads.Add(mainPCThread);
        mainPCThread.IsBackground = false;

        mainPCThread.Start();

    }

    /*
     <summary>
    This is run in pc thread. Do not run it alone.
    </summary>
     */
    private async void StartUp()
    {
        hardware.SetCurrentHardwareInstance();

        File boot = mainDrive.drive.GetFileByPath("/sys/boot.ini");
        var data = StaticHelper.ParseTextAsObjectKeyValue(Runtime.BytesToEncodedString(boot.data));
        if (data.TryGetValue("shell", out string shellPath))
        {
            File shell = mainDrive.drive.GetFileByPath(shellPath);
            File compiledShell = Compile(shell);
            Debug.Log(shell.GetFullPath());
            Debug.Log(Runtime.BytesToEncodedString(compiledShell.data));

            Execute(compiledShell);
        }
        else
        {

        }

    }
    #endregion

    #region Script running management

    #region static data

    private const string LINE_NUMBER_PREPROCESSOR_CODE = "__LINE__";
    private const string FILE_NAME_PREPROCESSOR_CODE = "__FILE__";

    private const bool ONLY_UPPERCASE_REDEFINE = false;

    public readonly ScriptOptions scriptOptionsBuffer = ScriptOptions.Default/*.WithReferences(
            typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly,
            typeof(UnityEngine.Vector2).GetTypeInfo().Assembly*/
        /*,
            

        typeof(System.Text.RegularExpressions.Regex).GetTypeInfo().Assembly,
             typeof(helper.GlobalHelper).GetTypeInfo().Assembly,
             typeof(System.Collections.Generic.Dictionary<int,int>).GetTypeInfo().Assembly
            //,typeof(System.Exception).GetTypeInfo().Assembly
*/
        /* )*//*.WithImports(
              "UnityEngine"
         )*/

        .WithEmitDebugInformation(true)
        .WithFileEncoding(HardwareInternal.mainEncoding);

    public static readonly List<Type> allLibraries = new List<Type>()
    {
        //   typeof(Libraries.system.output.Console),
        typeof(Libraries.system.Runtime),
        typeof(Libraries.system.output.graphics.Screen),
        typeof(Libraries.system.output.graphics.texture32.Texture32),
        typeof(Libraries.system.output.graphics.system_texture.SystemTexture),
        typeof(Libraries.system.output.graphics.color32.Color32),
        typeof(Libraries.system.output.graphics.system_colorspace.ColorConstants),
        typeof(Libraries.system.output.graphics.screen_buffer32.ScreenBuffer32),
        typeof(Libraries.system.output.graphics.system_screen_buffer.SystemScreenBuffer),
        typeof(Libraries.system.file_system.File),
        typeof(Libraries.system.mathematics.Vector2),
        typeof(Libraries.system.input.KeyHandler),
        typeof(Libraries.system.input.MouseHandler),
        typeof(Libraries.system.debug.Debugger),
        typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo),
        typeof(System.String),
        typeof(System.ComponentModel.AddingNewEventArgs),
        

        //    typeof(attic.StaticValueHaver),

        typeof(Libraries.system.output.graphics.mask_texture.MaskTexture),
        //typeof(System.Math),
        typeof(System.Text.RegularExpressions.Regex),
        typeof(Helpers.GlobalHelper),
        typeof(System.Collections.Generic.Dictionary<int, int>),
        typeof(Libraries.system.output.music.Sound),
        typeof(System.Linq.Enumerable),
                /*,
        typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)*/
    };

    public static readonly List<LibraryData> allLibraryDatas = allLibraries.ConvertAll(x => new LibraryData(x));

    #endregion



    #region Script running

    public AppDomain scriptAppDomain = null;

    //  private ThreadSafeList<Thread> runningThreads = new ThreadSafeList<Thread>();






    internal void KillAll()
    {

        Debug.Log("killing all!");
        mainPCThread?.Interrupt();
        mainPCThread?.Abort();
        compiledScripts?.Clear();
        compiledScripts = null;
        // AppDomain.Unload(scriptAppDomain);
        //todo 2 null all other important data


        /*  compiledScripts?.Clear();
          compiledScripts = null;*/
        //  mainPCThread.Abort();

        //  scriptOptionsBuffer = null;
        GC.Collect(); // collects all unused memory
        GC.WaitForPendingFinalizers(); // wait until GC has finished its work
        GC.Collect();
    }



    #endregion



    #region Script compilation
    public const string compilationExtension = ".cse";

    [SerializeField] internal FileCompilationDictionary compiledScripts = new FileCompilationDictionary();


    private readonly static FieldInfo fieldInfoOfStackTrace =
       typeof(Exception).GetField("captured_traces", BindingFlags.NonPublic | BindingFlags.Instance);

    public Compilation GetCompilation(File file)
    {
        if (file == null)
        {
            return null;
        }
        if (compiledScripts.TryGetValue(file, out Compilation compilation))
        {
            return compilation;
        }
        else
        {
            //todo 6 throw error, maybe a start for custom system errors?
            return null;
        }
    }
    public bool Execute(File compiledFile, string[] args = default)
    {
        Compilation compilation = GetCompilation(compiledFile);
        if (compilation == null)
        {
            return false;
        }
        return compilation.RunCode(hardware, args);

    }
    public void ExecuteAsync(File compiledFile, string[] args = default)
    {
        //todo 1 test
        Compilation compilation = GetCompilation(compiledFile);
        compilation?.RunCodeAsync(hardware, args);
    }

    internal File Compile(File file)
    {
        //todo 4 implement only allowing some libraries, change then "allLibraries"
        CodeObject codeObject = new CodeObject(Runtime.BytesToEncodedString(file.data), allLibraries);
        CodeParser(ref codeObject);
        var script = CompileCodeObject(codeObject);
        if (script == null)
        {
            return null;
            //todo 9 throw error
        }
        //todo 2 check if this is required

        script.Compile();
        var comp = script.GetCompilation();
        ClassDeclarationSyntax mainClass = null;
        string mainClassName = null;
        bool mainFunctionHasArgsRun = false;

        var classes = comp.SyntaxTrees.SelectMany(t => t.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());
        foreach (var clazz in classes)
        {
            if (mainClass != null)
            {
                break;
            }
            foreach (var members in clazz.Members)
            {
                if (members is MethodDeclarationSyntax method)
                {
                    if (method.Identifier.Text == "Main" && method.Modifiers.Any(x => x.Text == "static"))
                    {
                        if (method.ParameterList.Parameters.Count == 0)
                        {
                            mainClass = clazz;
                            mainClassName = clazz.Identifier.Text + ".Main";
                            mainFunctionHasArgsRun = false;
                            break;
                        }
                        else if (method.ParameterList.Parameters.Count == 1 &&
                          method.ParameterList.Parameters[0].Type.ToString() == "string[]")
                        {
                            mainClass = clazz;
                            mainClassName = clazz.Identifier.Text + ".Main";
                            mainFunctionHasArgsRun = true;
                            break;
                        }
                    }
                }
            }
        }


        File compiledFile = Drive.MakeFile(file.name + compilationExtension, Runtime.StringToEncodedBytes("*compiled code here*"));

        compiledScripts.Add(compiledFile, new Compilation(script, mainClassName, mainFunctionHasArgsRun));
        return compiledFile;
    }


    internal Script<object> CompileCodeObject(CodeObject codeObject)
    {
        //todo 6 think if catching compilation errors so hard is good idea AND/OR opimized, you should be catching runtime errors in compilation ALSO
        try
        {
            var script = CSharpScript.Create(codeObject.code
                 , scriptOptionsBuffer
                        .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                        .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))

#if UNITY_EDITOR
                     .WithFilePath(GlobalDebugger.assetPath)
#endif
                     , hardware.GetType()
             );

            return script;

        }
        catch (ThreadAbortException tae)
        {
            Debug.LogWarning("Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception cSharpScriptCallException)
        {
            Debug.Log("cSharpScriptCallException" + cSharpScriptCallException);
            try
            {
                string line = "";
                string file = "";
                int linePos = 0;
                int columnPos = 0;

                object field = fieldInfoOfStackTrace.GetValue(cSharpScriptCallException);
                string reason = "";
                try
                {
                    string s = cSharpScriptCallException.StackTrace;
                    (linePos, columnPos) = GetLineAndColumnFromExceptionMessage(cSharpScriptCallException.Message);
                    reason = cSharpScriptCallException.Message;
                    file = cSharpScriptCallException.Source;

                    Debug.Log("NormalException");
                }
                catch (Exception getLineAndColumnFromExceptionMessageCallException)
                {
                    Debug.Log("getLineAndColumnFromExceptionMessageCallException" + getLineAndColumnFromExceptionMessageCallException);
                    var p1 = fieldInfoOfStackTrace;
                    var p2 = p1.GetValue(cSharpScriptCallException);
                    var p3 = ((System.Diagnostics.StackTrace[])p2);
                    var p4 = p3[0];
                    var p5 = p4.GetFrame(0);

                    System.Diagnostics.StackFrame frame =
                        ((System.Diagnostics.StackTrace[])fieldInfoOfStackTrace.GetValue(cSharpScriptCallException))[0].GetFrame(0);
                    linePos = frame.GetFileLineNumber();
                    columnPos = frame.GetFileColumnNumber();
                    file = frame.GetFileName();
                    reason = cSharpScriptCallException.Message;
                    Debug.Log("CheatedException");
                }

                try
                {
                    string[] lines = codeObject.code.SplitNewLine();

                    line = lines[linePos - 1] + "\n" + lines[linePos] + "\n" + lines[linePos + 1] + "\n" +
                           lines[linePos + 2];
                }
                catch (Exception arrayException)
                {
                    Debug.Log("arrayException" + arrayException);

                }

                Debug.Log(
                    $"{cSharpScriptCallException.GetType()}\n{file}\n line:{linePos} column:{columnPos}\nline: {line} \n reason:{reason}");
            }
            catch (Exception globalException)
            {
                Debug.LogException(globalException);
            }
        }

        return null;
    }
    static (int line, int column) GetLineAndColumnFromExceptionMessage(string message)
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

    #endregion

    #region code Parsing

    static Regex includeRegex = new Regex("^\\s*?#\\s*?include\\s*?[\"'].*[\"']\\s*?");
    static Regex includeRegex2 = new Regex("^\\s*?#\\s*?include\\s*?[\"'].*[\"']\\s*;*");

    static Regex redefineRegex = new Regex("^\\s*?#\\s*?redefine\\s*?.*\\s*.*");

    static Regex undefineRegex = new Regex("^\\s*?#\\s*?undefine\\s*?.*\\s*");
    static Regex undefineRegexOld = new Regex("^\\s*?#\\s*?undefine\\s*?.*\\s*.*;*");
    static Regex topRegex = new Regex("^\\s*?#\\s*?top\\s*?.*");
    static Regex bottomRegex = new Regex("^\\s*?#\\s*?bottom\\s*?.*");


    internal void CodeParser(ref CodeObject codeObject)
    {
        /* codeObject.code = @"using static HardwareBox;
    public class HardwareBox
    {
     public static Hardware hardware;
    }
    HardwareBox.hardware = ownPointer;
    " + codeObject.code;*/
        int topCounter = 0;
        /* codeObject.code = @"
    static Runtime runtime = null;runtime=ownPointer.runtime;
    static FileSystem fileSystem = null;fileSystem=ownPointer.fileSystem;
    static KeyHandler keyHandler = null;keyHandler=ownPointer.keyHandler;
    static MouseHandler mouseHandler = null;mouseHandler=ownPointer.mouseHandler;
    static Screen screen = null;screen=ownPointer.screen;
    static AudioHandler audioHandler = null;audioHandler=ownPointer.audioHandler;

    " + codeObject.code;*/
        /* codeObject.code = @"Hardware.currentThreadInstance = thisHardware;
    " + codeObject.code;*/
        // codeObject.code = "/*using UVector2 = UnityEngine.Vector2;*/\n#include \"/sys/kernel\"\n" + codeObject.code;
        // Debug.Log("codeObject.code: " + codeObject.code);
        List<string> lines =
            new List<string>(codeObject.code.Replace("#if false //changeToTrue", "#if true").SplitNewLine());

        List<string> importedLibraries = new List<string>();
        Dictionary<string, string> currentRedefines = new Dictionary<string, string>();
        currentRedefines.Add("false//changeToTrue", "true");
        int positionToExpectNextInclude = 0;
        bool checkIncludes = true;
        bool checkRedefines = true;
        bool checkUndefines = true;
        for (int index = 0; index < lines.Count; index++)
        {
            string buffer = lines[index];
            if (currentRedefines.Count > 0)
            {
                List<string> parts = buffer.SplitSpaceQArgs();
                foreach (var item in currentRedefines)
                {
                    // Debug.Log($"line:{buffer}. replace {item.Key} for {item.Value} so now it looks: { buffer.Replace(item.Key, item.Value.replacor)}");
                    // Debug.Log(parts.ToFormatedString());
                    //todo-z no replace args with values. Impossible #redefine "lol($x,$y)" "Console.Debug($x+"--"+$y)";
                    for (int i = 0; i < parts.Count; i++)
                    {
                        string part = parts[i];
                        if (part.Contains(item.Key))
                        {
                        }

                        if (part == item.Key)
                        {
                            string replacor = item.Value;

                            parts[i] = part.Replace(item.Key, replacor)
                                .Replace(LINE_NUMBER_PREPROCESSOR_CODE, (index + 1).ToString())
                                .Replace(FILE_NAME_PREPROCESSOR_CODE, "unknown");
                        }
                    }
                }

                buffer = string.Join("", parts);
            }


            if ((positionToExpectNextInclude < index && !includeRegex.IsMatch(buffer)))
            {
                if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || buffer.StartsWith("//") ||
                    buffer.StartsWith("/*"))
                {
                    positionToExpectNextInclude++;
                }
                else
                {
                    //todo 9: this shouldn't be disabled
                    //  Debug.Log($"next:{positionToExpectNextInlcude} index:{index}");
                    //  checkIncludes = false;
                }
            }

            if (true)
            {
                if (topRegex.IsMatch(buffer))
                {
                    lines[index] = $"//moved '{buffer}' on top";

                    lines.Insert(topCounter, buffer.Substring(buffer.IndexOf("top") + "top".Length));
                    topCounter++;
                    continue;
                }

                if (bottomRegex.IsMatch(buffer))
                {
                    lines[index] = $"//moved '{buffer}' on bottom";

                    lines.Insert(lines.Count, buffer.Substring(buffer.IndexOf("bottom") + "bottom".Length));
                    continue;
                }
            }

            if (checkRedefines)
            {
                if (redefineRegex.IsMatch(buffer))
                {
                    positionToExpectNextInclude++;
                    List<string> lineParts = buffer.SplitSpaceQArgsCustom("#"); // buffer.Split(' ', '(', ')', ',');
                    int definitionIndex =
                        lineParts.FindIndex(x => x != "#" && !string.IsNullOrWhiteSpace(x) && x != "redefine");
                    int definitionEndIndex = lineParts.FindIndex(definitionIndex, x => string.IsNullOrWhiteSpace(x));

                    string definition = string.Join("",
                        lineParts.GetRange(definitionIndex, definitionEndIndex - definitionIndex));

                    if (ONLY_UPPERCASE_REDEFINE)
                    {
                        if (definition != definition.ToUpper())
                        {
                            //todo-future throw error
                            continue;
                        }
                    }

                    string definitionReplacor = string.Join(" ",
                            lineParts.Skip(definitionEndIndex).Take(lineParts.Count - definitionEndIndex).ToArray())
                        .TrimStart();

                    if (definition == null || definitionReplacor == null)
                    {
                        continue;
                    }

                    buffer = "//" + buffer.Substring(1);
                    currentRedefines.Add(definition, definitionReplacor);
                }
            }

            if (checkUndefines)
            {
                if (undefineRegex.IsMatch(buffer))
                {
                    positionToExpectNextInclude++;

                    List<string> lineParts = buffer.SplitSpaceQArgsCustom("#"); // buffer.Split(' ', '(', ')', ',');
                    int definitionIndex =
                        lineParts.FindIndex(x => x != "#" || string.IsNullOrEmpty(x) || x != "undefine");
                    string definition = lineParts[definitionIndex];
                    currentRedefines.Remove(definition);
                }
            }

            if (checkIncludes)
            {
                if (includeRegex.IsMatch(buffer))
                {
                    string between = buffer.GetRangeBetweenFirstLast("\"");
                    if (string.IsNullOrEmpty(between))
                    {
                        between = buffer.GetRangeBetweenFirstLast("\'");
                    }

                    if (!importedLibraries.Contains(between))
                    {
                        importedLibraries.Add(between);
                        File f = mainDrive.drive.GetFileByPath(between);
                        if (f == null)
                        {
                            /*if (null.Contains( between))
                            {
                           //todo 9 think about #include-ing core libraries like system or system.file_system
                            // this would be the best with system variables
                            }*/
                            lines[index] = $"//There would be imported \"{between}\" but it couldn't be found!";
                            continue;
                        }

                        List<string> importedLines =
                            new List<string>(Runtime.BytesToEncodedString(f.data).SplitNewLine());
                        buffer = "//imported: " + between;

                        lines[index] = buffer;
                        for (int iL = 0; iL < importedLines.Count; iL++)
                        {
                            string importedLine = importedLines[iL];
                            lines.Insert(index + iL + 1, importedLine);
                        }

                        positionToExpectNextInclude += importedLines.Count + 1;

                        index--;
                        continue;
                    }
                    else
                    {
                        lines[index] = $"//There would be imported \"{between}\" but it was already imported earlier!";

                        //todo-future add compilation error
                    }
                }
            }

            lines[index] = buffer;
        }

        codeObject.code = string.Join("\n", lines);
    }

    #endregion

    #endregion
}