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
using native_input = UnityEngine.InputSystem;
using static UnityEngine.KeyCode;
using Libraries.system.filesystem;
using Libraries.system.math;

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


    public void Awake()
    {
        instance = this;

    }
    private void MainCodeTest()
    {
        /*
          using native_system = System;
          using native_ue = UnityEngine;
        */
        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        File playerTextureFile = FileSystem.GetFileByPath("C:/System/dupa.dll");
        SystemTexture playerTexture = SystemTexture.FromData(playerTextureFile.data);
        Screen.InitSystemScreenBuffer(buffer);
        KeyHandler kh = new KeyHandler();
        KeySequence ks = null;
        int orbX = buffer.width / 2;
        int orbY = buffer.height / 2; ;
        SystemColor b = 0;
        while (true)
        {

            buffer.FillAll(SystemColor.black);
            buffer.SetTexture(orbX, orbY, playerTexture);

            // b++;
            AsyncScreen.SetScreenBuffer(buffer);
            Vector2Int v2 = MouseHander.GetScreenPosition();
            orbX = v2.v2.x;
            orbY = v2.v2.y;
            /* ks = kh.WaitForInput();
             if (ks.HasKey(KeyboardKey.UpArrow) || ks.HasKey(KeyboardKey.W))
             {
                 orbY--;
             }
             if (ks.HasKey(KeyboardKey.DownArrow) || ks.HasKey(KeyboardKey.S))
             {
                 orbY++;
             }
             if (ks.HasKey(KeyboardKey.LeftArrow) || ks.HasKey(KeyboardKey.A))
             {
                 orbX--;
             }
             if (ks.HasKey(KeyboardKey.RightArrow) || ks.HasKey(KeyboardKey.D))
             {
                 orbX++;
             }*/
            if (orbX > buffer.width - playerTexture.width)
            {
                orbX = buffer.width - playerTexture.width;
            }
            if (orbX < 0)
            {
                orbX = 0;
            }
            Console.Debug($"{orbY} is > than {buffer.height - playerTexture.height} or {buffer.height} - {playerTexture.height}");
            if (orbY > buffer.height - playerTexture.height)
            {
                orbY = buffer.height - playerTexture.height;
            }
            if (orbY < 0)
            {
                orbY = 0;
            }

            Runtime.Wait(1);
        }
    }
    private void drawTextOnScreen()
    {

        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitSystemScreenBuffer(buffer);
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
        Screen.InitSystemScreenBuffer(buffer);

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

}


