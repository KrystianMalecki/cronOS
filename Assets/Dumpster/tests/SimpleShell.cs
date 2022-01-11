#if false
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
using Libraries.system.output.graphics.mask_texture;
using System.Text.RegularExpressions;



//public class ls : ExtendedShellProgram { private static ls _instance; public static ls instance { get { if (_instance == null) { _instance = new ls(); } return _instance; } } public override string GetName() { return "ls"; } private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> { new AcceptedArgument("-wd", "working directory", true), new AcceptedArgument("-r", "recursive", false), new AcceptedArgument("-jn", "just names", false), new AcceptedArgument("-sz", "show size", false), new AcceptedArgument("-fp", "full paths instead of names", false), }; protected override List<AcceptedArgument> argumentTypes => _argumentTypes; protected override string InternalRun(Dictionary<string, string> argPairs) { string path = "/"; if (argPairs.TryGetValue("-wd", out path)) { } File f = FileSystem.GetFileByPath(path); return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp")); } string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths) { string str = string.Format(onlyNames ? "{2}" : "{0," + indent + "}{1}{2}:{3}\n", "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize()); if (recursive) { for (int i = 0; i < file.children.Count; i++) { bool last = i + 1 == file.children.Count; File child = file.children[i]; str += GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths); } } return str; } }
public class SimpleShell : UnityEngine.MonoBehaviour
{
    //ls import

    public class ls : ExtendedShellProgram
    {
        /* private static ls _instance;
         public static ls instance
         {
             get
             {
                 if (_instance == null)
                 {
                     _instance = new ls();
                 }
                 return _instance;
             }
         }*/
        public override string GetName()
        {
            return "ls";
        }
        private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-p", "path", true),
        new AcceptedArgument("-r", "recursive", false),
        new AcceptedArgument("-jn", "just names", false),
        new AcceptedArgument("-sz", "show size", false),
        new AcceptedArgument("-fp", "full paths instead of names", false),
    };

        protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

