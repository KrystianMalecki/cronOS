using Helpers;
using Libraries.system;
using Libraries.system.debug;
using Libraries.system.file_system;
using Libraries.system.input;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.mask_texture;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Hardware;

public class Shell
{
    static KeySequence bufferKeySequence = null;
    static string prefix = "";
    static string inputText = "";
    static string bufferInput = "";
    static string consoleText = "";
    static int historyPointer = 0;
    static List<string> history = new List<string>();
    static string binariesFolder = "/sys/bins/";
    static string workingDirectoryArgumentFlag = "-wd";

    static void UpdatePrefix()
    {
        prefix = currentFile.GetFullPath() + ">";
    }
    public static void WriteToConsole(string line)
    {
        consoleText += '\n' + line;
    }
    static void Draw()
    {
        FontLibrary.DrawStringAt(screenBuffer, 0, 0, consoleText);
        int consoleY = consoleText.CountEncounters('\n') + 1;
        FontLibrary.DrawStringAt(screenBuffer, 0, consoleY * 8, prefix);
        FontLibrary.DrawStringAt(screenBuffer, prefix.Length * 8, consoleY * 8, inputText);
        Screen.SetScreenBuffer(screenBuffer);
    }
    static void WaitAndProcessInput()
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
        string[] parts = GlobalHelper.SplitTextBySpaces(input, true);
        if (parts[0] == "cd")
        {
            if (parts.Length == 1)
            {
                ChangeDirectory("/");
            }
            else
            {
                ChangeDirectory(parts[1]);
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
            if (FindAndExecuteCommand(input))
            {
                return "";
            }
        }
        return $"Couldn't find command `{parts[0]}`.";
    }
    public static bool ChangeDirectory(File file)
    {
        if (file != null)
        {
            currentFile = file;
            UpdatePrefix();
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool ChangeDirectory(string path)
    {
        File file = FileSystem.GetFileByPath(path, currentFile);
        if (file != null)
        {
            return ChangeDirectory(file);

        }
        else
        {
            WriteToConsole($"File at'{path}' not found");
            return false;
        }
    }
    public static bool FindAndExecuteCommand(string input)
    {
        string[] parts = GlobalHelper.SplitTextBySpaces(input + " " + workingDirectoryArgumentFlag + " " + currentFile.GetFullPath(), true);


        string command = parts[0];
        File binaryFile = FileSystem.GetFileByPath(binariesFolder + command + HardwareInternal.compilationExtension);
        if (binaryFile == null)
        {
            // binaryFile = FileSystem.GetFileByPath(binariesFolder + command);
            return false;
        }
        return Runtime.Execute(binaryFile, parts.Skip(1).ToArray());

    }
    public static void Main()
    {
        Screen.InitScreenBuffer(screenBuffer);
        UpdatePrefix();
        while (true)
        {
            Debugger.Debug("shell loop");
            screenBuffer.FillAll(SystemColor.black);
            Draw();
            WaitAndProcessInput();
            Runtime.Wait();


        }
    }
}

