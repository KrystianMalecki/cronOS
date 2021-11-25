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
        mainDrive.GenerateCacheData();

    }
    #endregion
    public Drive mainDrive;
    public static readonly char catalogSymbol = '/';
    public ThreadSafeList<Drive> drives = new ThreadSafeList<Drive>();

    public void CacheAllDrives()
    {
        for (int i = 0; i < drives.Count; i++)
        {
            if (!drives[i].cached)
            {
                drives[i].GenerateCacheData();
            }
        }
    }
}
