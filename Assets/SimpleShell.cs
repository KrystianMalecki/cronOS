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
public class SimpleShell : UnityEngine.MonoBehaviour
{


    public string Begin()
    {
        string prefix = "";
        File currentFile = FileSystem.GetFileByPath("/System");
        string console = "";

        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = character;
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
                    text = "";
                }
                else
                {
                    text += c;
                }
            }
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
}