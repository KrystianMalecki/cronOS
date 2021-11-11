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
    [NaughtyAttributes.OnValueChanged("UpdateWRR")]
    public float FPSCap = 100;
    [NaughtyAttributes.OnValueChanged("UpdateFPSC")]

    public int WaitRefreshRate = 100;
    public int TasksPerCPULoop = -1;

    public bool canRun;
    public bool ignoreSomeErrors;
    public static readonly Encoding mainEncoding = Encoding.GetEncoding("437");
    public static bool checkIfCanRun()
    {
        return instance.canRun;
    }
    void UpdateWRR()
    {
        WaitRefreshRate = (int)((1f / FPSCap) * 1000f);
    }
    void UpdateFPSC()
    {
        FPSCap = ((1f / WaitRefreshRate) * 1000f);
    }
}
