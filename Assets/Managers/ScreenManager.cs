
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using libs = Libraries.system;
using System.Text;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    public TextMeshProUGUI screen;
    [NaughtyAttributes.ReadOnly]
    public Texture2D bufferTexture;
    public RawImage rawImage;
    private int pixelWidth;
    private int pixelHeight;
    public static readonly string asciiMap = " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀ɑϐᴦᴨ∑ơµᴛɸϴΩẟ∞∅∈∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ ";

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

    public void InitScreenBuffer(libs.output.graphics.IGenericScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.GetWidth(), screenBuffer.GetHeight());
        pixelWidth = screenBuffer.GetWidth();
        pixelHeight = screenBuffer.GetHeight();

        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void SetScreenBuffer(libs.output.graphics.IGenericScreenBuffer screenBuffer)
    {
        libs.mathematics.RectArray<Color32> array = new libs.mathematics.RectArray<Color32>(screenBuffer.GetWidth(), screenBuffer.GetHeight());
        for (int y = 0; y < screenBuffer.GetHeight(); y++)
        {
            for (int x = 0; x < screenBuffer.GetWidth(); x++)
            {
                array.SetAt(x, screenBuffer.GetHeight() - y - 1, screenBuffer.GetUnityColorAt(x, y));
            }
        }
        bufferTexture.SetPixels32(array.array);
        bufferTexture.Apply();


        rawImage.SetAllDirty();


    }


    public libs.mathematics.Vector2Int GetMousePos()
    {
        Vector2 v2out;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition.ToVector2(), Camera.main, out v2out);

        v2out.x /= rawImage.rectTransform.rect.width;
        v2out.y /= rawImage.rectTransform.rect.height;
        v2out.x += 0.5f;
        v2out.y += 0.5f;
        v2out.y = 1 - v2out.y;
        v2out.y = Mathf.Clamp(v2out.y, 0f, 1f);
        v2out.x = Mathf.Clamp(v2out.x, 0f, 1f);
        libs.mathematics.Vector2Int intOut = new libs.mathematics.Vector2Int((int)(v2out.x * pixelWidth), (int)(v2out.y * pixelHeight));

        return intOut;
    }
}
