using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public static readonly Encoding mainEncoding = Encoding.GetEncoding("437");
    public static bool checkIfCanRun()
    {
        return instance.canRun;
    }
}
