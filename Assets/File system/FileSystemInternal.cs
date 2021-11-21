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
        drive.GenerateParentLinks();

    }
    #endregion
    public DriveSO drive;
    public static readonly char catalogSymbol = '/';


    public File GetFileByPath(string rawPath, File parent = null)
    {
        return GetPath(rawPath, parent).GetFile();
    }

    public Path GetPath(string rawPath, File parent = null)
    {
        return new Path(rawPath, parent);
    }

    public bool RemoveFile(string path)
    {
        //todo-future add errors
        File file = GetFileByPath(path);
        file.parent.RemoveChild(file);
        return true;
    }
    public File MakeFolder(string name)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = (FilePermission)0b1111;
        newFile.children = new ThreadSafeList<File>();
        return newFile;
    }
    public File MakeFile(string name, byte[] data = null)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = (FilePermission)0b0111;

        newFile.children = null;
        newFile.data = data;

        return newFile;
    }
}
