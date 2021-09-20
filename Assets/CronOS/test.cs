using Libraries.system;
using Libraries.system.graphics;

using Libraries.system.graphics.color32;
using Libraries.system.graphics.system_color;

using Libraries.system.graphics.screen_buffer32;
using Libraries.system.graphics.system_screen_buffer;

using Libraries.system.graphics.texture32;
using Libraries.system.graphics.system_texture;

using native_system = System;

using native_ue = UnityEngine;
using native_input = UnityEngine.InputSystem;
public class test : native_ue.MonoBehaviour
{
    /*  public CancellationTokenSource source = new CancellationTokenSource();
      public CancellationToken token;
      Thread thread;
      // Start is called before the first frame update
      public void Start()
      {
          // flag: tests
          //  TaskVersion();
          // ThreadVersion();
          // Task.Run(ScreenTest);
      }
      void TaskVersion()
      {
          List<Task> tasks = new List<Task>();
          token = source.Token;
          Task main = Task.Run(async () =>
            {
                Task killer = Task.Run(() =>
               {
                   while (true)
                   {
                       if (token.IsCancellationRequested)
                       {
                           Debug.Log("canceled");
                           token.ThrowIfCancellationRequested();
                       }
                       Task.Delay(500).Wait();
                   }
               }, token);
                Task unusableLoop = Task.Run(() =>
                {
                    while (true)
                    {
                        Debug.Log("waiting" + token.IsCancellationRequested);
                        Task.Delay(1000).Wait();
                    }
                }, token);
                Debug.Log("after setup");

                tasks.Add(killer);
                tasks.Add(unusableLoop);
                token.Register(() =>
                {
                    throw new Exception();

                });
                this.Cancel();
                try
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                    token.ThrowIfCancellationRequested();

                    await Task.WhenAll(tasks.ToArray());
                }
                catch (OperationCanceledException oce)
                {
                    Debug.Log("main canceled");
                    // Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
                    token.ThrowIfCancellationRequested();
                }
                finally
                {
                    source.Dispose();
                }
                Debug.Log("after all");
            }, token);
      }
      void ThreadVersion()
      {
          thread = new Thread(() =>
          {
              while (true)
              {
                  Debug.Log("yes");
                  Thread.Sleep(100);
              }
          });
          thread.Start();

      }
      [Button]
      void Cancel()
      {
          // source.Cancel();
          if (thread != null)
          {
              thread.Abort();
          }
          else
          {
              //todo add better loging
              Debug.LogWarning("thread was null");
          }
      }
      private void OnApplicationQuit()
      {
          Cancel();
      }*/
    
    private void MainCodeTest()
    {
        /*
          using native_system = System;
          using native_ue = UnityEngine;
        */
        Random randomSystem = new Random();
        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitSystemScreenBuffer(buffer);
        int orbX = 0;
        int orbY = 0;
        while (true)
        {

            buffer.FillAll(SystemColor.black);
            buffer.SetPixel(orbX, orbY, SystemColor.white);

            orbX++;
            if (orbX > buffer.width - 1)
            {
                orbY++;
                orbX = 0;
            }
            if (orbY > buffer.height - 1)
            {
                orbX = 0;
                orbY = 0;
            }
            //console.WriteLine("orb is at x:"+orbX+" y:"+orbY);
            AsyncScreen.SetScreenBuffer(buffer);
            //  Runtime.Wait(1);
            Console.Debug(Input.WaitForAny().ToString());

        }
    }
    private void c()
    {
        int a = 0;
        while (true)
        {
            Console.WriteLine(a.ToString());
            a++;
            Runtime.Wait(1);
        }
    }

   

}
