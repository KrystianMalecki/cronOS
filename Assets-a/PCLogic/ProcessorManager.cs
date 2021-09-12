using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorManager : MonoBehaviour
{
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
    public bool canRun;

    public static bool checkIfCanRun()
    {
        return instance.canRun;

    }
}
