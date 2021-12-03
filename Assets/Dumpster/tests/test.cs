using Libraries.system;


using native_system = System;

using native_ue = UnityEngine;
using static UnityEngine.KeyCode;
using Libraries.system.mathematics;
using Libraries.system.input;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.output.graphics;
using Libraries.system.file_system;
using Libraries.system.output.graphics.system_texture;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output;

public class test : native_ue.MonoBehaviour
{




    private void MainCodeTest()
    {
        const string help = "Press mouse to move to mouse\n"
            + "Arrows to move\n"
            + "Keypad +/- to change speed\n"
            + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = 0;// Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
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
            if (ks.ReadKey(Key.UpArrow))
            {
                orbPos.y -= speed;
            }
            if (ks.ReadKey(Key.DownArrow))
            {
                orbPos.y += speed;
            }
            if (ks.ReadKey(Key.LeftArrow))
            {
                orbPos.x -= speed;
            }
            if (ks.ReadKey(Key.RightArrow))
            {
                orbPos.x += speed;
            }
            if (ks.ReadKey(Key.KeypadPlus))
            {
                speed++;
            }
            if (ks.ReadKey(Key.KeypadMinus))
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
            buffer.DrawTexture(orbPos.x, orbPos.y, playerTexture);
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
            if (ks.ReadKey(Key.Mouse0))
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
    private void drawTextOnScreen()
    {

        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);
        string str = "Hello, world!";
        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);
        void DrawCharAt(int x, int y, char character)
        {
            int index = 0;// Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
        }
        buffer.FillAll(SystemColor.black);
        for (int i = 0; i < str.Length; i++)
        {
            DrawCharAt(i * 8, 0, str.ToCharArray()[i]);
        }
        Screen.SetScreenBuffer(buffer);
    }
    private void MainCodeTest2()
    {
        /*
          using native_system = System;
          using native_ue = UnityEngine;
        */
        Random r = new Random();
        r.NextInt();

        SystemScreenBuffer buffer = new SystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);

        KeyHandler kh = new KeyHandler();
        int orbX = buffer.width / 2;
        int orbY = buffer.height / 2; ;

        SystemColor b = 0;
        KeySequence ks = null;

        while (true)
        {

            buffer.FillAll(SystemColor.black);
            buffer.SetAt(orbX, orbY, b);
            b++;


            AsyncScreen.SetScreenBuffer(buffer);

            ks = kh.WaitForInput();
            //  ks.keys
            if (ks.ReadKey(Key.W))
            {
                orbY++;
            }
            if (ks.ReadKey(Key.S))
            {
                orbY--;
            }
            if (ks.ReadKey(Key.A))
            {
                orbX--;
            }
            if (ks.ReadKey(Key.D))
            {
                orbX++;
            }
            if (orbX > buffer.width - 1)
            {
                orbX = 0;
            }

            if (orbX < 0)
            {
                orbX = buffer.width - 1;
            }
            if (orbY > buffer.height - 1)
            {
                orbY = 0;
            }
            if (orbY < 0)
            {
                orbY = buffer.height - 1;
            }
            Console.Debug("frame" + kh);
            Runtime.Wait(1);
        }
    }
    private void c()
    {
        int a = 0;
        while (true)
        {
            a++;
            Runtime.Wait(1);
        }
    }
    private void d()
    {

//# redefine //# #
        //# include "/System/test_library"
        //commnet

        Console.Debug();
        const string help = "Press mouse to move to mouse\n"
                   + "Arrows to move\n"
                   + "Keypad +/- to change speed\n"
                   + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
        Screen.InitScreenBuffer(buffer);



        Console.Debug();
        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");

        Console.Debug();
        //Console.Debug(fontAtlas.data);
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        Console.Debug(1);
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
            char[] charText = text.ToCharArray();
            for (int i = 0; i < text.Length; i++)
            {
                char c = charText[i];
                if (c == '\r')
                {
                    posX = x;
                    continue;
                }
                else if (c == '\n')
                {
                    posX = x;
                    posY += 8;
                    continue;
                }
                DrawCharAt(posX, posY, c);
                posX += 8;
            }
        }




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
            if (ks.ReadKey(Key.UpArrow))
            {
                orbPos.y -= speed;
            }
            if (ks.ReadKey(Key.DownArrow))
            {
                orbPos.y += speed;
            }
            if (ks.ReadKey(Key.LeftArrow))
            {
                orbPos.x -= speed;
            }
            if (ks.ReadKey(Key.RightArrow))
            {
                orbPos.x += speed;
            }
            if (ks.ReadKey(Key.KeypadPlus))
            {
                speed++;
            }
            if (ks.ReadKey(Key.KeypadMinus))
            {
                speed--;
            }
        }


        void ClampPositionToFrame()
        {
            if (orbPos.x > buffer.width - 1)
            {
                orbPos.x = buffer.width - 1;
            }
            if (orbPos.x < 0)
            {
                orbPos.x = 0;
            }
            if (orbPos.y > buffer.height - 1)
            {
                orbPos.y = buffer.height - 1;
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
            buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
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
                text = text.AddInputSpecial(input, ks);
            }
            CheckMovement(ks);
            if (ks.ReadKey(Key.Mouse0))
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


