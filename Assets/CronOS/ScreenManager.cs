using libraries.system.graphics;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    public TextMeshProUGUI screen;
    public Texture2D screenBuffer;
    public RawImage rawImage;
    public int x;
    public int y;
    public Color color;
    [Button]
    public void SayPixel()
    {
        Debug.Log(screenBuffer.GetPixel(x, y).ToString());
    }
    [Button]
    public void SetPixel()
    {
        screenBuffer.SetPixel(x, y, color);
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
        screenBuffer = new Texture2D(4, 4);
        screenBuffer.filterMode = FilterMode.Point;

    }

    public static void AddToScreen(string text)
    {
        instance.screen.text += text;
    }
    public void SetPixel(int x, int y, libraries.system.graphics.color color)
    {
        screenBuffer.SetPixel(x, y, color.ToUnityColor());

    }
    [Button]
    public void SetScreenBuffer()
    {
        screenBuffer.Apply();
        rawImage.texture = screenBuffer;
        rawImage.SetAllDirty();
        //  rawImage.SetMaterialDirty();
    }
    public void SetScreenBuffer(screen_buffer screen_buffer)
    {
        Texture2D texture;
        texture = new Texture2D(4, 4);
        texture.filterMode = FilterMode.Point;
        for (int x = 0; x < screen_buffer.width; x++)
        {
            for (int y = 0; y < screen_buffer.height; y++)
            {
                texture.SetPixel(x, y, screen_buffer.GetPixel(x, y).ToUnityColor());
            }
        }
        texture.Apply();
        rawImage.texture = texture;
        rawImage.SetAllDirty();
        //  rawImage.SetMaterialDirty();
    }
}
