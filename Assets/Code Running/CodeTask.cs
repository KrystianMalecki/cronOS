using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using InternalLogger;


[Serializable]
public class CodeTask
{
    //  public static readonly string ThreadCodeTaskID = "codeTask";
    //  public static readonly string ThreadID = "threadID";


    public Thread thread;
    public CodeObject codeObject;
    private static FieldInfo fieldInfoOfStackTrace = typeof(Exception).GetField("captured_traces", BindingFlags.NonPublic | BindingFlags.Instance);


    public void RunCode(CodeObject codeObject)
    {
        this.codeObject = codeObject;
        thread = new Thread(RunAsync);

        thread.IsBackground = true;



        thread.Start();
    }
    private async void RunAsync()
    {
        try
        {
            //todo-maybe change back
            /*   script = CSharpScript.Create(codeObject.code.ToBytes().ToEncodedString(), ScriptManager.instance.scriptOptionsBuffer
                      .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                      .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace)).WithEmitDebugInformation(true)
                   );
               scriptRunner = script.CreateDelegate();
               await scriptRunner.Invoke();*/


            //  CSharpScript.Create()
            /*  codeObject.code = "try{\n" +
                  codeObject.code + "\n}catch(System.Exception e){\n" +
                  "Console.Debug(\"before\");\nConsole.Debug(e);\n}";*/
            Debug.Log(codeObject.code);
            await CSharpScript.EvaluateAsync(codeObject.code
                 , ScriptManager.instance.scriptOptionsBuffer
                .WithReferences(codeObject.libraries.ConvertAll(x => Assembly.Load(x.assembly)))
                .WithImports(codeObject.libraries.ConvertAll(x => x.nameSpace))
                 .WithEmitDebugInformation(true)
                );
            /*  await CSharpScript.EvaluateAsync(@"string n= null; 
  n.ToString();", ScriptOptions.Default
                .WithEmitDebugInformation(false)
               );*/

        }
        catch (ThreadAbortException tae)
        {
            FlagLogger.LogWarning(LogFlags.SystemWarning, "Aborted thread running code.\nData: " + tae.Message);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            //todo 0 make reflection to grab that
            //e>base>base>captured_traces>[0]>frames>[0]>this
            try
            {

                Exception current = e;
                //   System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(e);

                /*tring stackIndent = "";
                 for (int i = 0; i < st.FrameCount; i++)
                 {
                     // Note that at this level, there are four
                     // stack frames, one for each method invocation.
                     System.Diagnostics.StackFrame sf = st.GetFrame(i);
                     string combine = "";
                     combine += (stackIndent + " Method: {0}",
                         sf.GetMethod()) + "\n";
                     combine += (stackIndent + " File: {0}",
                         sf.GetFileName()) + "\n";
                     combine += (stackIndent + " Line Number: {0}",
                         sf.GetFileLineNumber()) + "\n";
                     Debug.Log(combine);
                     stackIndent += "  ";
                 }
                 Debug.Log(st.ToString());
                 Debug.Log(st.GetFrames().GetValuesToString());*/
                /* BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;
                 Debug.Log(current.GetType());
                 MemberInfo[] mis = current.GetType().GetMembers(bf);
                 Debug.Log(mis.GetValuesToString());

                 FieldInfo[] fis = current.GetType().GetFields(bf);
                 Debug.Log(fis.GetValuesToString());*/
                Debug.Log(((System.Diagnostics.StackTrace[])fieldInfoOfStackTrace.GetValue(e))[0].GetFrame(0));
                /* System.Diagnostics.StackTrace[] stacktrace = fi.GetValue(current) as System.Diagnostics.StackTrace[];
                 for (int i = 0; i < stacktrace.Length; i++)
                 {
                     for (int j = 0; j < stacktrace[i].FrameCount; j++)
                     {
                         System.Diagnostics.StackFrame sf = stacktrace[i].GetFrame(j);
                         Debug.Log(sf);
                     }
                 }
                 Debug.Log(stacktrace.GetValuesToString("\n"));

                 PropertyInfo[] pis = current.GetType().GetProperties(bf);
                 Debug.Log(pis.GetValuesToString());*/







                /*Debug.LogError(e.Message);

                  Debug.LogError(e.StackTrace);

                  Debug.LogError(e.InnerException);
                  Debug.LogError(e.InnerException);*/
            }
            catch (Exception ee)
            {
                Debug.LogException(ee);
            }
        }
        Debug.Log("end");
        //  Destroy();


    }

