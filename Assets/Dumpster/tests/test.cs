#if false
using System.Text.RegularExpressions;
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
using Libraries.system.output.graphics.color32;
using Libraries.system.output.graphics.mask_texture;

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
            int index = 0; // Screen.GetCharacterIndex(character);
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
            int index = 0; // Screen.GetCharacterIndex(character);
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
        int orbY = buffer.height / 2;
        ;

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
    private void all()
    {
        #region fontLibrary.ll
        File fontFile = FileSystem.GetFileByPath("/System/defaultFontMask");
         MaskTexture fontTexture = MaskTexture.FromData(fontFile.data);

        
         Regex colorTagRegex = new Regex(@"^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))");
         Regex backgroundColorTagRegex =
 new Regex(@"^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc)))");
         Regex nonParseTagRegex = new Regex(@"^(\s*?\/?((raw)|(no-parsing)|(np)))");



         void DrawColoredCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character,
            SystemColor foreground, SystemColor background )
        {
            if (systemScreenBuffer == null)
            {
                
                return;

            }

            int index = Runtime.CharToByte(character);
            int posx = index % 16;
            int posy = index / 16;

            systemScreenBuffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8).Convert<SystemColor>
                    (o => (o ? foreground : background)),
                fontTexture.transparencyFlag);
        }

         void DrawColoredStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
            SystemColor foreground, SystemColor background, bool enableTags = true)
        {
            SystemColor currentForeground = foreground, currentBackground = background;
            int posX = x;
            int posY = y;
            char[] charText = text.ToCharArray();
            bool parseTags = true;
            for (int i = 0; i < text.Length; i++)
            {
                char character = charText[i];
                if (character == '\r')
                {
                    posX = x;
                    continue;
                }
                else if (character == '\n')
                {
                    posX = x;
                    posY += 8;
                    continue;
                }
                else if (enableTags) //tags enabled
                {
                    if (character == '<') //found tag
                    {
                        if (i < 1 || charText[i - 1] != '\\') //not escaped
                        {
                            int endTagIndex = text.IndexOf('>', i);
                            if (endTagIndex != -1)
                            {
                                string tagText = text.Substring(i + 1, endTagIndex - i - 1).Trim();
                                if (nonParseTagRegex.IsMatch(tagText))
                                {
                                    parseTags = (tagText.StartsWith("/"));
                                    i = endTagIndex;
                                    continue;
                                }

                                if (parseTags)
                                {
                                    if (colorTagRegex.IsMatch(tagText))
                                    {
                                        if (tagText.StartsWith("/"))
                                        {
                                            currentForeground = foreground;
                                        }
                                        else
                                        {
                                            int equalsIndex = tagText.IndexOf('=');
                                            string color = tagText.Substring(equalsIndex + 1);
                                            Console.Debug(
                                                $"match c! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                            if (color.Length == 1)
                                            {
                                                currentForeground = Runtime.HexToByte(color);
                                            }
                                        }
                                    }

                                    if (backgroundColorTagRegex.IsMatch(tagText))
                                    {
                                        if (tagText.StartsWith("/"))
                                        {
                                            currentBackground = background;
                                        }
                                        else
                                        {
                                            int equalsIndex = tagText.IndexOf('=');
                                            string color = tagText.Substring(equalsIndex + 1);
                                            Console.Debug(
                                                $"match bckc! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                            if (color.Length == 1)
                                            {
                                                currentBackground = Runtime.HexToByte(color);
                                            }
                                        }
                                    }




                                    i = endTagIndex;

                                    continue;
                                }
                            }
                        }
                    }
                }
                else if (character == '\\')
                {
                    continue;
                }

                DrawColoredCharAt(systemScreenBuffer, posX, posY, character, currentForeground, currentBackground);
                posX += 8;
            }
        }

         void DrawCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character)
        {
            DrawColoredCharAt(systemScreenBuffer, x, y, character, SystemColor.white, SystemColor.black);
        }

         void DrawStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text, bool enableTags = true)
        {
            DrawColoredStringAt(systemScreenBuffer, x, y, text, SystemColor.white, SystemColor.black, enableTags);
        }

         string GetColorTag(SystemColor color)
        {
            return $"<color={Runtime.ByteToHex(color, false)}>";
        }
         string GetBackgroundColorTag(SystemColor color)
        {
            return $"<color={Runtime.ByteToHex(color, false)}>";
        }
        
        #endregion

        #region advanced CT
        //# include "/System/libraries/fontLibrary.ll"


        const string help = "Press mouse to move to mouse\n"
                       + "Arrows to move\n"
                       + "Keypad +/- to change speed\n"
                       + "Type to type☻";
            SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
            Screen.InitScreenBuffer(buffer);




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
                DrawStringAt(buffer, 8, 0, help);

                position = $"X:{orbPos.x},Y:{orbPos.y},Speed:{speed}";

                DrawStringAt(buffer, 0, 4 * 8, position);
                buffer.DrawLine(mousePos.x, mousePos.y, orbPos.x, orbPos.y, SystemColor.white);
                buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
                DrawStringAt(buffer, 0, 5 * 8, text);
                buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
                flasher = !flasher;
                AsyncScreen.SetScreenBuffer(buffer);
            }
            void ProcessInput()
            {
                ks = kh.WaitForInput();
                string input = KeyHandler.GetInputAsString();
                text += kh.TryGetCombinedSymbol(ref ks);
                text = text.AddInputSpecial(input, ks);

                Console.Debug(ks);
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
        #endregion
    }

    void e()
    {
        File fontAtlas = FileSystem.GetFileByPath("/System/defaultFontAtlas");
        SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);
        MaskTexture mask = new MaskTexture(fontTexture.width, fontTexture.height);
        //copy all the data from the font texture to the mask texture
        for (int y = 0; y < fontTexture.height; y++)
        {
            for (int x = 0; x < fontTexture.width; x++)
            {
                mask.SetAt(x, y, fontTexture.GetAt(x, y) == SystemColor.white);
            }
        }

        File maskFile = FileSystem.MakeFile("/System/defaultFontMask");
        maskFile.data = mask.ToData();
    }

    void ee()
    {
        Regex r = new Regex("<.+?=.+?>");

        string RemoveTags(string input)
        {
            //regex to remove all text between <> but keep then <> are escaped by \
            return r.Replace(input, "");
        }

        string input = "<color=F>text</color=F>";
        Console.Debug(input, RemoveTags(input));

        Console.Debug(input, RemoveTags(input));
    }

    /*regexes
     ^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))  get color
     ^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc))) get background color

     */
    //using System.Text.RegularExpressions;

}
#endif