using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using Libraries.system.filesystem;
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
    }
    #endregion
    public ThreadSafeList<Drive> drives = new ThreadSafeList<Drive>();

    public File GetFileByPath(string path)
    {
        string[] parts = path.Split('/');
        Drive d = drives.Find(x => x.driveFile.name + ":" == parts[0]);
        //todo 8 check d
        File currentFile = d.driveFile;
        for (int i = 1; i < parts.Length; i++)
        {
            if (string.IsNullOrEmpty(parts[i]))
            {
                return currentFile;
            }
            currentFile = currentFile.GetFileByFullName(parts[i]);
            if (currentFile == null)
            {
                return null;
            }
        }
        return currentFile;

    }
    public string path;
    [SerializeReference]
    public File file;
    [Button]
    public void getfile()
    {
        file = GetFileByPath(path);
        Debug.Log(file);
    }
}
