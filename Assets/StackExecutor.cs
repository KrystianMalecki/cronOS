using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class StackExecutor : MonoBehaviour
{
    public Cronos.System system;
    public void Update()
    {
        ExecuteFromQueue();
    }
    private ITryToRun _delegateBuffer = null;
    private int _maxTasksBuffer;
    [SerializeField]
    private ConcurrentQueue<ITryToRun> actionQueue = new ConcurrentQueue<ITryToRun>();
    private void ExecuteFromQueue()
    {
        _maxTasksBuffer = system.TasksPerCPULoop == -1 ? actionQueue.Count : system.TasksPerCPULoop;
        for (int i = 0; (i < _maxTasksBuffer); i++)
        {
            if (actionQueue.Count > 0)
            {
                try
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
                    _delegateBuffer = null;
                }
                catch (Exception e)
                {
                    Debug.Log($"{e.StackTrace}");

                }
            }
            else
            {
                break;
            }
        }
    }
}
