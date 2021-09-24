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
        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void InitScreenBuffer(libs.system_screen_buffer.SystemScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
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
}
