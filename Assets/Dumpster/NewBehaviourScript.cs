//#include "/System/libraries/tools.ll"

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.input;
using Libraries.system.mathematics;
using Libraries.system.output;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.mask_texture;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.shell;

#region fontLibrary

#if false
//#include "/sys/libs/util.ll"
//#top using static FontLibrary;
public class FontLibrary
{
    static MaskTexture fontTexture = null;
    static Regex colorTagRegex = new Regex(@"^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))");
const string fontPath = "/sys/defFontAtlas";
    static Regex backgroundColorTagRegex =
        new Regex(
            @"^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc)))");

    static Regex nonParseTagRegex = new Regex(@"^(\s*?\/?((raw)|(no-parsing)|(np)))");

    public static void Init()
    {
        File fontFile = fileSystem.GetFileByPath(fontPath);
        fontTexture = MaskTexture.FromData(fontFile.data);
    }

    public static void DrawColoredCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character,
        SystemColor foreground, SystemColor background)
    {
        if (systemScreenBuffer == null)
        {
            //todo-future add error
            return;
        }

        int index = Runtime.CharToByte(character);
        int posx = index % 16;
        int posy = index / 16;

        systemScreenBuffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8).Convert<SystemColor>
                (o => (o ? foreground : background)),
            fontTexture.transparencyFlag);
    }

    public static void DrawColoredStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
        SystemColor foreground, SystemColor background, bool enableTags = true)
    {
        SystemColor currentForeground = foreground, currentBackground = background;
        int posX = x;
        int posY = y;
        char[] charText = text.ToCharArray();
        bool parseTags = true;
        for (int i = 0; i < text.Length; i++)
        {
            char character = charText[i];
            if (character == '\r')
            {
                posX = x;
                continue;
            }
            else if (character == '\n')
            {
                posX = x;
                posY += 8;
                continue;
            }
            else if (enableTags) //tags enabled
            {
                if (character == '<') //found tag
                {
                    if (i < 1 || charText[i - 1] != '\\') //not escaped
                    {
                        int endTagIndex = text.IndexOf('>', i);
                        if (endTagIndex != -1)
                        {
                            string tagText = text.Substring(i + 1, endTagIndex - i - 1).Trim();
                            if (nonParseTagRegex.IsMatch(tagText))
                            {
                                parseTags = (tagText.StartsWith("/"));
                                i = endTagIndex;
                                continue;
                            }

                            if (parseTags)
                            {
                                if (colorTagRegex.IsMatch(tagText))
                                {
                                    if (tagText.StartsWith("/"))
                                    {
                                        currentForeground = foreground;
                                    }
                                    else
                                    {
                                        int equalsIndex = tagText.IndexOf('=');
                                        string color = tagText.Substring(equalsIndex + 1);
                                        Console.Debug(
                                            $"match c! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                        if (color.Length == 1)
                                        {
                                            currentForeground = Runtime.HexToByte(color);
                                        }
                                    }
                                }

                                if (backgroundColorTagRegex.IsMatch(tagText))
                                {
                                    if (tagText.StartsWith("/"))
                                    {
                                        currentBackground = background;
                                    }
                                    else
                                    {
                                        int equalsIndex = tagText.IndexOf('=');
                                        string color = tagText.Substring(equalsIndex + 1);
                                        Console.Debug(
                                            $"match bckc! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                        if (color.Length == 1)
                                        {
                                            currentBackground = Runtime.HexToByte(color);
                                        }
                                    }
                                }


                                i = endTagIndex;

                                continue;
                            }
                        }
                    }
                }
            }
            else if (character == '\\')
            {
                continue;
            }

            DrawColoredCharAt(systemScreenBuffer, posX, posY, character, currentForeground, currentBackground);
            posX += 8;
        }
    }

    public static void DrawCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character)
    {
        DrawColoredCharAt(systemScreenBuffer, x, y, character, SystemColor.white, SystemColor.black);
    }

    public static void DrawStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
        bool enableTags = true)
    {
        DrawColoredStringAt(systemScreenBuffer, x, y, text, SystemColor.white, SystemColor.black, enableTags);
    }

    public static string GetColorTag(SystemColor color)
    {
        return $"<color={Runtime.ByteToHex(color, false)}>";
    }

    public static string GetBackgroundColorTag(SystemColor color)
    {
        return $"<color={Runtime.ByteToHex(color, false)}>";
    }
}

FontLibrary.Init();
#endif

#endregion

#region ls

#if false
    public class ls : ExtendedShellProgram
    {
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
            File workingDirectory = fileSystem.GetFileByPath(wdPath);
            string path = "/";
            if (argPairs.TryGetValue("-p", out path))
            {
            }
            File f = fileSystem.GetFileByPath(path, workingDirectory);
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
                        str +=
 GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths);

                    }
                }
            }
            return str;
        }
    }

#endif

#endregion

#region mkf

