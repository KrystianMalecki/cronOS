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

public class SimpleShell : UnityEngine.MonoBehaviour
{
    public void Begin()
    {
        const string help = "Press mouse to move to mouse\n"
                + "Arrows to move\n"
                + "Keypad +/- to change speed\n"
                + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth,Screen.screenHeight);//todo 0 change!
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("C:/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.SetTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
        }
        void DrawStringAt(int x, int y, string text)
        {
            //  Console.Debug(x + " " + y);
            int posX = x;
            int posY = y;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text.ToCharArray()[i];
                //Console.Debug(c,(int)c,(int)'\n');
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

        File playerTextureFile = FileSystem.GetFileByPath("C:/System/dupa");
        SystemTexture playerTexture = SystemTexture.FromData(playerTextureFile.data);

        Vector2Int orbPos = new Vector2Int(buffer.width / 2, buffer.height / 2);
        Vector2Int mousePos = new Vector2Int(0, 0);

        string position = "";
        string text = "";
        int speed = 1;
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;
        bool flasher = false;

        void CheckMovement(KeySequence ks)
        {
            if (ks.HasKey(Key.UpArrow))
            {
                orbPos.y -= speed;
            }
            if (ks.HasKey(Key.DownArrow))
            {
                orbPos.y += speed;
            }
            if (ks.HasKey(Key.LeftArrow))
            {
                orbPos.x -= speed;
            }
            if (ks.HasKey(Key.RightArrow))
            {
                orbPos.x += speed;
            }
            if (ks.HasKey(Key.KeypadPlus))
            {
                speed++;
            }
            if (ks.HasKey(Key.KeypadMinus))
            {
                speed--;
            }
        }


        void ClampPositionToFrame()
        {
            if (orbPos.x > buffer.width - playerTexture.width)
            {
                orbPos.x = buffer.width - playerTexture.width;
            }
            if (orbPos.x < 0)
            {
                orbPos.x = 0;
            }
            if (orbPos.y > buffer.height - playerTexture.height)
            {
                orbPos.y = buffer.height - playerTexture.height;
            }
            if (orbPos.y < 0)
            {
                orbPos.y = 0;
            }
        }
        void Draw()
        {
            DrawStringAt(8, 0, help);

            position = $"X:{orbPos.x},Y:{orbPos.y},Speed:{speed}";

            DrawStringAt(0, 4 * 8, position);
            buffer.DrawLine(mousePos.x, mousePos.y, orbPos.x, orbPos.y, SystemColor.white);
            buffer.SetTexture(orbPos.x, orbPos.y, playerTexture);
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
            CheckMovement(ks);
            if (ks.HasKey(Key.Mouse0))
            {
                mousePos = MouseHander.GetScreenPosition();
            }
            ClampPositionToFrame();
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