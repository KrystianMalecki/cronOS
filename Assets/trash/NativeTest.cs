using BigGustave;

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using sio = System.IO;
using TMPro;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Reflection;
using Libraries.system.file_system;
using Libraries.system.output.graphics.system_texture;

public class NativeTest : MonoBehaviour
{
    //Keyboard keyboard;
    HashSet<KeyCode> keysPressed;
    public SystemTexture st;
    public byte[] arr;
    public TextMeshProUGUI t;
    [Button]
    public void makeAll()
    {
        t.text = "";
        for (byte i = 1; i < 255; i++)

        {
            string text = GetAsNewChar(i);
            Debug.Log(i + " " + text);
            t.text += text;
        }
    }
    public unsafe string GetAsNewChar(byte b, int count = 1)
    {
        return Encoding.GetEncoding(437).GetString(&b, count);
    }
    public string Datas
    {
        get { return datas2; }
        set
        {
            datas2 = value;
        }
    }
    public string datas2;
    [HorizontalLine]
    public File f;
    [HorizontalLine]
    public FilePermission permission;
    public string serIn;

    public string serOut;
    public void Start()
    {
        string code = @"0
balls
balls line 2
line 3
test
";
        Debug.Log(code);
        Debug.Log(ScriptManager.ParseIncludes(code));
       /* var s = CSharpScript.Create("begin", ScriptManager.instance.scriptOptionsBuffer
           );

        Debug.Log(s.Code);
        s.ContinueWith("end");
        Debug.Log(s.Code);
        Debug.Log(s.ContinueWith("end").Code);*/
        /*  serOut = JsonUtility.ToJson(CSharpScript.Create(serIn, ScriptManager.instance.scriptOptionsBuffer
                 .WithReferences(ScriptManager.allLibraryDatas.ConvertAll(x => Assembly.Load(x.assembly)))
                 .WithImports(ScriptManager.allLibraryDatas.ConvertAll(x => x.nameSpace))
                 ));*/

    }
    public void Update()
    {
        // StartCoroutine(ie());
        //  Keyboard.current.onTextInput
    }

    [Button]
    public void toArr()
    {
        using (var stream = System.IO.File.OpenRead(@"H:\Projects\cronOS\Assets\cronos_test1.png"))
        {
            Png image = Png.Open(stream);
            arr = new byte[image.Height * image.Width];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    arr[y * image.Width + x] = image.GetPixel(x, y).R;
                }
            }
        }
    }
    public void makePng()
    {
        var builder = PngBuilder.Create(2, 2, false);

        var red = new Pixel(255, 0, 0);

        builder.SetPixel(red, 0, 0);
        builder.SetPixel(255, 120, 16, 1, 1);

        using (var memory = new sio.MemoryStream())
        {
            builder.Save(memory);

            //  return memory.ToArray();
        }
    }
    [Button]
    public void convertCopy()
    {
        Debug.Log(GUIUtility.systemCopyBuffer);

    }
    [Button]
    public void makeBytes()
    {
        arr = st.ToData();
    }
    [AllowNesting, SerializeField]
    public File textureFile;
    public SystemTextureFile stf = new SystemTextureFile();
    public SystemTextureFile outSTF = new SystemTextureFile();
    [Button]
    public void write()
    {
        /* using (sio.MemoryStream ms = textureFile.Open())
         {
             ms.Write(stf.width.ToBytes());
             ms.Write(stf.height.ToBytes());
             ms.Write(Array.ConvertAll(stf.colors, x => x.value));
         }*/
        textureFile.data = stf.ToData();
    }
    [Button]
    public void read()
    {
        outSTF = SystemTextureFile.FromData(textureFile.data);
        /*  using (sio.MemoryStream ms = textureFile.Open())
          {
              byte[] width = new byte[2];
              byte[] height = new byte[2];
              ms.Read(width);
              ms.Read(height);
              outSTF.width = BitConverter.ToInt16(width, 0);
              outSTF.height = BitConverter.ToInt16(height, 0);
              byte[] colors = new byte[outSTF.width * outSTF.height];
              ms.Read(colors);
              outSTF.colors = Array.ConvertAll(stf.colors, x => new Libraries.system.graphics.system_color.SystemColor(x));
          }*/

    }
    /* public void Update()
     {
         //  Debug.Log(String.Join(" , ", KeyboardInputHelper.GetCurrentKeys()));
         //  keysPressed.Add(KeyboardInputHelper.GetCurrentKeys());
     }*/
    public void OnDestroy()
    {

    }


}
