#if false
 using static FontLibrary;
 using static Shell;
Hardware.currentThreadInstance = thisHardware;
//imported: /sys/libs/binsLib.lib
 //   # include "/sys/bins/ls"
  //  # include "/sys/bins/mkf"
  //  # include "/sys/bins/paint"
//# include "/sys/bins/play"
//#include "/sys/bins/shout"
    static readonly IShellProgram[] commands = {/* new ls(),new mkf(),new paint(),new play(),new shout()*/ };
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
//imported: /sys/libs/fontLib.lib
//imported: /sys/libs/util.lib
//redefine line Debugger.Debug(__line__);
//moved '# top using static FontLibrary;' on top
public class FontLibrary
{
    public static MaskTexture fontTexture = null;
    static Regex colorTagRegex = new Regex(@"^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))");
    const string fontPath = "/sys/defFontAtlas";
    static Regex backgroundColorTagRegex =
        new Regex(
            @"^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc)))");
    static Regex nonParseTagRegex = new Regex(@"^(\s*?\/?((raw)|(no-parsing)|(np)))");
    public static SystemColor defaultForegroundColor = SystemColor.white;
    public static SystemColor defaultBackgroundColor = SystemColor.black;
    public static void Init()
    {
        File fontFile = FileSystem.GetFileByPath(fontPath);
        fontTexture = MaskTexture.FromData(fontFile.data);
    }
    protected static void DrawColoredCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character,
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
                (o => (o > 0 ? foreground : background)),
            fontTexture.transparencyFlag);
    }
    protected static void DrawColoredStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
        SystemColor foreground, SystemColor background, bool enableTags)
    {
        SystemColor currentForeground = foreground, currentBackground = background;
        int posX = x, posY = y;
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
                                i = endTagIndex;
                                ParseTags(tagText, ref i, ref currentForeground, ref currentBackground, ref posX, ref posY, ref character, ref text);
                            }
                        }
                    }
                }
            }
            /*else if (character == '\\')
            {
                continue;
            }*/
            if (ParseString(ref currentForeground, ref currentBackground, ref posX, ref posY, ref character, ref text))
            {
                DrawColoredCharAt(systemScreenBuffer, posX, posY, character, currentForeground, currentBackground);
                posX += 8;
            }
        }
    }
    protected static bool ParseString(ref SystemColor currentForeground, ref SystemColor currentBackground, ref int posX, ref int posY, ref char character, ref string text)
    {
        return true;
    }
    protected static bool ParseTags(string tagText, ref int index,
 ref SystemColor currentForeground,
        ref SystemColor currentBackground,
        ref int posX, ref int posY,
        ref char character,
        ref string text)
    {
        if (colorTagRegex.IsMatch(tagText))
        {
            if (tagText.StartsWith("/"))
            {
                currentForeground = defaultForegroundColor;
            }
            else
            {
                int equalsIndex = tagText.IndexOf('=');
                string color = tagText.Substring(equalsIndex + 1);
                Debugger.Debug(
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
                currentBackground = defaultBackgroundColor;
            }
            else
            {
                int equalsIndex = tagText.IndexOf('=');
                string color = tagText.Substring(equalsIndex + 1);
                Debugger.Debug(
                    $"match bckc! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                if (color.Length == 1)
                {
                    currentBackground = Runtime.HexToByte(color);
                }
            }
        }
        return true;
    }
    public static void DrawCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character)
    {
        DrawColoredCharAt(systemScreenBuffer, x, y, character, defaultForegroundColor, defaultBackgroundColor);
    }
    public static void DrawStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
        bool enableTags = true)
    {
        DrawColoredStringAt(systemScreenBuffer, x, y, text, defaultForegroundColor, defaultBackgroundColor, enableTags);
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
//moved '#top using static Shell;' on top
public class Shell
{
    File currentFile = FileSystem.GetFileByPath("/sys");
  public static   SystemScreenBuffer screenBuffer = new SystemScreenBuffer();
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
        Screen.SetScreenBuffer(screenBuffer);
    }
    void ProcessInput()
    {
        bufferKeySequence = KeyHandler.WaitForInputBuffer();
        string input = KeyHandler.GetInputAsString();
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
        if (bufferKeySequence.ReadAndCooldownKey(Key.UpArrow))
        {
            historyPointer--;
	historyPointer=Math.Clamp(historyPointer,0,history.Count);
            if (historyPointer == history.Count)
            {
                inputText = bufferInput;
            }
            else
            {
                inputText = history[historyPointer];
            }
        }
        else if (bufferKeySequence.ReadAndCooldownKey(Key.DownArrow))
        {
            historyPointer++;
            historyPointer=Math.Clamp(historyPointer,0,history.Count);
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
            File f = FileSystem.GetFileByPath(parts[1], currentFile);
            if (f == null)
            {
                return $"Couldn't find file {parts[1]}!";
            }
            currentFile = f;
            UpdatePrefix();
            return "";
        }else if(parts[0]=="balls"){
}
        else
        {
            return FindAndExecuteCommand(input + " -wd " + currentFile.GetFullPath());
        }
        return $"Couldn't find command `{parts[0]}`.";
    }
    public void Run()
    {
       Screen.InitScreenBuffer(screenBuffer);
        UpdatePrefix();
        while (true)
        {
Debugger.Debug("shell loop");
            screenBuffer.FillAll(SystemColor.black);
            Draw();
            ProcessInput();
            Runtime.Wait();
     
 }
    }
}
static Shell shell = null;
shell=new Shell();
shell.Run();
#endif