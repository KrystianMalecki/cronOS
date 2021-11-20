using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.output.graphics.system_screen_buffer;


using Libraries.system.input;
using Libraries.system.mathematics;
using System.Collections;
using System.Collections.Generic;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.system_texture;
using Libraries.system.output.graphics.system_colorspace;
using NaughtyAttributes;
using Libraries.system.output;
using Libraries.system.shell;
using JetBrains.Annotations;
using helper;
using System.Linq;


//ls import
/*public class ls : ExtendedShellProgram
{
    private static  ls _instance;
    public static  ls instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ls();
            }
            return _instance;
        }
    }
    public override string GetName()
    {
        return "ls";
    }
    private static  readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-r", "recursive", false),
        new AcceptedArgument("-jn", "just names", false),
                new AcceptedArgument("-sz", "show size", false),
                new AcceptedArgument("-fp", "full paths instead of names", false),
    };

    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

    protected override string InternalRun(Dictionary<string, string> argPairs)
    {
        string path = "/";
        if (argPairs.TryGetValue("-wd", out path))
        {
        }
        File f = FileSystem.GetFileByPath(path);

        return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
    }
    string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths)
    { string str = string.Format(
           onlyNames ? "{2}" : "{0," + indent + "}{1}{2}:{3}\n"
            , "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize());
        if (recursive)
        {
            for (int i = 0; i < file.children.Count; i++)
            {
                bool last = i + 1 == file.children.Count;
                File child = file.children[i];
                str += GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths);

            }
        }
        return str;
    }


}*/
public class ls : ExtendedShellProgram { private static ls _instance; public static ls instance { get { if (_instance == null) { _instance = new ls(); } return _instance; } } public override string GetName() { return "ls"; } private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> { new AcceptedArgument("-wd", "working directory", true), new AcceptedArgument("-r", "recursive", false), new AcceptedArgument("-jn", "just names", false), new AcceptedArgument("-sz", "show size", false), new AcceptedArgument("-fp", "full paths instead of names", false), }; protected override List<AcceptedArgument> argumentTypes => _argumentTypes; protected override string InternalRun(Dictionary<string, string> argPairs) { string path = "/"; if (argPairs.TryGetValue("-wd", out path)) { } File f = FileSystem.GetFileByPath(path); return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp")); } string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths) { string str = string.Format(onlyNames ? "{2}" : "{0," + indent + "}{1}{2}:{3}\n", "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize()); if (recursive) { for (int i = 0; i < file.children.Count; i++) { bool last = i + 1 == file.children.Count; File child = file.children[i]; str += GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths); } } return str; } }
public class SimpleShell : UnityEngine.MonoBehaviour
{














    public void Awake()
    {
        UnityEngine.Debug.Log(GlobalHelper.SplitString2Q("ls -w \"/System\"").GetValuesToString("|"));
        UnityEngine.Debug.Log(new Path("/System/"));
        UnityEngine.Debug.Log(new Path("/System/programs"));
        UnityEngine.Debug.Log(new Path("./ls", FileSystem.GetFileByPath("/System/programs")));
        UnityEngine.Debug.Log(new Path("./..", FileSystem.GetFileByPath("/System/programs")));
        UnityEngine.Debug.Log(new Path("./../programs", FileSystem.GetFileByPath("/System/programs")));
        UnityEngine.Debug.Log(new Path("./../can'tfind", FileSystem.GetFileByPath("/System/programs")));
        UnityEngine.Debug.Log(new Path("./../../programs", FileSystem.GetFileByPath("/System/programs/ls")));
        UnityEngine.Debug.Log(new Path("./../../programs/ls", FileSystem.GetFileByPath("/System/programs/ls")));

    }










    public void Begin()
    {

        //# include "/System/programs/programsLibrary"

        string prefix = "";
        File currentFile = FileSystem.GetFileByPath("/System");
        string console = "";
        string bufferedText = "";
        int historyPointer = 0;
        List<string> history = new List<string>();
        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = Runtime.CharToByte(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
        }
        void DrawStringAt(int x, int y, string text)
        {
            int posX = x;
            int posY = y;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text.ToCharArray()[i];
                if (c == '\n' || c == '\r')
                {
                    posX = x;
                    posY += 8;
                    continue;
                }
                DrawCharAt(posX, posY, c);
                posX += 8;
            }
        }

        void UpdatePrefix()
        {
            prefix = currentFile.GetFullPath() + ">";
        }




        string text = "";
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;


        int CountEncounters(string input, params char[] toFind)
        {
            int counter = 0;
            foreach (char c in input)
            {
                foreach (char compareTo in toFind)
                {
                    if (compareTo == c)
                    {
                        counter++;
                        break;
                    }
                }

            }
            return counter;
        }


