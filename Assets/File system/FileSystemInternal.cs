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
    public static readonly string folderExtension = "DIR";
    public static readonly char catalogSymbol = '/';
    public Drive GetDrive(string name)
    {
        Debug.Log(name);
        return drives.Find(x => x.driveFile.name + ":" == name);

    }
    public File GetFileByPath(string rawPath)
    {
        string[] parts = rawPath.Split(catalogSymbol);
        Drive d = GetDrive(parts[0]);
        //todo 8 check d
        File currentFile = d.driveFile;
        for (int i = 1; i < parts.Length; i++)
        {
            if (string.IsNullOrEmpty(parts[i]))
            {
                return currentFile;
            }
            currentFile = currentFile.GetChildByName(parts[i]);
            if (currentFile == null)
            {
                return null;
            }
        }
        return currentFile;

    }
 /*   public Path GetPath(string rawPath)
    {
        return new Path(rawPath);
    }*/
    public bool RemoveFile(string path)
    {
        //todo-future add errors
        File file = GetFileByPath(path);
        file.parent.RemoveFile(file);
        return true;
    }
    public File MakeFolder(string name)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = new FilePermissions(0b1111);
        newFile.files = new ThreadSafeList<File>();
        return newFile;
    }
    public File MakeFile(string name, byte[] data = null)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = new FilePermissions(0b0111);

        newFile.files = null;
        if (data != null)
        {
            newFile.data = data;
        }
        return newFile;
    }
}
