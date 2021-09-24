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
using static UnityEngine.KeyCode;
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
                           FlagLogger.Log("canceled");
                           token.ThrowIfCancellationRequested();
                       }
                       Task.Delay(500).Wait();
                   }
               }, token);
                Task unusableLoop = Task.Run(() =>
                {
                    while (true)
                    {
                        FlagLogger.Log("waiting" + token.IsCancellationRequested);
                        Task.Delay(1000).Wait();
                    }
                }, token);
                FlagLogger.Log("after setup");

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
                    FlagLogger.Log("main canceled");
                    // Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
                    token.ThrowIfCancellationRequested();
                }
                finally
                {
                    source.Dispose();
                }
                FlagLogger.Log("after all");
            }, token);
      }
      void ThreadVersion()
      {
          thread = new Thread(() =>
          {
              while (true)
              {
                  FlagLogger.Log("yes");
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
              FlagLogger.LogWarning("thread was null");
          }
      }
      private void OnApplicationQuit()
      {
          Cancel();
      }*/
    public static test instance;
    public int count1 = 0;
    public int count2 = 0;
    public int count3 = 0;
    public int count4 = 0;
    public int count5 = 0;

    public int counts0 = 0;
    public int counts1 = 0;
    public int counts2 = 0;
    public int counts3 = 0;
    public int counts4 = 0;
    public int counts5 = 0;
    public int counts6 = 0;
    public int counts7 = 0;
    public int counts8 = 0;
    public int counts9 = 0;
    public int counts10 = 0;


    public void Awake()
    {
        instance = this;
    }
    private void MainCodeTest()
    {
        /*
          using native_system = System;
          using native_ue = UnityEngine;
        */
        SystemScreenBuffer buffer = Screen.MakeSystemScreenBuffer();
        Screen.InitSystemScreenBuffer(buffer);
        KeyboardHandler kh = KeyboardHandler.Init();
        int orbX = buffer.width / 2;
        int orbY = buffer.height / 2; ;
        SystemColor b = 0;
       // test.instance.counts0++;
        while (true)
        {

            buffer.FillAll(SystemColor.black);
          //  test.instance.counts1++;
            buffer.SetAt(orbX, orbY, b);
           // test.instance.counts2++;
            // orbX++;
            b++;


            AsyncScreen.SetScreenBuffer(buffer);
           // test.instance.counts3++;
            if (kh.GetKeyDown(KeyboardKey.W))
            {
                orbY++;
            }
           // test.instance.counts4++;
            if (kh.GetKeyDown(KeyboardKey.S))
            {
                orbY--;
            }
           // test.instance.counts5++;
            if (kh.GetKeyDown(KeyboardKey.A))
            {
                orbX--;
            }
          //  test.instance.counts6++;
            if (kh.GetKeyDown(KeyboardKey.D))
            {
                orbX++;
            }
          //  test.instance.counts7++;
            if (orbX > buffer.width - 1)
            {
                orbX = 0;
            }

            if (orbX < 0)
            {
                orbX = buffer.width - 1;
            }
            if (orbY > buffer.height - 1)
            {
                orbY = 0;
            }
            if (orbY < 0)
            {
                orbY = buffer.height - 1;
            }
         //   test.instance.counts8++;
            Console.Debug("frame"+kh);
          //  test.instance.counts9++;
            Runtime.Wait(1);
           // test.instance.counts10++;
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