        void Draw()
        {
            DrawStringAt(0, 0, console);
            int consoleY = CountEncounters(console, '\r', '\n') + 1;
            DrawStringAt(0, consoleY * 8, prefix);


            DrawStringAt(prefix.Length * 8, consoleY * 8, text);
            AsyncScreen.SetScreenBuffer(buffer);
        }
        void ProcessInput()
        {
            ks = kh.WaitForInput();
            string input = KeyHandler.GetInputAsString();
            foreach (char c in input)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (text.Length != 0)
                    {
                        text = text.Substring(0, text.Length - 1);
                        bufferedText = text;
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    console += '\n' + prefix + text;
                    string output = PraseCommand(text);
                    if (!string.IsNullOrEmpty(output))
                    {
                        console += '\n' + output;
                    }
                    text = "";
                    bufferedText = "";
                }
                else
                {
                    text += c;
                    bufferedText = text;
                }
            }
        
            if (ks.HasKey(Key.UpArrow))
            {
                historyPointer--;
                if (historyPointer < 0)
                {
                    historyPointer = 0;
                }
                if (historyPointer > history.Count)
                {
                    historyPointer = history.Count;
                }
                if (historyPointer == history.Count)
                {
                    text = bufferedText;
                }
                else
                {
                    text = history[historyPointer];
                }
            }
            if (ks.HasKey(Key.DownArrow))
            {
                historyPointer++;
                if (historyPointer < 0)
                {
                    historyPointer = 0;
                }
                if (historyPointer > history.Count)
                {
                    historyPointer = history.Count;
                }
                if (historyPointer == history.Count)
                {
                    text = bufferedText;
                }
                else
                {
                    text = history[historyPointer];
                }
            }
        }
        string PraseCommand(string input)
        {
            history.Add(input);
            historyPointer++;
            string[] parts = input.Split(' ');
            if (parts[0] == "cd")
            {
                File f = FileSystem.GetFileByPath(parts[1], currentFile);
                if (f == null)
                {
                    return $"Couldn't find file {parts[1]}!";
                }
                currentFile = f;
                UpdatePrefix();
                return "";
            }
            else
            {
                return FindAndExecuteCommand(input + " -wd " + currentFile.GetFullPath());
            }
            return $"Couldn't find command `{parts[0]}`.";
        }


        string GetChildren(string indent, File file, string prefix, bool recursive = false)
        {
            string str = $"{indent}{prefix}{file.name}:{file.GetByteSize()}\n";
            if (recursive)
            {
                for (int i = 0; i < file.children.Count; i++)
                {
                    File child = file.children[i];
                    str += GetChildren(indent + " ", child, $"{((i + 1) == file.children.Count ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))} ", recursive);

                }
            }
            return str;
        }

        UpdatePrefix();
        while (true)
        {
            buffer.FillAll(SystemColor.black);


            Draw();
            ProcessInput();

            Runtime.Wait(1);
        }

    }
    public void s()
    {
        string charInfo = "";


        SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = Runtime.CharToByte(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8), fontTexture.transparencyFlag);
        }
        void DrawStringAt(int x, int y, string text)
        {
            int posX = x;
            int posY = y;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text.ToCharArray()[i];
                if (c == '\n' || c == '\r')
                {
                    posX = x;
                    posY += 8;
                    continue;
                }
                DrawCharAt(posX, posY, c);
                posX += 8;
            }
        }





        string text = "";
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;
        Vector2Int mousePos = new Vector2Int(0, 0);



        // Console.Debug(ls.instance.Run("-f / "));

        void Draw()
        {

            buffer.DrawTexture(0, 0, fontTexture, fontTexture.transparencyFlag);
            DrawStringAt(0, 17 * 8, charInfo);
            DrawStringAt(0, 18 * 8, text);
            AsyncScreen.SetScreenBuffer(buffer);
        }
        void ProcessInput()
        {
            ks = kh.WaitForInput();
            string input = KeyHandler.GetInputAsString();
            if (input != "")
            {
                text = text.AddInput(input);
            }
            mousePos = MouseHander.GetScreenPosition();
            if (mousePos.x < 8 * 16 && mousePos.y < 8 * 16)
            {
                int posx = (mousePos.x / 8) % 16;
                int posy = (mousePos.y / 8);
                //charInfo = $"{mousePos.y} {posy} {(mousePos.y / 8)}";
                charInfo = $"X{posx} Y{posy} {posx + posy * 16} Char:{Runtime.ByteToChar((byte)(posx + posy * 16))}";
            }
        }
        while (true)
        {
            buffer.FillAll(SystemColor.black);

            Draw();
            ProcessInput();

            Runtime.Wait(1);
        }
    }

    /*

     #include "/system/programs/ls"
     */
    static readonly IShellProgram[] commands = { ls.instance };
    public static string FindAndExecuteCommand(string rawCommand)
    {
        List<string> parts = GlobalHelper.SplitString2Q(rawCommand);

        string command = parts[0];
        string restOfArgs = (rawCommand.Length <= command.Length) ? "" : rawCommand.Substring(command.Length);
        string output = $"Command '{command}' not found!";
        for (int i = 0; i < commands.Length; i++)
        {
            if (commands[i].GetName() == command)
            {
                return commands[i].Run(restOfArgs);
            }
        }
        return output;
    }
}
