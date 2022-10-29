#if false
//#include "/sys/libs/binsLib.lib"
//#include "/sys/libs/fontLib.lib"
//#top using static Shell;
Debugger.Debug("shell start");
public class Shell
{
    public static File currentFile = FileSystem.GetFileByPath("/sys");
    public static SystemScreenBuffer screenBuffer = new SystemScreenBuffer();
    KeySequence bufferKeySequence = null;
    string prefix = "";
    string inputText = "";
    string bufferInput = "";
    string consoleText = "";
    int historyPointer = 0;
    List<string> history = new List<string>();

    static string binariesFolder = "/sys/bins/";
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
            historyPointer = Math.Clamp(historyPointer, 0, history.Count);
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
            historyPointer = Math.Clamp(historyPointer, 0, history.Count);
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
            if (parts.Length == 1)
            {
                currentFile = FileSystem.GetFileByPath("/");
            }
            else
            {
                File file = FileSystem.GetFileByPath(parts[1], currentFile);
                if (file != null)
                {
                    currentFile = file;
                }
                else
                {
                    return $"File '{parts[1]}' not found";
                }
            }
            UpdatePrefix();
            return "";
        }
        else
        {
            return FindAndExecuteCommand(input);
        }
        return $"Couldn't find command `{parts[0]}`.";
    }
    public static string FindAndExecuteCommand(string input)
    {
        List<string> parts = input.SplitSpaceQ();

        string command = parts[0];
        string restOfArgs = (input.Length <= command.Length) ? "" : input.Substring(command.Length);
        string output = $"Command '{command}' not found!";
        Runtime.Execute(binariesFolder + parts[0], string.Join(" ", parts.Skip(1).ToArray()));
        return "";
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
shell = new Shell();
shell.Run();
#endif