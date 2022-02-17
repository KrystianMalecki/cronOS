/*using BigGustave;

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;*/

using System;
using System.Collections;
using System.ComponentModel;
using Libraries.system.mathematics;
using Libraries.system.output.graphics.screen_buffer32;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_texture;
using UnityEngine;
using sio = System.IO;
/*using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Reflection;
using Libraries.system.file_system;
using Libraries.system.output.graphics.system_texture;
using System.Linq;
using helper;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.mask_texture;
using Cinemachine.Utility;*/
using static HardwareBox;
using Color32 = Libraries.system.output.graphics.color32.Color32;
using Console = Libraries.system.output.Console;

public class HardwareBox
{
    public static Hardware hardware;
}

//HardwareBox.hardware = hardware;
public class C
{
}

/*C c = new C();
c.run();*/
public class NativeTest : MonoBehaviour
{
    public void Start()
    {
        SystemTexture image = new SystemTexture();
        image.DrawLine(0, 0, 0, 0, 9);
        
        /* Color32[] palette =
         {
             new Color32(0, 0, 0, 255),
             new Color32(255, 0, 0, 255)
         };
 
 
         PaletteTexture pt = new PaletteTexture(new RectArray<byte>(2, 2, new byte[] { 0, 1, 0, 1 }), palette);
         var data = pt.ToData();
         Debug.Log(data.ToFormatedString());
         PaletteTexture pt2 = new PaletteTexture();
         pt2.FromData(data);
         Debug.Log(pt2.ToData().ToFormatedString());*/
        if (false)
        {
            RectArray<long> rect = new RectArray<long>(2, 2, new long[] { 0, 1, 0, 1 });

            var data = rect.ToData(sizeof(long) * 8, BitConverter.GetBytes);
            Debug.Log(data.ToFormattedString());
            RectArray<long> rect2 =
                RectArray<long>.FromData(data, sizeof(long) * 8, x => new long[] { BitConverter.ToInt64(x) });
            Debug.Log(rect2.ToData(sizeof(long) * 8, BitConverter.GetBytes).ToFormattedString());
        }

        if (false)
        {
            PaletteTexture pt = new PaletteTexture(2, 2,
                new Color32[]
                    { SystemColor.black.ToColor32(), SystemColor.red.ToColor32(), SystemColor.blue.ToColor32() });
            pt.SetAt(0, 0, 2);
            pt.SetAt(1, 1, 2);
            var data2 = pt.ToData();
            Debug.Log(data2.ToFormattedString());
            PaletteTexture pt2 = PaletteTexture.FromDataUsingColorCount(data2, 3);


            Debug.Log(pt2.ToData().ToFormattedString());
        }

        if (false)
        {
            SystemTexture pt = new SystemTexture(2, 2);
            pt.SetPalette(new[]
                { SystemColor.black.ToColor32(), SystemColor.red.ToColor32(), SystemColor.blue.ToColor32() });

            pt.SetAt(0, 0, 2);
            pt.SetAt(1, 1, 2);
            var data2 = pt.ToData();
            Debug.Log(data2.ToFormattedString());
            SystemTexture pt2 = SystemTexture.FromData(data2);


            Debug.Log(pt2.ToData().ToFormattedString());
        }

        if (false)
        {
            bool[] bools = new bool[] { true, true, true, true, true, true, true, false, true };
            byte[] nibbles = new byte[] { 1, 8, 15 };
            byte[] dibits = new byte[] { 3, 1, 2, 3, 1, 3, 3, 3 };

            Debug.Log(bools.ConvertAllBoolsToBytes().ConvertBytesToBools().ToFormattedString());
            Debug.Log(nibbles.ConvertAllNibblesToBytes().ConvertBytesToNibbles().ToFormattedString());
            Debug.Log(dibits.ConvertAllDibitsToBytes().ConvertBytesToDibits().ToFormattedString());
        }
        //   Debug.Log(128.ToBytes().ToFormattedString());
        //    Debug.Log(new BitArray(128.ToBytes()).ToDisplayString());


// SystemColor sc = 9;
/* Console.Debug(sc);

 Console.Debug(sc - 5);
 Console.Debug(sc + 5);
 Console.Debug(sc * 5);
 Console.Debug(sc / 5);

 Console.Debug(sc - 11);
 Console.Debug(sc + 11);
 SystemColor sc2 = SystemColor.blue;
 Console.Debug(sc2);
 Console.Debug(sc2.Darken());
 Console.Debug(sc2.Lighten());
 Console.Debug(sc2.Lighten());*/
/*  Debug.Log(ProcessorManager.mainEncoding.GetType().Assembly.Location);
  Debug.Log("  # define   lol(x,y)   Cosnole.Log($\"lol {x} {y}\")".SplitSpaceQ().ToFormatedString("-"));
  Debug.Log(new Path("./..", FileSystem.GetFileByPath("/")));


  Debug.Log(new Path("/System/"));
  Debug.Log(new Path("/System/programs"));
  Debug.Log(new Path("./ls", FileSystem.GetFileByPath("/System/programs")));
  Debug.Log(new Path("./..", FileSystem.GetFileByPath("/System/programs")));
  Debug.Log(new Path("./../programs", FileSystem.GetFileByPath("/System/programs")));
  Debug.Log(new Path("./../can'tfind", FileSystem.GetFileByPath("/System/programs")));
  Debug.Log(new Path("./../../programs", FileSystem.GetFileByPath("/System/programs/ls")));
  Debug.Log(new Path("./../../programs/ls", FileSystem.GetFileByPath("/System/programs/ls")));
*/
/* Debug.Log(SystemColor.sizeOf);
 MaskTexture st = new MaskTexture(2, 2);
 st.SetAt(0, 0, true);
 st.SetAt(0, 1, false);
 st.SetAt(1, 0, true);
 st.SetAt(1, 1, false);
 Debug.Log(st.array.ToFormatedString());

 byte[] data = st.ToData();
 Debug.Log(data.ToFormatedString());
 MaskTexture st2 = MaskTexture.FromData(data);
 Debug.Log(st2.array.ToFormatedString());
 byte[] data2 = st2.ToData();
 Debug.Log(data2.ToFormatedString());
Debug.Log(default(SystemColor).value);*/


// FileSystem.MakeFile("/a/b/c/d/e.e/f.f/hh/gj");
    }

/*  public static string GetPath(string rawPath, File workingDirectory)
  {

  }*/
}