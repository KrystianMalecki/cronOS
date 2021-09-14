using Libraries.system;
using Libraries.system.graphics;
using native_system = System;

using native_ue = UnityEngine;

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
        Random randomSystem = new Random();
        ScreenBuffer buffer = Screen.MakeScreenBuffer();
        int orbX = 0;
        int orbY = 0;
        while (true)
        {
            for (int y = 0; y < buffer.height; y++)
            {
                for (int x = 0; x < buffer.width; x++)
                {
                    Color currentColor = new Color(0, 0, 0, 255);
                    if (x == orbX && y == orbY)
                    {
                        currentColor = new Color(255, 255, 255, 255);
                    }
                    buffer.SetPixel(x, y, currentColor);
                    //console.WriteLine("setting at x:"+x+" y:"+y+" color:"+currentColor);
                }
            }
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
            Screen.SetScreenBuffer(buffer);
            Runtime.Wait(1);

        }
    }
}
