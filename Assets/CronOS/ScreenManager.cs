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

    public void InitScreenBuffer(libs.ScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.width, screenBuffer.height);
        bufferTexture.filterMode = FilterMode.Point;
        rawImage.texture = bufferTexture;
    }
    public void SetScreenBuffer(libs.ScreenBuffer screenBuffer)
    {

        /*  for (int x = 0; x < screen_buffer.width; x++)
          {
              for (int y = 0; y < screen_buffer.height; y++)
              {
                  texture.SetPixel(x, y, screen_buffer.GetPixel(x, y).ToUnityColor());
              }
          }*/
        /* int size = screen_buffer.width * screen_buffer.height;
         Color32[] result = new Color32[size];
         unsafe
         {
             fixed (color* row = &(screen_buffer.texture[0, 0]))
             {
                 color* rowP = row;
                 for (int i = 0; i < size; i++)
                 {
                     result[i] = rowP->ToUnityColor();
                     rowP += 1;
                 }
             }
         }*/

        //    System.Buffer.BlockCopy(screen_buffer.texture, 0, result, 0, screen_buffer.width * screen_buffer.height);
        bufferTexture.SetPixels32(
          Array.ConvertAll(screenBuffer.texture, x => x.GetColor32())
     );
        bufferTexture.Apply();

        //  screen_buffer.buffer.Apply();
        // screen_buffer.buffer;
        rawImage.SetAllDirty();
        //  rawImage.SetMaterialDirty();
    }
}
