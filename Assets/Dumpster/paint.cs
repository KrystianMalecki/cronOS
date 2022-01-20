#if false //changeToTrue
/*using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.input;
using Libraries.system.mathematics;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.output.graphics.system_texture;

static Runtime runtime = null;
static FileSystem fileSystem = null;
static KeyHandler keyHandler = null;
static MouseHandler mouseHandler = null;
static Screen screen = null;
*/

string filePath = "/sys";
string fileName = "output";

SystemTexture image = null;
int scale = 20;
if (fileSystem.TryGetFile(filePath + "/" + fileName, out File file))
{
    image = SystemTexture.FromData(file.data);
}
else
{
    image = new SystemTexture(20, 20);
}


Vector2Int mousePos = new Vector2Int(0, 0);
SystemColor mainColor = SystemColor.white;
SystemColor secondaryColor = SystemColor.red;
bool editingMainColor = true;
SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
screen.InitScreenBuffer(buffer);
KeySequence ks = null;
bool running = true;

void Draw()
{
    buffer.FillAll(SystemColor.black);

    for (int iterY = 0; iterY < image.height; iterY++)
    {
        for (int iterX = 0; iterX < image.width; iterX++)
        {
            for (int iterYScale = 0; iterYScale < scale; iterYScale++)
            {
                for (int iterXScale = 0; iterXScale < scale; iterXScale++)
                {
                    buffer.SetAt(iterX * scale + iterXScale, iterY * scale + iterYScale, image.GetAt(iterX, iterY));
                }
            }
        }
    }

    buffer.DrawLine(image.width * scale, 0, image.width * scale, image.height * scale, SystemColor.white);
    buffer.DrawLine(0, image.height * scale, image.width * scale, image.height * scale, SystemColor.white);
    buffer.Fill(image.width * scale + 2, 2, 20, 20, editingMainColor ? SystemColor.blue : SystemColor.white);
    buffer.Fill(image.width * scale + 2, 22, 20, 20, editingMainColor ? SystemColor.white : SystemColor.blue);

    buffer.Fill(image.width * scale + 4, 4, 16, 16, mainColor);
    buffer.Fill(image.width * scale + 4, 24, 16, 16, secondaryColor);

    screen.SetScreenBuffer(buffer);
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
    ks = keyHandler.WaitForInput();
    if (ks.ReadKey(Key.Escape))
    {
        running = false;
        return;
    }

    if (ks.ReadKey(Key.Plus) || ks.ReadKey(Key.KeypadPlus))
    {
        scale++;
    }

    if (ks.ReadKey(Key.Minus) || ks.ReadKey(Key.KeypadMinus))
    {
        scale--;
    }

    if (ks.ReadAndCooldownKey(Key.U))
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

    mousePos = mouseHandler.GetScreenPosition();
    mousePos.x = mousePos.x / scale;
    mousePos.y = mousePos.y / scale;
    if (ks.ReadKey(Key.Mouse1))
    {
        image.SetAt(mousePos, secondaryColor);
    }

    if (ks.ReadKey(Key.Mouse0))
    {
        image.SetAt(mousePos, mainColor);
    }
}

while (running)
{
    Draw();
    ProcessInput();
    runtime.Wait();
}


byte[] data = image.ToData();
File parent = fileSystem.GetFileByPath(filePath);
File imageFile = Drive.MakeFile("output", data);
parent.AddChild(imageFile);
#endif