using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class StackExecutor : MonoBehaviour
{
    public Hardware system;
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
    [Button]

    internal void CheackThreads()
    {
        system.CheackThreads();
    }

    [Button]
    internal void KillAll()
    {
        system.KillAll();
    }
    public void OnDestroy()
    {
        system.KillAll();
    }
    public void OnApplicationQuit()
    {
        system.KillAll();
    }
}
