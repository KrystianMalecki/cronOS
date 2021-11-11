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
    [Button]
    public void Begin()
    {
        const string help = "Press mouse to move to mouse\n"
                + "Arrows to move\n"
                + "Keypad +/- to change speed\n"
                + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("C:/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = character;
            int posx = index % 16;
            int posy = index / 16;
            buffer.SetTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
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
        bool flasher = false;

      


       
        void Draw()
        {
            DrawStringAt(8, 0, help);


            DrawStringAt(0, 5 * 8, text);
            buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
            flasher = !flasher;
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