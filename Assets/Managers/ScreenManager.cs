
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using libs = Libraries.system;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    public TextMeshProUGUI screen;
    public Texture2D bufferTexture;
    public RawImage rawImage;
    private int pixelWidth;
    private int pixelHeight;
    public static readonly string asciiMap = " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀ɑϐᴦᴨ∑ơµᴛɸϴΩẟ∞∅∈∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ ";
    public Texture2D map;
    public char c;

    [Button]
    public void Draw()
    {
        int texHeight = map.height;
        int index = asciiMap.ToList().FindIndex(x => x == c);
        int posx = index % 16;
        int posy = index / 16;
        Debug.Log($"index {index} with x {posx} and y {posy}");
        bufferTexture = new Texture2D(8, 8);
        pixelWidth = 8;
        pixelHeight = 8;

        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
        libs.RectArray<Color32> array = new libs.RectArray<Color32>(8, 8);
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                array.SetAt(x, y, (Color32)map.GetPixel(posx * 8 + x, ((texHeight - (posy + 1) * 8) + y)));
            }
        }
        bufferTexture.SetPixels32(array.array);
        bufferTexture.Apply();


        rawImage.SetAllDirty();
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void AddToScreen(string text)
    {
        instance.screen.text += text;
    }

    public void InitScreenBuffer(libs.graphics.screen_buffer32.ScreenBuffer32 screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
        pixelWidth = screenBuffer.width;
        pixelHeight = screenBuffer.height;

        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void InitScreenBuffer(libs.graphics.system_screen_buffer.SystemScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
        pixelWidth = screenBuffer.width;
        pixelHeight = screenBuffer.height;
        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void SetScreenBuffer(libs.graphics.screen_buffer32.ScreenBuffer32 screenBuffer)
    {
        libs.RectArray<Color32> array = new libs.RectArray<Color32>(screenBuffer.width, screenBuffer.height);
        for (int y = 0; y < screenBuffer.height; y++)
        {
            for (int x = 0; x < screenBuffer.width; x++)
            {
                array.SetAt(x, screenBuffer.height - y - 1, screenBuffer.GetAt(x, y));
            }
        }
        bufferTexture.SetPixels32(array.array);
        bufferTexture.Apply();


        rawImage.SetAllDirty();
    }
    public void SetScreenBuffer(libs.graphics.system_screen_buffer.SystemScreenBuffer screenBuffer)
    {

        libs.RectArray<Color32> array = new libs.RectArray<Color32>(screenBuffer.width, screenBuffer.height);
        for (int y = 0; y < screenBuffer.height; y++)
        {
            for (int x = 0; x < screenBuffer.width; x++)
            {
                array.SetAt(x, screenBuffer.height - y - 1, screenBuffer.GetAt(x, y).ToColor32());
            }
        }
        bufferTexture.SetPixels32(array.array);
        bufferTexture.Apply();


        rawImage.SetAllDirty();
    }

    public Vector2 GetMousePos()
    {
        Vector2 v2out;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition.ToVector2(), Camera.main, out v2out);

        v2out.x /= rawImage.rectTransform.rect.width;
        v2out.y /= rawImage.rectTransform.rect.height;
        v2out.x += 0.5f;
        v2out.y += 0.5f;
        v2out.y = 1 - v2out.y;
        v2out.x *= pixelWidth;
        v2out.y *= pixelHeight;
        return v2out;
    }
}