    ~CodeTask()
    {

        Destroy();
    }
    public void Destroy()
    {
        FlagLogger.LogWarning(LogFlags.SystemWarning, "Destroying CodeTask");
        ScriptManager.instance.RemoveCodeTask(this);
        if (thread != null)
        {
            thread.Abort();
            thread.Join();//todo-maybe fix
            thread = null;

        }
        else
        {
            FlagLogger.LogWarning(LogFlags.SystemWarning, "thread was null");

        }
    }

}
/*#include "/System/test_library"
//commnet

const string help = "Press mouse to move to mouse\n"
           + "Arrows to move\n"
           + "Keypad +/- to change speed\n"
           + "Type to type☻";
        SystemScreenBuffer buffer = new SystemScreenBuffer(Screen.screenWidth, Screen.screenHeight);
        Screen.InitScreenBuffer(buffer);

*/

// File fontAtlas =null;// FileSystem.GetFileByPath("/System/fontAtlas");
//Console.Debug(fontAtlas.data);
//  SystemTexture fontTexture = SystemTexture.FromData(fontAtlas.data);

/*   Console.Debug(1);
   void DrawCharAt(int x, int y, char character)
   {
       int index = Runtime.CharToByte(character);
       int posx = index % 16;
       int posy = index / 16;
       buffer.SetTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8));
   }
   void DrawStringAt(int x, int y, string text)
   {
       //  Console.Debug(x + " " + y);
       int posX = x;
       int posY = y;
       for (int i = 0; i < text.Length; i++)
       {
           char c = text.ToCharArray()[i];
           //Console.Debug(c,(int)c,(int)'\n');
           if (c == '\n' || c == '\r')
           {
               posX = x;
               posY += 8;
               continue;
           }
           DrawCharAt(posX, posY, c);
           posX += 8;
       }
   }




   Vector2Int orbPos = new Vector2Int(buffer.width / 2, buffer.height / 2);
   Vector2Int mousePos = new Vector2Int(0, 0);

   string position = "";
   string text = "";
   int speed = 1;
   KeyHandler kh = new KeyHandler();
   KeySequence ks = null;
   bool flasher = false;

   void CheckMovement(KeySequence ks)
   {
       if (ks.HasKey(Key.UpArrow))
       {
           orbPos.y -= speed;
       }
       if (ks.HasKey(Key.DownArrow))
       {
           orbPos.y += speed;
       }
       if (ks.HasKey(Key.LeftArrow))
       {
           orbPos.x -= speed;
       }
       if (ks.HasKey(Key.RightArrow))
       {
           orbPos.x += speed;
       }
       if (ks.HasKey(Key.KeypadPlus))
       {
           speed++;
       }
       if (ks.HasKey(Key.KeypadMinus))
       {
           speed--;
       }
   }


   void ClampPositionToFrame()
   {
       if (orbPos.x > buffer.width - 1)
       {
           orbPos.x = buffer.width - 1;
       }
       if (orbPos.x < 0)
       {
           orbPos.x = 0;
       }
       if (orbPos.y > buffer.height - 1)
       {
           orbPos.y = buffer.height - 1;
       }
       if (orbPos.y < 0)
       {
           orbPos.y = 0;
       }
   }
   void Draw()
   {
       DrawStringAt(8, 0, help);

       position = $"X:{orbPos.x},Y:{orbPos.y},Speed:{speed}";

       DrawStringAt(0, 4 * 8, position);
       buffer.DrawLine(mousePos.x, mousePos.y, orbPos.x, orbPos.y, SystemColor.white);
       buffer.SetAt(orbPos.x, orbPos.y, SystemColor.yellow);
       DrawStringAt(0, 5 * 8, text);
       buffer.Fill(0, 0, 8, 8, flasher ? SystemColor.red : SystemColor.blue);
       flasher = !flasher;
       AsyncScreen.SetScreenBuffer(buffer);
   }
   void ProcessInput()
   {
       ks = kh.WaitForInput();
       string input = KeyHandler.GetInputAsString();
       if (input != "")
       {
           text = text.AddInput(input);
       }
       CheckMovement(ks);
       if (ks.HasKey(Key.Mouse0))
       {
           mousePos = MouseHander.GetScreenPosition();
       }
       ClampPositionToFrame();
   }
   while (true)
   {
       buffer.FillAll(SystemColor.black);

       Draw();
       ProcessInput();

       Runtime.Wait(1);
   }*/