#if false
public class mkf : ExtendedShellProgram
{
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
        File workingDirectory = fileSystem.GetFileByPath(wdPath);

        string path = argPairs.GetValueOrDefault("-p", "/");
        File f = fileSystem.GetFileByPath(path, workingDirectory);

        string name = argPairs.GetValueOrDefault("-n", "new file");
        short fpInt;
        if (!short.TryParse(argPairs.GetValueOrDefault("-fp", "0b0000111"), out fpInt))
        {
            fpInt = 0b0000111;
        }
        FilePermission fp = (FilePermission)fpInt;
        File newFile = fileSystem.MakeFile(wdPath, name, fp, null);

        return $"Created file {newFile.name} at {newFile.Parent.GetFullPath()}.";
    }

}

#endif

#endregion

#region adv CT

#if false
//#include "/sys/libs/fontLib.ll"


            const string help = "Press mouse to move to mouse\n"
                       + "Arrows to move\n"
                       + "Keypad +/- to change speed\n"
                       + "Type to type☻";
            SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
            screen.InitScreenBuffer(buffer);




            Vector2Int orbPos = new Vector2Int(buffer.width / 2, buffer.height / 2);
            Vector2Int mousePos = new Vector2Int(0, 0);

            string position = "";
            string text = "";
            int speed = 1;
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
                DrawStringAt(buffer, 8, 0, help);

                position = $"X:{orbPos.x},Y:{orbPos.y},Speed:{speed}";

                DrawStringAt(buffer, 0, 4 * 8, position);

                buffer.DrawLine(mousePos, orbPos, SystemColor.white);
                buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
                DrawStringAt(buffer, 0, 5 * 8, text);
                buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
                flasher = !flasher;
                hardware.screen.SetScreenBuffer(buffer);
            }
            void ProcessInput()
            {
                ks = keyHandler.WaitForInput();
                string input = keyHandler.GetInputAsString();
                text += keyHandler.TryGetCombinedSymbol(ref ks);
                text = text.AddInputSpecial(input, ks);


                CheckMovement(ks);
                if (ks.ReadKey(Key.Mouse0))
                {
                    mousePos = mouseHandler.GetScreenPosition();
                }
                ClampPositionToFrame();
            }
            while (true)
            {
                buffer.FillAll(SystemColor.black);
                Draw();
                ProcessInput();
                runtime.Wait(100);

            }
#endif

#endregion

#region shell

#if false
//#include "/sys/libs/binsLib.ll"
//#include "/sys/libs/fontLib.ll"
//#top using static Shell;
public class Shell
{
    File currentFile = fileSystem.GetFileByPath("/sys");

    SystemScreenBuffer screenBuffer = new SystemScreenBuffer();
    KeySequence bufferKeySequence = null;


    string prefix = "";
    string inputText = "";
    string bufferInput = "";
    string consoleText = "";


    int historyPointer = 0;
    List<string> history = new List<string>();


    void UpdatePrefix()
    {
        prefix = currentFile.GetFullPath() + ">";
    }

    void Draw()
    {
        DrawStringAt(screenBuffer, 0, 0, consoleText);
        int consoleY = consoleText.CountEncounters('\n') + 1;
        DrawStringAt(screenBuffer, 0, consoleY * 8, prefix);
        DrawStringAt(screenBuffer, prefix.Length * 8, consoleY * 8, inputText);
        screen.SetScreenBuffer(screenBuffer);
    }

    void ProcessInput()
    {
        bufferKeySequence = keyHandler.WaitForInput();
        string input = keyHandler.GetInputAsString();
        bool shift = bufferKeySequence.ReadAnyShift();
        foreach (char c in input)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (inputText.Length != 0)
                {
                    inputText = inputText.Substring(0, inputText.Length - 1);
                    bufferInput = inputText;
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                consoleText += '\n' + prefix + inputText;
                string output = PraseCommand(inputText);
                if (!string.IsNullOrEmpty(output))
                {
                    consoleText += '\n' + output;
                }

                inputText = "";
                bufferInput = "";
            }
            else
            {
                inputText += (shift ? ((c + "").ToUpperInvariant()) : c);
                bufferInput = inputText;
            }
        }

        if (bufferKeySequence.ReadKey(Key.UpArrow))
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
                inputText = bufferInput;
            }
            else
            {
                inputText = history[historyPointer];
            }
        }
        else if (bufferKeySequence.ReadKey(Key.DownArrow))
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
                inputText = bufferInput;
            }
            else
            {
                inputText = history[historyPointer];
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
            File f = fileSystem.GetFileByPath(parts[1], currentFile);
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

    public void Run()
    {
        screen.InitScreenBuffer(screenBuffer);
        UpdatePrefix();
        while (true)
        {
            screenBuffer.FillAll(SystemColor.black);
            Draw();
            ProcessInput();
            runtime.Wait();
        }
    }
}

static Shell shell = new Shell();

#endif

#endregion