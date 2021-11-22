using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using Libraries.system.file_system;
using System.Linq;
using NaughtyAttributes;

public class FileSystemInternal : MonoBehaviour
{
    #region singleton logic
    public static FileSystemInternal instance;
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
        drive.GenerateCacheData();

    }
    #endregion
    public DriveSO drive;
    public static readonly char catalogSymbol = '/';



}
