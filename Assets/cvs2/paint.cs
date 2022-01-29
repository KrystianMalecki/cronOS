#if false //changeToTrue
//todo 1 add reading of args, add to the list of programs
//todo 3 square/circle pen
//todo 4 eraser? 
using System;
using System.Collections.Generic;
using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.input;
using Libraries.system.mathematics;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.output.graphics.system_texture;
using Libraries.system.shell;


*/

enum DrawingState
{
    Drawing,
    DrawingLine,
    DrawingRectangle,
    DrawingElipse,
}

public class paint : ExtendedShellProgram
{
    string filePath = "/sys";
    string fileName = "output";
    bool waitingForStartPosition = false;
    SystemTexture image = null;
    SystemTexture overlay = null;
    File imageFile = null;
    int scale = 4;

    Vector2Int mousePos = new Vector2Int(0, 0);
    SystemColor mainColor = SystemColor.white;
    SystemColor UIColor = SystemColor.dark_gray;
    SystemColor secondaryColor = SystemColor.black;
    bool editingMainColor = true;
    DrawingState drawingState = DrawingState.Drawing;
    Vector2Int startPos = Vector2Int.zero; //Vector2Int.incorrectVector;
    Vector2Int lastPos = Vector2Int.zero; //Vector2Int.incorrectVector;

    KeySequence ks = null;
    bool running = true;

    public override string GetName()
    {
        return "paint";
    }

