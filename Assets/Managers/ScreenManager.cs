using libs = Libraries.system.graphics;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    public TextMeshProUGUI screen;
    public Texture2D bufferTexture;
    public RawImage rawImage;
    private int pixelWidth;
    private int pixelHeight;

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

    public void InitScreenBuffer(libs.screen_buffer32.ScreenBuffer32 screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
        pixelWidth = screenBuffer.width;
        pixelHeight = screenBuffer.height;

        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void InitScreenBuffer(libs.system_screen_buffer.SystemScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
        pixelWidth = screenBuffer.width;
        pixelHeight = screenBuffer.height;
        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void SetScreenBuffer(libs.screen_buffer32.ScreenBuffer32 screenBuffer)
    {

        bufferTexture.SetPixels32(
          Array.ConvertAll(screenBuffer.GetArray(), x => x.GetColor32())
     );
        bufferTexture.Apply();


        rawImage.SetAllDirty();
    }
    public void SetScreenBuffer(libs.system_screen_buffer.SystemScreenBuffer screenBuffer)
    {

        bufferTexture.SetPixels32(
          Array.ConvertAll(screenBuffer.GetArray(), x => x.ToColor32().GetColor32())
     );
        bufferTexture.Apply();


        rawImage.SetAllDirty();
    }

    public (int x, int y) GetMousePos()
    {

        Vector2 v2out;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition.ToVector2(), Camera.main, out v2out);

        var v2outRange = new Vector2(v2out.x / rawImage.rectTransform.rect.width, v2out.y / rawImage.rectTransform.rect.height);
        v2outRange.x += 0.5f;
        v2outRange.y += 0.5f;
        return ((int)(v2outRange.x * pixelWidth), (int)(v2outRange.y * pixelHeight));
    }
}
