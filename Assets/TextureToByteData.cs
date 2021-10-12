using Libraries.system.graphics.system_color;
using Libraries.system.graphics.system_texture;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore;

public class TextureToByteData : MonoBehaviour
{
    public Texture2D texture;
    public SystemTexture systemTexture;
    public byte[] data;
    public Color32 color;
    public TMP_FontAsset asset;
    public Texture2D character;
    public Color[] colors;
    public string l;
    [Button]
    public void s()
    {
        GlyphRect gr = asset.characterLookupTable[l[0]].glyph.glyphRect;
        Debug.Log(gr.x + " " + gr.y + " " + gr.width + " " + gr.height);
        colors = asset.atlasTexture.GetPixels(gr.x, gr.y, gr.width, gr.height);
        systemTexture = new SystemTexture(gr.width, gr.height);


        for (int y = 0; y < systemTexture.height; y++)
        {
            for (int x = 0; x < systemTexture.width; x++)
            {
                Color c = colors[y * systemTexture.width + x];

                if (c.a == 0)
                {
                    c = Color.black;
                }


                byte b = (byte)ColorConstants.FindNearestID(ColorConstants.SystemColors, ((Color32)c).ToCronosColor());
                systemTexture.SetAt(x, systemTexture.height - y - 1, b);
            }
        }
      ;
        /*   systemTexture.array = Array.ConvertAll(texture.GetPixels32(), x =>
           {
               byte b = (byte)ColorConstants.FindNearestID(ColorConstants.SystemColors, x.ToCronosColor());
               return new SystemColor(b);
           });*/
        data = systemTexture.ToData();
    }
    [Button]
    public void text()
    {
        Debug.Log(ColorConstants.FindNearest(ColorConstants.SystemColors, color.ToCronosColor()));
    }
    [Button]
    public void ConvertToData()
    {


        systemTexture = new SystemTexture(texture.width, texture.height);

        /*   bufferTexture.SetPixels32(
             Array.ConvertAll(screenBuffer.GetArray(), x => (Color32)x.ToColor32())
        );*/
        Color32[] colors = texture.GetPixels32();
        for (int y = 0; y < systemTexture.height; y++)
        {
            for (int x = 0; x < systemTexture.width; x++)
            {
                byte b = (byte)ColorConstants.FindNearestID(ColorConstants.SystemColors, colors[y * systemTexture.width + x].ToCronosColor());
                systemTexture.SetAt(x, systemTexture.height - y - 1, b);
            }
        }
      ;
        /*   systemTexture.array = Array.ConvertAll(texture.GetPixels32(), x =>
           {
               byte b = (byte)ColorConstants.FindNearestID(ColorConstants.SystemColors, x.ToCronosColor());
               return new SystemColor(b);
           });*/
        data = systemTexture.ToData();


    }
}