    protected override string InternalRun(Dictionary<string, string> argPairs)
    {
        lock (lockMain)
        {
            if (fileSystem.TryGetFile(filePath + "/" + fileName, out File file))
            {
                image = SystemTexture.FromData(file.data);
                imageFile = file;
            }

            else
            {
                image = new SystemTexture(100, 100);
                imageFile = Drive.MakeFile("output", new byte[0]);
            }

            overlay = new SystemTexture(image.width, image.height);


            while (running)
            {
                Draw();
                ProcessInput();
                runtime.Wait();
            }


            byte[] data = image.ToData();
            imageFile.data = data;
            File parent = fileSystem.GetFileByPath(filePath);
            parent.AddChild(imageFile);
        }

        return "paint finished work.";
    }

    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("-wd", "working directory", true)
    };


    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;


    void Draw()
    {
        screenBuffer.FillAll(SystemColor.black);
        overlay.FillAll(SystemColor.black);
        if (waitingForStartPosition)
        {
        }
        else
        {
            switch (drawingState)
            {
                case DrawingState.DrawingLine:
                    overlay.DrawLine(startPos, mousePos + new Vector2Int(1, 1), UIColor);
                    break;
                case DrawingState.DrawingRectangle:
                    overlay.DrawRectangle(startPos, mousePos + new Vector2Int(1, 1), UIColor);
                    break;
                case DrawingState.DrawingElipse:
                    overlay.DrawEllipseInRect(startPos, mousePos + new Vector2Int(1, 1), UIColor);
                    break;
                default:
                    overlay.SetAt(mousePos.x + 1, mousePos.y, UIColor);
                    overlay.SetAt(mousePos.x - 1, mousePos.y, UIColor);
                    overlay.SetAt(mousePos.x, mousePos.y + 1, UIColor);
                    overlay.SetAt(mousePos.x, mousePos.y - 1, UIColor);
                    break;
            }
        }

        for (int iterY = 0; iterY < image.height; iterY++)
        {
            for (int iterX = 0; iterX < image.width; iterX++)
            {
                for (int iterYScale = 0; iterYScale < scale; iterYScale++)
                {
                    for (int iterXScale = 0; iterXScale < scale; iterXScale++)
                    {
                        screenBuffer.SetAt(iterX * scale + iterXScale, iterY * scale + iterYScale,
                            image.GetAt(iterX, iterY));
                        if (overlay.GetAt(iterX, iterY) != SystemColor.black)
                        {
                            screenBuffer.SetAt(iterX * scale + iterXScale, iterY * scale + iterYScale,
                                overlay.GetAt(iterX, iterY));
                        }
                    }
                }
            }
        }

        screenBuffer.DrawLine(image.width * scale, 0, image.width * scale, image.height * scale, SystemColor.white);
        screenBuffer.DrawLine(0, image.height * scale, image.width * scale, image.height * scale, SystemColor.white);
        screenBuffer.Fill(image.width * scale + 2, 2, 20, 20, editingMainColor ? SystemColor.blue : SystemColor.white);
        screenBuffer.Fill(image.width * scale + 2, 22, 20, 20, editingMainColor ? SystemColor.white : SystemColor.blue);

        screenBuffer.Fill(image.width * scale + 4, 4, 16, 16, mainColor);
        screenBuffer.Fill(image.width * scale + 4, 24, 16, 16, secondaryColor);

        screen.SetScreenBuffer(screenBuffer);
    }

    void ChangeColor(ref SystemColor selectedColor)
    {
        int num = ks.ReadDigit(out Key key);
        if (key != Key.None && num != -1)
        {
            selectedColor = num;
        }

        if (ks.ReadKey(Key.Q))
        {
            selectedColor = 10;
        }

        if (ks.ReadKey(Key.W))
        {
            selectedColor = 11;
        }

        if (ks.ReadKey(Key.E))
        {
            selectedColor = 12;
        }

        if (ks.ReadKey(Key.R))
        {
            selectedColor = 13;
        }

        if (ks.ReadKey(Key.T))
        {
            selectedColor = 14;
        }

        if (ks.ReadKey(Key.Y))
        {
            selectedColor = 15;
        }
    }

    void ProcessInput()
    {
        ks = keyHandler.WaitForInputBuffer(true ? 20 : 0);

        if (ks.ReadKey(Key.Escape))
        {
            running = false;
            return;
        }

        if (ks.ReadKey(Key.Plus) || ks.ReadKey(Key.KeypadPlus))
        {
            scale++;
            if (scale > 40)
            {
                scale = 40;
            }
        }

        if (ks.ReadKey(Key.Minus) || ks.ReadKey(Key.KeypadMinus))
        {
            scale--;
            if (scale < 1)
            {
                scale = 1;
            }
        }

        if (ks.ReadKey(Key.A))
        {
            drawingState = DrawingState.Drawing;
            waitingForStartPosition = false;
        }

        if (ks.ReadKey(Key.S))
        {
            drawingState = DrawingState.DrawingLine;
            waitingForStartPosition = true;
            // return;
        }

        if (ks.ReadKey(Key.D))
        {
            drawingState = DrawingState.DrawingRectangle;
            waitingForStartPosition = true;
            // return;
        }

        if (ks.ReadKey(Key.F))
        {
            drawingState = DrawingState.DrawingElipse;
            waitingForStartPosition = true;
            // return;
        }

        Console.Debug(drawingState);
        if (ks.ReadKey(Key.U))

        {
            editingMainColor = !editingMainColor;
        }

        if (editingMainColor)
        {
            ChangeColor(ref mainColor);
        }
        else
        {
            ChangeColor(ref secondaryColor);
        }

        lastPos = mousePos;
        mousePos = mouseHandler.GetScreenPosition();
        mousePos.x = mousePos.x / scale;
        mousePos.y = mousePos.y / scale;
        if (ks.ReadKey(Key.Mouse0, false) || ks.ReadKey(Key.Mouse1, false))
        {
            if (waitingForStartPosition)
            {
                startPos = mousePos;
                waitingForStartPosition = false;
                ks.ReadAndCooldownKey(Key.Mouse1);
                ks.ReadAndCooldownKey(Key.Mouse0);
            }
            else
            {
                switch (drawingState)
                {
                    case DrawingState.DrawingLine:
                    {
                        image.DrawLine(startPos, mousePos + new Vector2Int(1, 1),
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;
                        ks.ReadAndCooldownKey(Key.Mouse1);

                        break;
                    }
                    case DrawingState.DrawingRectangle:
                    {
                        image.DrawRectangle(startPos, mousePos,
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;
                        ks.ReadAndCooldownKey(Key.Mouse1);

                        break;
                    }
                    case DrawingState.DrawingElipse:
                    {
                        image.DrawEllipseInRect(startPos, mousePos + new Vector2Int(1, 1),
                            ks.ReadAndCooldownKey(Key.Mouse0) ? mainColor : secondaryColor);
                        drawingState = DrawingState.Drawing;

                        ks.ReadAndCooldownKey(Key.Mouse1);

                        break;
                    }
                    case DrawingState.Drawing:
                    {
                        //image.SetAt(mousePos, ks.ReadKey(Key.Mouse0) ? mainColor : secondaryColor);
                        image.DrawLine(lastPos, mousePos,
                            ks.ReadKey(Key.Mouse0) ? mainColor : secondaryColor);

                        break;
                    }
                }
            }
        }
    }
}
#endif