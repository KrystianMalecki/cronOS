using UnityEngine;
using Libraries.system.file_system;
using System.Text;
using System;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;
using helper;
using System.Linq;
using System.Text.RegularExpressions;
using Libraries.system;
using NaughtyAttributes;
using UnityEditor;

[Serializable]
public class HardwareInternal
{
    [NonSerialized] internal Hardware hardware;


    [SerializeField] internal DriveSO mainDrive;
    [SerializeField] internal InputManager inputManager;
    [SerializeField] internal ScreenManager screenManager;
    [SerializeField] internal StackExecutor stackExecutor;
    [SerializeField] internal AudioManager audioManager;
    public long CurrentMilliseconds => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

    #region Processor

    //  [SerializeField]
    [AllowNesting, OnValueChanged("UpdateWRR")]
    public float FPSCap = 100;

    //  [SerializeField]
    [AllowNesting, OnValueChanged("UpdateFPSC")]
    public int WaitRefreshRate = 100;

    [SerializeField] internal int TasksPerCPULoop = -1;
    internal static readonly Encoding mainEncoding = Encoding.GetEncoding("437");

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

    #region Script running management

    #region static data

    private const string LINE_NUMBER_PREPROCESSOR_CODE = "__LINE__";
    private const string FILE_NAME_PREPROCESSOR_CODE = "__FILE__";

    private const bool ONLY_UPPERCASE_REDEFINE = false;

    public static ScriptOptions scriptOptionsBuffer = ScriptOptions.Default.AddReferences(
            typeof(UnityEngine.MonoBehaviour).GetTypeInfo().Assembly /*,
        typeof(System.Text.RegularExpressions.Regex).GetTypeInfo().Assembly,
             typeof(helper.GlobalHelper).GetTypeInfo().Assembly,
             typeof(System.Collections.Generic.Dictionary<int,int>).GetTypeInfo().Assembly
            //,typeof(System.Exception).GetTypeInfo().Assembly
*/
        ).AddImports(
            // "UnityEngine", "System"
        ).WithEmitDebugInformation(true)
        .WithFileEncoding(HardwareInternal.mainEncoding);

    public static readonly List<Type> allLibraries = new List<Type>()
    {
        typeof(Libraries.system.output.Console),
        typeof(Libraries.system.Runtime),
        typeof(Libraries.system.output.graphics.Screen),
        typeof(Libraries.system.output.graphics.texture32.Texture32),
        typeof(Libraries.system.output.graphics.system_texture.SystemTexture),
        typeof(Libraries.system.output.graphics.color32.Color32),
        typeof(Libraries.system.output.graphics.system_colorspace.ColorConstants),
        typeof(Libraries.system.output.graphics.screen_buffer32.ScreenBuffer32),
        typeof(Libraries.system.output.graphics.system_screen_buffer.SystemScreenBuffer),
        typeof(Libraries.system.file_system.File),
        typeof(Libraries.system.shell.IShellProgram),
        typeof(Libraries.system.mathematics.Vector2),
        typeof(Libraries.system.input.KeyHandler),
        typeof(Libraries.system.input.MouseHandler),
        typeof(Libraries.system.output.graphics.mask_texture.MaskTexture),


        typeof(System.Text.RegularExpressions.Regex),
        typeof(helper.GlobalHelper),
        typeof(System.Collections.Generic.Dictionary<int, int>),
        typeof(Libraries.system.output.music.Sound),

        typeof(System.Linq.Enumerable) /*,
        typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)*/
    };

    public static readonly List<LibraryData> allLibraryDatas = allLibraries.ConvertAll(x => x.ToLibraryData());

    #endregion


    [SerializeField] internal List<CodeTask> scriptsRunning = new List<CodeTask>();

    #region code Parsing

    static Regex includeRegex = new Regex("^\\s*?#\\s*?include\\s*?[\"'].*[\"']\\s*?");
    static Regex includeRegex2 = new Regex("^\\s*?#\\s*?include\\s*?[\"'].*[\"']\\s*;*");

    static Regex redefineRegex = new Regex("^\\s*?#\\s*?redefine\\s*?.*\\s*.*");

    static Regex undefineRegex = new Regex("^\\s*?#\\s*?undefine\\s*?.*\\s*");
    static Regex undefineRegexOld = new Regex("^\\s*?#\\s*?undefine\\s*?.*\\s*.*;*");
    static Regex topRegex = new Regex("^\\s*?#\\s*?top\\s*?.*");
    static Regex bottomRegex = new Regex("^\\s*?#\\s*?bottom\\s*?.*");

    internal void RunCode(CodeObject codeObject)
    {
        Debug.Log("Making new codeTask");
        CodeTask codeTask = new CodeTask(hardware);
        scriptsRunning.Add(codeTask);
        CodeParser(ref codeObject);
        codeTask.RunCode(codeObject);
        GlobalDebugger.instance.WrapInIf(codeObject.code);
        Debug.Log("After running code");
    }

    internal void RemoveCodeTask(CodeTask codeTask)
    {
        scriptsRunning.Remove(codeTask);
    }

    internal void CheckThreads()
    {
        foreach (CodeTask ct in scriptsRunning)
        {
            Debug.Log("thread null?" + (ct.thread == null ? "yes" : "no") + ct.thread);
        }
    }


    internal void KillAll()
    {
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

    internal void CodeParser(ref CodeObject codeObject)
    {
        /* codeObject.code = @"using static HardwareBox;
 public class HardwareBox
 {
     public static Hardware hardware;
 }
 HardwareBox.hardware = ownPointer;
 " + codeObject.code;*/
        codeObject.code = @"static Runtime runtime = null;runtime=ownPointer.runtime;
static FileSystem fileSystem = null;fileSystem=ownPointer.fileSystem;
static KeyHandler keyHandler = null;keyHandler=ownPointer.keyHandler;
static MouseHandler mouseHandler = null;mouseHandler=ownPointer.mouseHandler;
static Screen screen = null;screen=ownPointer.screen;
" + codeObject.code;
        codeObject.code = "\n#include \"/sys/kernel\"\n" + codeObject.code;
        List<string> lines =
            new List<string>(codeObject.code.Replace("#if false //changeToTrue", "#if true").SplitNewLine());

        List<string> importedLibraries = new List<string>();
        Dictionary<string, string> currentRedefines = new Dictionary<string, string>();
        currentRedefines.Add("false//changeToTrue", "true");
        int positionToExpectNextInlcude = 0;
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


            if ((positionToExpectNextInlcude < index && !includeRegex.IsMatch(buffer)))
            {
                if (string.IsNullOrEmpty(buffer) || string.IsNullOrWhiteSpace(buffer) || buffer.StartsWith("//") ||
                    buffer.StartsWith("/*"))
                {
                    positionToExpectNextInlcude++;
                }
                else
                {
                    //  Debug.Log($"next:{positionToExpectNextInlcude} index:{index}");
                    //  checkIncludes = false;
                }
            }

            if (true)
            {
                if (topRegex.IsMatch(buffer))
                {
                    lines[index] = $"//moved '{buffer}' on top";

                    lines.Insert(0, buffer.Substring(buffer.IndexOf("top") + "top".Length));
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
                    positionToExpectNextInlcude++;
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
                    positionToExpectNextInlcude++;

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

                        positionToExpectNextInlcude += importedLines.Count + 1;

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