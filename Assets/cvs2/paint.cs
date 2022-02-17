#if false //changeToTrue
//todo 3 square/circle pen
//todo 4 eraser? 

#if true
using System.Collections.Generic;
using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.input;
using Libraries.system.mathematics;
using Libraries.system.output;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.output.graphics.system_texture;
using Libraries.system.shell;
using static Shell;

public class Shell
{
    public static Runtime runtime = new Runtime();
    public static FileSystem fileSystem = new FileSystem();
    public static KeyHandler keyHandler = new KeyHandler();
    public static MouseHandler mouseHandler = new MouseHandler();
    public static Screen screen = new Screen();
    public static object lockMain = new object();
    File currentFile = fileSystem.GetFileByPath("/sys");

    public static SystemScreenBuffer screenBuffer = new SystemScreenBuffer();
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
            lock (lockMain)
            {
                Console.Debug("shell loop");
                screenBuffer.FillAll(SystemColor.black);
                Draw();
                ProcessInput();
                runtime.Wait();
            }
        }
    }
}

#endif
//*/

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

    protected override string InternalRun(Dictionary<string, string> argPairs)
    {
        Console.Debug(argPairs.ToFormattedString2());
        if (argPairs.TryGetValue("-wd", out string workingPath))
        {
        }

        if (argPairs.TryGetValue("-n", out string name))
        {
        }
        else
        {
            name = "image.img";
        }

        int height = 10;
        if (argPairs.TryGetValue("-h", out string heightText))
        {
            height = int.Parse(heightText);
        }

        int width = 10;
        if (argPairs.TryGetValue("-w", out string widthText))
        {
            width = int.Parse(widthText);
        }


        if (argPairs.TryGetValue("-f", out filePath))
        {
        }
        else
        {
            filePath = workingPath;
        }

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
        lock (lockMain)
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
            parent.AddChild(imageFile);
        }

        Console.Debug("paint lock off");
        keyHandler.DumpInputBuffer();
        keyHandler.DumpStringInputBuffer();
        return "paint finished work.";
    }

    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-f", "current file", true),
        new AcceptedArgument("-nf", "new file", false),
        new AcceptedArgument("-n", "name", true),
        new AcceptedArgument("-w", "width", true),
        new AcceptedArgument("-h", "height", true),
        new AcceptedArgument("-cp", "color palette", true),
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
            mousePos = new Vector2Int(mousePos.Value.x / scale , mousePos.Value.y / scale);
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

#endif