using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using libs = Libraries.system;
using System.Text;

public class ScreenManager : MonoBehaviour
{
    [NaughtyAttributes.ReadOnly] public Texture2D bufferTexture;
    public Transform localTransform;
    public Material mat;
    public RawImage rawImage;
    private int pixelWidth;
    private int pixelHeight;

    public static readonly string asciiMap =
        " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼ !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~⌂ÇüéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒáíóúñÑªº¿⌐¬½¼¡«»░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀ɑϐᴦᴨ∑ơµᴛɸϴΩẟ∞∅∈∩≡±≥≤⌠⌡÷≈°∙·√ⁿ²■ ";

    float minX, minY, maxX, maxY;
    int layerMask = 1 << 6;
    public MeshFilter mesh;
    private libs.mathematics.RectArray<Color32> array;

    public void InitScreenBuffer(libs.output.graphics.IGenericScreenBuffer screenBuffer)
    {
        bufferTexture = new Texture2D(screenBuffer.GetWidth(), screenBuffer.GetHeight());
        pixelWidth = screenBuffer.GetWidth();
        pixelHeight = screenBuffer.GetHeight();
        bufferTexture.filterMode = FilterMode.Point;
        //  rawImage.texture = bufferTexture;
        mat.SetTexture("_MainTex", bufferTexture);
        array = new libs.mathematics.RectArray<Color32>(screenBuffer.GetWidth(), screenBuffer.GetHeight());
    }

    public void SetScreenBuffer(libs.output.graphics.IGenericScreenBuffer screenBuffer)
    {
        for (int y = 0; y < screenBuffer.GetHeight(); y++)
        {
            for (int x = 0; x < screenBuffer.GetWidth(); x++)
            {
                array.SetAt(x, screenBuffer.GetHeight() - y - 1, screenBuffer.GetUnityColorAt(x, y));
            }
        }

        bufferTexture.SetPixels32(array.array);
        bufferTexture.Apply();
    }

    public System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();

    public libs.mathematics.Vector2Int GetMousePostion()
    {
        libs.mathematics.Vector2 v2out = libs.mathematics.Vector2.incorrectVector;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 20, layerMask))
        {
            v2out.x = info.point.x;
            v2out.y = info.point.y;
            v2out.x = Mathf.InverseLerp(minX, maxX, v2out.x);
            v2out.y = Mathf.InverseLerp(minY, maxY, v2out.y);

            // Debug.Log($"x{minX}-{maxX} y{minY}-{maxY} pos{v2out}");
            v2out.y = 1 - v2out.y;
            v2out.y = Mathf.Clamp(v2out.y, 0f, 1f);
            v2out.x = Mathf.Clamp(v2out.x, 0f, 1f);
        }
        else
        {
            return libs.mathematics.Vector2Int.incorrectVector;
        }

        // return libs.mathematics.Vector2Int.incorrectVector;

        return new libs.mathematics.Vector2Int((int)(v2out.x * pixelWidth), (int)(v2out.y * pixelHeight));
    }

    public void Awake()
    {
        minX = transform.TransformPoint(mesh.sharedMesh.vertices[0]).x;
        minY = transform.TransformPoint(mesh.sharedMesh.vertices[0]).y;
        maxX = transform.TransformPoint(mesh.sharedMesh.vertices[3]).x;
        maxY = transform.TransformPoint(mesh.sharedMesh.vertices[3]).y;
    }

    public libs.mathematics.Vector2Int GetMousePos()
    {
        return GetMousePostion();
        Vector2 v2out;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition.ToVector2(),
            Camera.main, out v2out);

        v2out.x /= rawImage.rectTransform.rect.width;
        v2out.y /= rawImage.rectTransform.rect.height;
        v2out.x += 0.5f;
        v2out.y += 0.5f;
        v2out.y = 1 - v2out.y;
        v2out.y = Mathf.Clamp(v2out.y, 0f, 1f);
        v2out.x = Mathf.Clamp(v2out.x, 0f, 1f);
        libs.mathematics.Vector2Int intOut =
            new libs.mathematics.Vector2Int((int)(v2out.x * pixelWidth), (int)(v2out.y * pixelHeight));

        return intOut;
    }
}