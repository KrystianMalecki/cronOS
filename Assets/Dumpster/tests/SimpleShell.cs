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

public class SimpleShell : UnityEngine.MonoBehaviour
{

    public void Begin()
    {
        string prefix = "";
        File currentFile = FileSystem.GetFileByPath("/System");
        string console = "";

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
                }
                else
                {
                    text += c;
                }
            }
        }
        string PraseCommand(string input)
        {
            string[] parts = input.Split(' ');
            switch (parts[0])
            {
                case "cd":
                    {
                        File f = FileSystem.GetFileByPath(FileSystem.MakeAbsolutePath(parts[1], currentFile));
                        if (f == null)
                        {
                            return $"Couldn't find file {parts[1]}!";
                        }
                        currentFile = f;
                        UpdatePrefix();
                        return "";
                    }
                case "ls":
                    {
                        return GetChildren("", currentFile, "", (parts.Length > 1 ? (parts[1] == "-a") : false));
                    }
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
}