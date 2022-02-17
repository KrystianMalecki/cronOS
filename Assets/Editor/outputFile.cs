#if false
 using static Shell;
 using static FontLibrary;
 using static Kernel;
//imported: /sys/kernel
//moved '#top using static Kernel;' on top
public class Kernel
{
	public static object mainLock = new object();
}
static Runtime runtime = null;runtime=ownPointer.runtime;
static FileSystem fileSystem = null;fileSystem=ownPointer.fileSystem;
static KeyHandler keyHandler = null;keyHandler=ownPointer.keyHandler;
static MouseHandler mouseHandler = null;mouseHandler=ownPointer.mouseHandler;
static Screen screen = null;screen=ownPointer.screen;
//imported: /sys/libs/binsLib.ll
//imported: /sys/bins/ls
public class ls : ExtendedShellProgram
{
    public override string GetName()
    {
        return "ls";
    }
    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("path", true, "-p"),
        new AcceptedArgument("recursive", false, "-r"),
        new AcceptedArgument("just names", false, "-jn"),
        new AcceptedArgument("show size", false, "-sz"),
        new AcceptedArgument("full paths instead of names", false, "-fp"),
    };
    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;
    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
        string wdPath = argPairs.GetValueOrNull("-wd")?.Value ?? "/";
        File workingDirectory = fileSystem.GetFileByPath(wdPath);
        string path = argPairs.GetValueOrNull("-p")?.Value ?? "/";
        
        File f = fileSystem.GetFileByPath(path, workingDirectory);
        Console.Debug($"{f} {path}");
        return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"),
            argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
    }
    string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames,
        bool fullPaths)
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
                        GetChildren(child, indent + (onlyNames ? 0 : 1),
                            $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize,
                            onlyNames, fullPaths);
                }
            }
        }
        return str;
    }
}
//imported: /sys/bins/mkf
public class mkf : ExtendedShellProgram
{
    public override string GetName()
    {
        return "mkf";
    }
    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("path", true, "-p"),
        new AcceptedArgument("data", true, "-d"),
        new AcceptedArgument("name", true, "-n"),
        new AcceptedArgument("file permissions", true, "-fp"),
    };
    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;
    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
        string wdPath = argPairs.GetValueOrNull("-wd")?.Value ?? "/";
        File workingDirectory = fileSystem.GetFileByPath(wdPath);
        string path = argPairs.GetValueOrNull("-p")?.Value ?? "/";
        File f = fileSystem.GetFileByPath(path, workingDirectory);
        string name = argPairs.GetValueOrNull("-n")?.Value ?? "newFile";
        short fpInt;
        if (!short.TryParse(argPairs.GetValueOrNull("-n")?.Value ?? "0b0000111", out fpInt))
        {
            fpInt = 0b0000111;
        }
        FilePermission fp = (FilePermission)fpInt;
        File newFile = fileSystem.MakeFile(wdPath, name, fp, null);
        return $"Created file {newFile.name} at {newFile.Parent.GetFullPath()}.";
    }
}
//imported: /sys/bins/paint
enum DrawingState
{
    Drawing,
    DrawingLine,
    DrawingRectangle,
    DrawingElipse,
}
public class paint : ExtendedShellProgram
{
    private string filePath;
    bool waitingForStartPosition = false;
    SystemTexture image = null;
    SystemTexture overlay = null;
    File imageFile = null;
    int scale = 4;
    Vector2Int? mousePos = null;
    SystemColor mainColor = SystemColor.white;
    SystemColor UIColor = SystemColor.dark_gray;
    SystemColor secondaryColor = SystemColor.black;
    bool editingMainColor = true;
    DrawingState drawingState = DrawingState.Drawing;
    Vector2Int? startPos = null; //Vector2Int.incorrectVector;
    Vector2Int? lastPos = null; //Vector2Int.incorrectVector;
    KeySequence ks = null;
    bool running = true;
    public override string GetName()
    {
        return "paint";
    }
    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
        Console.Debug(argPairs.ToFormattedString2());
        string workingPath = argPairs.GetValueOrNull("-wd")?.Value ?? "/";
        string name = argPairs.GetValueOrNull("-n")?.Value ?? "image.img";
        int height = int.Parse(argPairs.GetValueOrNull("-h")?.Value ?? "10");
        int width = int.Parse(argPairs.GetValueOrNull("-w")?.Value ?? "10");
        filePath = argPairs.GetValueOrNull("-f")?.Value ?? workingPath;
        if (fileSystem.TryGetFile(filePath + "/" + name, out File file))
        {
            image = SystemTexture.FromData(file.data);
            imageFile = file;
        }
        else
        {
//todo 1 fix: -nf doesn't make new file when file exists
            if (argPairs.ContainsKey("-nf"))
            {
                image = new SystemTexture(width, height);
            }
            else
            {
                //todo 4 error
                return "error";
            }
        }
        overlay = new SystemTexture(image.width, image.height);
        lock (mainLock)
        {
            keyHandler.DumpInputBuffer();
            keyHandler.DumpStringInputBuffer();
//todo 2 rework work loop: paint mode, menu. In paint you draw in menu you can open and save file.
            while (running)
            {
                Console.Debug("paint loop");
                Draw();
                ProcessInput();
                runtime.Wait();
            }
            imageFile ??= Drive.MakeFile(name, new byte[0]);
            byte[] data = image.ToData();
            imageFile.data = data;
            File parent = fileSystem.GetFileByPath(filePath);
            parent.SetChild(imageFile);
        }
        Console.Debug("paint lock off");
        keyHandler.DumpInputBuffer();
        keyHandler.DumpStringInputBuffer();
        return "paint finished work.";
    }
    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("current file", true, "-f"),
        new AcceptedArgument("new file", false, "-nf"),
        new AcceptedArgument("name", true, "-n"),
        new AcceptedArgument("width", true, "-w"),
        new AcceptedArgument("height", true, "-h"),
        new AcceptedArgument("color palette", true, "-cp")
    };
    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;
    void Draw()
    {
        screenBuffer.FillAll(SystemColor.black);
        overlay.FillAll(SystemColor.black);
        if (waitingForStartPosition)
        {
        }
        else
        {
            if (startPos.HasValue && mousePos.HasValue)
            {
                switch (drawingState)
                {
                    case DrawingState.DrawingLine:
                        overlay.DrawLine(startPos.Value, mousePos.Value + new Vector2Int(1, 1), UIColor);
                        break;
                    case DrawingState.DrawingRectangle:
                        overlay.DrawRectangle(startPos.Value, mousePos.Value + new Vector2Int(1, 1), UIColor);
                        break;
                    case DrawingState.DrawingElipse:
                        overlay.DrawEllipseInRect(startPos.Value, mousePos.Value + new Vector2Int(1, 1), UIColor);
                        break;
                    default:
                        overlay.SetAt(mousePos.Value.x + 1, mousePos.Value.y, UIColor);
                        overlay.SetAt(mousePos.Value.x - 1, mousePos.Value.y, UIColor);
                        overlay.SetAt(mousePos.Value.x, mousePos.Value.y + 1, UIColor);
                        overlay.SetAt(mousePos.Value.x, mousePos.Value.y - 1, UIColor);
                        break;
                }
            }
        }
        for (int iterY = 0; iterY < image.height; iterY++)
        {
            for (int iterX = 0; iterX < image.width; iterX++)
            {
                for (int iterYScale = 0; iterYScale < scale; iterYScale++)
                {
                    for (int iterXScale = 0; iterXScale < scale; iterXScale++)
                    {
                        screenBuffer.SetAt(iterX * scale + iterXScale, iterY * scale + iterYScale,
                            image.GetAt(iterX, iterY));
                        if (overlay.GetAt(iterX, iterY) != SystemColor.black)
                        {
                            screenBuffer.SetAt(iterX * scale + iterXScale, iterY * scale + iterYScale,
                                overlay.GetAt(iterX, iterY));
                        }
                    }
                }
            }
        }
        screenBuffer.DrawLine(image.width * scale, 0, image.width * scale, image.height * scale, SystemColor.white);
        screenBuffer.DrawLine(0, image.height * scale, image.width * scale, image.height * scale, SystemColor.white);
        screenBuffer.Fill(image.width * scale + 2, 2, 20, 20, editingMainColor ? SystemColor.blue : SystemColor.white);
        screenBuffer.Fill(image.width * scale + 2, 22, 20, 20, editingMainColor ? SystemColor.white : SystemColor.blue);
        screenBuffer.Fill(image.width * scale + 4, 4, 16, 16, mainColor);
        screenBuffer.Fill(image.width * scale + 4, 24, 16, 16, secondaryColor);
        screen.SetScreenBuffer(screenBuffer);
    }
    void ChangeColor(ref SystemColor selectedColor)
    {
        int num = ks.ReadDigit(out Key key);
        if (key != Key.None && num != -1)
        {
            selectedColor = num;
        }
        if (ks.ReadKey(Key.Q))
        {
            selectedColor = 10;
        }
        if (ks.ReadKey(Key.W))
        {
            selectedColor = 11;
        }
        if (ks.ReadKey(Key.E))
        {
            selectedColor = 12;
        }
        if (ks.ReadKey(Key.R))
        {
            selectedColor = 13;
        }
        if (ks.ReadKey(Key.T))
        {
            selectedColor = 14;
        }
        if (ks.ReadKey(Key.Y))
        {
            selectedColor = 15;
        }
    }
    void ProcessInput()
    {
        ks = keyHandler.WaitForInputBuffer(true ? 20 : 0);
        if (ks.ReadKey(Key.Escape))
        {
            running = false;
            return;
        }
        if (ks.ReadKey(Key.Plus) || ks.ReadKey(Key.KeypadPlus))
        {
            scale++;
            if (scale > 40)
            {
                scale = 40;
            }
        }
        if (ks.ReadKey(Key.Minus) || ks.ReadKey(Key.KeypadMinus))
        {
            scale--;
            if (scale < 1)
            {
                scale = 1;
            }
        }
        if (ks.ReadKey(Key.A))
        {
            drawingState = DrawingState.Drawing;
            waitingForStartPosition = false;
        }
        if (ks.ReadKey(Key.S))
        {
            drawingState = DrawingState.DrawingLine;
            waitingForStartPosition = true;
            // return;
        }
        if (ks.ReadKey(Key.D))
        {
            drawingState = DrawingState.DrawingRectangle;
            waitingForStartPosition = true;
            // return;
        }
        if (ks.ReadKey(Key.F))
        {
            drawingState = DrawingState.DrawingElipse;
            waitingForStartPosition = true;
            // return;
        }
        Console.Debug(drawingState);
        if (ks.ReadKey(Key.U))
        {
            editingMainColor = !editingMainColor;
        }
        if (editingMainColor)
        {
            ChangeColor(ref mainColor);
        }
        else
        {
            ChangeColor(ref secondaryColor);
        }
        lastPos = mousePos;
        mousePos = mouseHandler.GetScreenPosition();
        if (mousePos.HasValue)
        {
            Console.Debug(mousePos);
            mousePos = new Vector2Int(mousePos.Value.x / scale, mousePos.Value.y / scale);
            Console.Debug(mousePos);
        }
        if (ks.ReadKey(Key.Mouse0, false) || ks.ReadKey(Key.Mouse1, false))
        {
            if (waitingForStartPosition)
            {
                startPos = mousePos;
                waitingForStartPosition = false;
                ks.ReadAndCooldownKey(Key.Mouse1);
                ks.ReadAndCooldownKey(Key.Mouse0);
            }
            else
            {
                switch (drawingState)
                {
                    case DrawingState.DrawingLine:
                    {
                        if (!startPos.HasValue || !mousePos.HasValue)
                        {
                            break;
                        }
                        image.DrawLine(startPos.Value, mousePos.Value + new Vector2Int(1, 1),
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;
                        ks.ReadAndCooldownKey(Key.Mouse1);
                        break;
                    }
                    case DrawingState.DrawingRectangle:
                    {
                        if (!startPos.HasValue || !mousePos.HasValue)
                        {
                            break;
                        }
                        image.DrawRectangle(startPos.Value, mousePos.Value,
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;
                        ks.ReadAndCooldownKey(Key.Mouse1);
                        break;
                    }
                    case DrawingState.DrawingElipse:
                    {
                        if (!startPos.HasValue || !mousePos.HasValue)
                        {
                            break;
                        }
                        image.DrawEllipseInRect(startPos.Value, mousePos.Value + new Vector2Int(1, 1),
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;
                        ks.ReadAndCooldownKey(Key.Mouse1);
                        break;
                    }
                    case DrawingState.Drawing:
                    {
                        if (!lastPos.HasValue || !mousePos.HasValue)
                        {
                            break;
                        }
                        //image.SetAt(mousePos, ks.ReadKey(Key.Mouse0) ? mainColor : secondaryColor);
                        image.DrawLine(lastPos.Value, mousePos.Value,
                            ks.ReadKey(Key.Mouse0) ? mainColor : secondaryColor);
                        break;
                    }
                }
            }
        }
    }
}
//imported: /sys/bins/play
public class play : ExtendedShellProgram
{
    public override string GetName()
    {
        return "play";
    }
    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("note", true, "-nt"),
    };
    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;
    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
       // string wdPath = argPairs.GetValueOrNull("-wd")?.Value;
       // File workingDirectory = fileSystem.GetFileByPath(wdPath);
       string stringNote = argPairs.GetValueOrNull("-nt")?.Value;
int note=13;
	if(!int.TryParse(stringNote,out note)){
  note = AudioHandler.StringToNote(stringNote);
	}
       
        return $"Playing note {note}.";
    }
}
    static readonly IShellProgram[] commands = { new ls(),new mkf(),new paint(),new play() };
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
//imported: /sys/libs/fontLib.ll
//imported: /sys/libs/util.ll
//redefine line Console.Debug(__line__);
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
        File fontFile = fileSystem.GetFileByPath(fontPath);
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
                currentBackground = defaultBackgroundColor;
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
    File currentFile = fileSystem.GetFileByPath("/sys");
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
lock(mainLock){
Console.Debug("shell loop");
            screenBuffer.FillAll(SystemColor.black);
            Draw();
            ProcessInput();
            runtime.Wait();
}       
 }
    }
}
static Shell shell = null;
shell=new Shell();
shell.Run();
#endif