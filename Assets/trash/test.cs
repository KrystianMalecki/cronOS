using Libraries.system;
using Libraries.system.graphics;

using Libraries.system.graphics.color32;
using Libraries.system.graphics.system_color;

using Libraries.system.graphics.screen_buffer32;
using Libraries.system.graphics.system_screen_buffer;

using Libraries.system.graphics.texture32;
using Libraries.system.graphics.system_texture;

using native_system = System;

using native_ue = UnityEngine;
using static UnityEngine.KeyCode;
using Libraries.system.filesystem;
using Libraries.system.mathematics;
using Libraries.system.input;

public class test : native_ue.MonoBehaviour
{

    public static test instance;
    public int count1 = 0;
    public int count2 = 0;
    public int count3 = 0;
    public int count4 = 0;
    public int count5 = 0;

    public int counts0 = 0;
    public int counts1 = 0;
    public int counts2 = 0;
    public int counts3 = 0;
    public int counts4 = 0;
    public int counts5 = 0;
    public int counts6 = 0;
    public int counts7 = 0;
    public int counts8 = 0;
    public int counts9 = 0;
    public int counts10 = 0;

    //string inp=KeyHandler.WaitForStringInput();
    public void Awake()
    {
        instance = this;

    }
    private void MainCodeTest()
    {
        const string help = "Press mouse to move to mouse\n"
            + "Arrows to move\n"
            + "Keypad +/- to change speed\n"
            + "Type to type☻";
        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("C:/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.SetTexture(x, y, fontTexture.GetPart(posx * 8, (posy) * 8, 8, 8));
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
            buffer.DrawLine2(mousePos.x, mousePos.y, orbPos.x, orbPos.y, SystemColor.white);
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
    private void drawTextOnScreen()
    {

        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);
        string str = "Hello, world!";
        File fontAtlas = FileSystem.GetFileByPath("C:/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);
        void DrawCharAt(int x, int y, char character)
        {
            int index = Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.SetTexture(x, y, fontTexture.GetPart(posx * 8, (posy) * 8, 8, 8));
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

        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
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
            if (ks.HasKey(Key.W))
            {
                orbY++;
            }
            if (ks.HasKey(Key.S))
            {
                orbY--;
            }
            if (ks.HasKey(Key.A))
            {
                orbX--;
            }
            if (ks.HasKey(Key.D))
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
            Console.WriteLine(a.ToString());
            a++;
            Runtime.Wait(1);
        }
    }
    private void d()
    {

        const string help = "Press mouse to move to mouse\n"
            + "Arrows to move\n"
            + "Keypad +/- to change speed\n"
            + "Type to type☻";
        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitScreenBuffer(buffer);


        File fontAtlas = FileSystem.GetFileByPath("C:/System/fontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

        void DrawCharAt(int x, int y, char character)
        {
            int index = Screen.GetCharacterIndex(character);
            int posx = index % 16;
            int posy = index / 16;
            buffer.SetTexture(x, y, fontTexture.GetPart(posx * 8, (posy) * 8, 8, 8));
        }
        void DrawStringAt(int x, int y, string text)
        {
            Console.Debug(x + " " + y);
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
                orbPos = MouseHander.GetScreenPosition();
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


