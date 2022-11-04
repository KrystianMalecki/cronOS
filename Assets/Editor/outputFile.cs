#if false

public class Shell
{
    File currentFile = FileSystem.GetFileByPath("/sys");
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
shell = new Shell();
shell.Run();
#endif