using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

public class StackExecutor : MonoBehaviour
{
    [NonSerialized] public HardwareInternal hardwareInternal;
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
    [SerializeField] private ConcurrentQueue<ITryToRun> actionQueue = new ConcurrentQueue<ITryToRun>();

    public void UpdateMaxTask()
    {
        _maxTasksBuffer = hardwareInternal.TasksPerCPULoop == -1
            ? 100
            : hardwareInternal.TasksPerCPULoop;
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
        return AddDelegateToStack(new MainThreadDelegate<T>(action, hardwareInternal.WaitRefreshRate), sync);
    }

    internal T AddDelegateToStack<T>(MainThreadDelegate<T> mtd, bool sync = true)
    {
        actionQueue.Enqueue(mtd);

        if (sync)
        {
            return mtd.WaitForReturn();
        }

        return default(T);
    }

    internal void AddDelegateToStack(Action<Hardware> action, bool sync = true)
    {
        AddDelegateToStack((ref bool done, ref object returnValue) => { action.Invoke(hardwareInternal.hardware); }, sync);
    }

    /* [Button]
     internal void CheckThreads()
     {
         hardwareInternal.CheckThreads();
     }*/

    [Button]
    internal void KillAll()
    {
        hardwareInternal.KillAll();
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