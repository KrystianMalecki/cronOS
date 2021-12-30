/*using BigGustave;

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;*/

using Libraries.system.output;
using Libraries.system.output.graphics.system_colorspace;
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

public class HardwareBox
{
    public static Hardware hardware;
}

//HardwareBox.hardware = hardware;
public class C
{
    public void add()
    {
        hardware.number++;
        say();
    }

    public void say()
    {
        Console.Debug(hardware.number);
    }

    public void run()
    {
        while (true)
        {
            add();
            hardware.runtime.Wait(100);
        }
    }
}

/*C c = new C();
c.run();*/
public class NativeTest : MonoBehaviour
{
    public void Start()
    {
        SystemColor sc = 9;
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