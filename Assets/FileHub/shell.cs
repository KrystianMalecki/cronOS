#if false
//#top using static Hardware;
/*#include "/sys/libs/binsLib.lib"*/
//#include "/sys/libs/fontLib.lib"
public class Shell
{
    public Shell thisShell;
    public string balls;
    static KeySequence bufferKeySequence = null;
    static string prefix = "";
    static string inputText = "";
    static string bufferInput = "";
    static string consoleText = "";
    static int historyPointer = 0;
    static List<string> history = new List<string>();

    static string binariesFolder = "/sys/bins/";
    static void UpdatePrefix()
    {
    Debugger.Debug(currentFile);
        prefix = currentFile.GetFullPath() + ">";
    }
    public void WriteToConsole(string line){
        Debugger.Debug("WriteToConsole");
        consoleText += '\n' + line;
    }    
   static void  Draw()
    {
        DrawStringAt(screenBuffer, 0, 0, consoleText);
        int consoleY = consoleText.CountEncounters('\n') + 1;
        DrawStringAt(screenBuffer, 0, consoleY * 8, prefix);
        DrawStringAt(screenBuffer, prefix.Length * 8, consoleY * 8, inputText);
        Screen.SetScreenBuffer(screenBuffer);
    }
    static void ProcessInput()
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
    static string PraseCommand(string input)
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
        else if (parts[0] == "compile")
        {
            if (parts.Length > 1)
            {
                File file = FileSystem.GetFileByPath(parts[1], currentFile);
                if (file != null)
                {
                    File compileFile = Runtime.Compile(file);
                    if (compileFile != null)
                    {
                        compileFile = file.Parent.SetChild(compileFile);
                        return $"Compiled '{file.GetFullPath()}' to '{compileFile.GetFullPath()}'";
                    }
                    else
                    {
                        return $"Failed to compile '{file.GetFullPath()}'";
                    }
                }
                else
                {
                    return $"File '{parts[1]}' not found";
                }
            }

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
        File binaryFile = FileSystem.GetFileByPath(binariesFolder + command + HardwareInternal.compilationExtension);
        if (binaryFile == null)
        {
            binaryFile = FileSystem.GetFileByPath(binariesFolder + command);

        }
        Runtime.Execute(binaryFile, parts.Skip(1).ToArray());
        return "";
    }
    public static void Main()
    {
    Debugger.Debug("shell start");
Debugger.DisplayLoadedAssembliesNumber();
        Shell s = new Shell();
     s.thisShell = s;
        s.balls = "balls";
        Hardware.AddStatic(s);
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

#endif