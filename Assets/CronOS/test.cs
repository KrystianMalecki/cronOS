using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class test : MonoBehaviour
{
    public CancellationTokenSource source = new CancellationTokenSource();
    public CancellationToken token;
    Thread thread;
    // Start is called before the first frame update
    public void Start()
    {
        // flag: tests
        //  TaskVersion();
        // ThreadVersion();
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
                  Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
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
    }
}
