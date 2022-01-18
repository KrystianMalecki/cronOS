#if false
//#include "/sys/libs/fontLib.ll"


            const string help = "Press mouse to move to mouse\n"
                       + "Arrows to move\n"
                       + "Keypad +/- to change speed\n"
                       + "Type to type☻";
            SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
            screen.InitScreenBuffer(buffer);




            Vector2Int orbPos = new Vector2Int(buffer.width / 2, buffer.height / 2);
            Vector2Int mousePos = new Vector2Int(0, 0);

            string position = "";
            string text = "";
            int speed = 1;
            KeySequence ks = null;
            bool flasher = false;
	SystemColor bckCol = SystemColor.red;

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
	if (ks.ReadKey(Key.P))
                {
                    bckCol.Lighten();
                }
if (ks.ReadKey(Key.O))
                {
                    bckCol.Darken();
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

                buffer.DrawLine(mousePos, orbPos, SystemColor.white);
                buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
                DrawStringAt(buffer, 0, 5 * 8, text);
                buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
                flasher = !flasher;
                screen.SetScreenBuffer(buffer);
            }
            void ProcessInput()
            {
                ks = keyHandler.WaitForInput();
                string input = keyHandler.GetInputAsString();
                text += keyHandler.TryGetCombinedSymbol(ref ks);
                text = text.AddInputSpecial(input, ks);


                CheckMovement(ks);
                if (ks.ReadKey(Key.Mouse0))
                {
                    mousePos = mouseHandler.GetScreenPosition();
                }
                ClampPositionToFrame();
            }
            while (true)
            {
                buffer.FillAll(bckCol);
                Draw();
                ProcessInput();
                runtime.Wait();

            }
#endif