using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class StackExecutor : MonoBehaviour
{
    [NonSerialized]
    public Hardware hardware;
    public object locker = new object();
    public void Start()
    {
        UpdateMaxTask();
        StartCoroutine(ExecuteFromQueue());
    }
    /*  public void Update()
      {
          ExecuteFromQueue();
      }*/
    private ITryToRun _delegateBuffer = null;
    private int _maxTasksBuffer;
    [SerializeField]
    private ConcurrentQueue<ITryToRun> actionQueue = new ConcurrentQueue<ITryToRun>();
    public void UpdateMaxTask()
    {
        _maxTasksBuffer = hardware.hardwareInternal.TasksPerCPULoop == -1 ? 100 : hardware.hardwareInternal.TasksPerCPULoop;
       // _maxTasksBuffer = 1;
    }
    private IEnumerator ExecuteFromQueue()
    {
        while (true)
        {
            for (int i = 0; (i < _maxTasksBuffer); i++)
            {
                if (actionQueue.Count > 0)
                {
                    /*try
                    {*/
                    if (true)
                    {
                        if (actionQueue.TryDequeue(out _delegateBuffer))
                        {

                            if (_delegateBuffer != null && !_delegateBuffer.TryToRun())
                            {
                                actionQueue.Enqueue(_delegateBuffer);
                            }
                            else
                            {
                                //  Debug.Log("killing null:");
                                //_delegateBuffer.Speak();
                            }
                        }
                        //_delegateBuffer = null;
                    }

                    /* }
                     catch (Exception e)
                     {
                         Debug.Log(e);

                     }*/
                    yield return null;
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }
    }
    [Button("Display stack")]
    void DisplayStack()
    {
        try
        {
            foreach (ITryToRun runTry in actionQueue)
            {
                //   Debug.Log(runTry.function.ToString());
                runTry.Speak();
            }
            if (actionQueue.Count == 0)
            {
                Debug.Log("actionStack is empty");
            }
        }
        catch (Exception)
        {

        }
    }

    internal T AddDelegateToStack<T>(MainThreadDelegate<T>.MTDFunction action, bool sync = true)
    {

        return AddDelegateToStack(new MainThreadDelegate<T>(action, hardware.hardwareInternal.WaitRefreshRate), sync);

    }
    internal T AddDelegateToStack<T>(MainThreadDelegate<T> mtf, bool sync = true)
    {
        actionQueue.Enqueue(mtf);

        if (sync)
        {
            return mtf.WaitForReturn();
        }
        return default(T);
    }
    internal void AddDelegateToStack(Action action, bool sync = true)
    {
        AddDelegateToStack((ref bool done, ref object returnValue) =>
        {
            action.Invoke();
        }, sync);
    }
    [Button]
    internal void CheackThreads()
    {
        hardware.hardwareInternal.CheackThreads();
    }

    [Button]
    internal void KillAll()
    {
        hardware.hardwareInternal.KillAll();
    }
    public void OnDestroy()
    {
        KillAll();
    }
    public void OnApplicationQuit()
    {
        KillAll();
    }
}
