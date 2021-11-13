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


    public File GetFileByPath(File father, string rawPath)
    {
        string[] parts = rawPath.Split(catalogSymbol);
        if (parts[0] != father.name)
        {
            return null;
        }
        File currentFile = father;
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
    public File GetFileByPath(string rawPath)
    {
        return GetFileByPath(drive.root, rawPath);

    }
    public Path GetPath(string rawPath)
    {
        return new Path(rawPath);
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
        if (data != null)
        {
            newFile.data = data;
        }
        return newFile;
    }
    //todo 1 add: (make it better)
    // . this maybe done?
    // special symbol that marks system path from system config
    // .. up only first one works
    // maybe later some regex?
    public string MakeAbsolutePath(string path, File currentFile = null)
    {
        string bufferPath = path;
        //  Debug.Log($"path:{bufferPath} file:{currentFile}");
        if (currentFile != null)
        {
            if (bufferPath.StartsWith(".."))
            {
                if (currentFile.parent != null)
                {

                    bufferPath = currentFile.parent.GetFullPath() + "/" + bufferPath.Substring(2);
                }
            }
            if (bufferPath.StartsWith("."))
            {
                bufferPath = currentFile.GetFullPath() + "/" + bufferPath.Substring(1);
            }
        }
        return bufferPath;
    }
}
