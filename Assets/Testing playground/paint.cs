#if false
using Libraries.system.input;
using Libraries.system.mathematics;
using Libraries.system.output.graphics;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;

//#include "/sys/libs/fontLib.ll"


SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
screen.InitScreenBuffer(buffer);


Vector2Int mousePos = new Vector2Int(0, 0);


KeySequence ks = null;
SystemColor pickedColor = SystemColor.white;


void Draw()
{
    buffer.DrawLine(mousePos, orbPos, SystemColor.white);
    buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
    buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);

    screen.SetScreenBuffer(buffer);
}

void ProcessInput()
{
    ks = keyHandler.WaitForInput();
    string input = keyHandler.GetInputAsString();
    // text += keyHandler.TryGetCombinedSymbol(ref ks);
    //  text = text.AddInputSpecial(input, ks);

    mousePos = mouseHandler.GetScreenPosition();
}

while (true)
{
    Draw();
    ProcessInput();
    runtime.Wait();
}
#endif