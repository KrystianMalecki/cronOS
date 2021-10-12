using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorManager : MonoBehaviour
{
    #region singleton logic
    public static ProcessorManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    public int WaitRefreshRate = 100;
    public int TasksPerCPULoop = -1;

    public bool canRun;
    public bool ignoreSomeErrors;

    public static bool checkIfCanRun()
    {
        return instance.canRun;

    }
}