        protected override string InternalRun(Dictionary<string, string> argPairs)
        {
            string wdPath = "/";
            if (argPairs.TryGetValue("-wd", out wdPath))
            {
            }
            File workingDirectory = FileSystem.GetFileByPath(wdPath);
            string path = "/";
            if (argPairs.TryGetValue("-p", out path))
            {
            }
            File f = FileSystem.GetFileByPath(path, workingDirectory);
            Console.Debug($"{f} {path}");
            return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
        }
        string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths)
        {
            string str = string.Format(
                 onlyNames ? "{2}" : "{0," + indent + "}{1}{2}:{3}\n"
                  , "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize());
            if (recursive)
            {
                if (file.children != null || true)
                {
                    for (int i = 0; i < file.children?.Count; i++)
                    {
                        bool last = i + 1 == file.children.Count;
                        File child = file.children[i];
                        str += GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths);

                    }
                }
            }
            return str;
        }
    }

    public class mkf : ExtendedShellProgram
    {
        /* private static ls _instance;
         public static ls instance
         {
             get
             {
                 if (_instance == null)
                 {
                     _instance = new ls();
                 }
                 return _instance;
             }
         }*/
        public override string GetName()
        {
            return "mkf";
        }
        private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-p", "path", true),
        new AcceptedArgument("-d", "data", true),
        new AcceptedArgument("-n", "name", true),
        new AcceptedArgument("-fp", "file permissions", true),

    };

        protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

        protected override string InternalRun(Dictionary<string, string> argPairs)
        {
            string wdPath = argPairs.GetValueOrDefault("-wd", "/");
            File workingDirectory = FileSystem.GetFileByPath(wdPath);

            string path = argPairs.GetValueOrDefault("-p", "/");
            File f = FileSystem.GetFileByPath(path, workingDirectory);

            string name = argPairs.GetValueOrDefault("-n", "new file");
            short fpInt;
            if (!short.TryParse(argPairs.GetValueOrDefault("-fp", "0b0000111"), out fpInt))
            {
                fpInt = 0b0000111;
            }
            FilePermission fp = (FilePermission)fpInt;
            File newFile = FileSystem.MakeFile(wdPath, name, fp, null);

            return $"Created file {newFile.name} at {newFile.Parent.GetFullPath()}.";
        }

    }














    private void b()
    {
        //# redefine //# #
        //# include "/System/test_library"
        //commnet

        Console.Debug();
        const string help = "Press mouse to move to mouse\n"
                   + "Arrows to move\n"
                   + "Keypad +/- to change speed\n"
                   + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
        Screen.InitScreenBuffer(buffer);



        Console.Debug();
        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");

        Console.Debug();
        //Console.Debug(fontAtlas.data);
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        Console.Debug(1);
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
            char[] charText = text.ToCharArray();
            for (int i = 0; i < text.Length; i++)
            {
                char c = charText[i];
                if (c == '\r')
                {
                    posX = x;
                    continue;
                }
                else if (c == '\n')
                {
                    posX = x;
                    posY += 8;
                    continue;
                }
                DrawCharAt(posX, posY, c);
                posX += 8;
            }
        }




        Vector2Int orbPos = new Vector2Int(buffer.width / 2, buffer.height / 2);
        Vector2Int mousePos = new Vector2Int(0, 0);

        string position = "";
        string text = "";
        int speed = 1;
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;
        bool flasher = false;

        void CheckMovement(KeySequence ks)
        {
            if (ks.ReadKey(Key.UpArrow))
            {
                orbPos.y -= speed;
            }
            if (ks.ReadKey(Key.DownArrow))
            {
                orbPos.y += speed;
            }
            if (ks.ReadKey(Key.LeftArrow))
            {
                orbPos.x -= speed;
            }
            if (ks.ReadKey(Key.RightArrow))
            {
                orbPos.x += speed;
            }
            if (ks.ReadKey(Key.KeypadPlus))
            {
                speed++;
            }
            if (ks.ReadKey(Key.KeypadMinus))
            {
                speed--;
            }
        }


        void ClampPositionToFrame()
        {
            if (orbPos.x > buffer.width - 1)
            {
                orbPos.x = buffer.width - 1;
            }
            if (orbPos.x < 0)
            {
                orbPos.x = 0;
            }
            if (orbPos.y > buffer.height - 1)
            {
                orbPos.y = buffer.height - 1;
            }
            if (orbPos.y < 0)
            {
                orbPos.y = 0;
            }
        }
        void Draw()
        {
            DrawStringAt(8, 0, help);

            position = $"X:{orbPos.x},Y:{orbPos.y},Speed:{speed}";

            DrawStringAt(0, 4 * 8, position);
            buffer.DrawLine(mousePos.x, mousePos.y, orbPos.x, orbPos.y, SystemColor.white);
            buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
            DrawStringAt(0, 5 * 8, text);
            buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
            flasher = !flasher;
            AsyncScreen.SetScreenBuffer(buffer);
        }
        void ProcessInput()
        {
            ks = kh.WaitForInput();
            string input = KeyHandler.GetInputAsString();
            text += kh.TryGetCombinedSymbol(ref ks);
          //  text = text.AddInputSpecial(input, ks);

            CheckMovement(ks);
            if (ks.ReadKey(Key.Mouse0))
            {
                mousePos = MouseHander.GetScreenPosition();
            }
            ClampPositionToFrame();
        }
        while (true)
        {
            buffer.FillAll(SystemColor.black);

            Draw();
            ProcessInput();

            Runtime.Wait(1);
        }
    }








    public void Begin()
    {

        //# include "/System/libraries/programsLibrary.ll"

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
            char[] charText = text.ToCharArray();
            for (int i = 0; i < text.Length; i++)
            {
                char c = charText[i];
                if (c == '\r')
                {
                    posX = x;
                    continue;
                }
                else if (c == '\n')
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
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;
        void ProcessInput()
        {
            ks = kh.WaitForInput();
            string input = KeyHandler.GetInputAsString();
            bool shift = ks.ReadAnyShift();
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
                    text += (shift ? ((c + "").ToUpperInvariant()) : c);
                    bufferedText = text;
                }
            }

            if (ks.ReadKey(Key.UpArrow))
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
            if (ks.ReadKey(Key.DownArrow))
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

            Runtime.Wait();
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



    //# include "/System/programs/ls"
    //# include "/System/programs/mkf"

    // static readonly IShellProgram[] commands = { ls.instance };
    static readonly IShellProgram[] commands = { new ls(), new mkf() };

    public static string FindAndExecuteCommand(string rawCommand)
    {
        List<string> parts = rawCommand.SplitSpaceQ();

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
    //# include "/System/libraries/programsLibrary.ll"

    //# include "/System/libraries/fontLibrary.ll"
   
}


#endif