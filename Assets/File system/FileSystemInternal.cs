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
        ValidateRoot();

    }
    #endregion
   // public ThreadSafeList<Drive> drives = new ThreadSafeList<Drive>();
    public File root;
    // public static readonly string folderExtension = "DIR";//todo 0  remove
    public static readonly char catalogSymbol = '/';
    /* public Drive GetDrive(string name)
     {
         // Debug.Log($"equals?{drives[0].driveFile.name} == C:? {drives[0].driveFile.name == "C:"}");
         return drives.Find(x => x.driveFile.name == name);
     }*/
    [Button]
    private void ValidateRoot()
    {
        root.Validate(true);
    }
    //todo 0 rethink
    public File GetFileByPath(File father, string rawPath)
    {
        string[] parts = rawPath.Split(catalogSymbol);
        //todo 0 rethink that
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
        return GetFileByPath(root, rawPath);

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
        newFile.permissions = (FilePermission)0b1111;
        newFile.files = new ThreadSafeList<File>();
        return newFile;
    }
    public File MakeFile(string name, byte[] data = null)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = (FilePermission)0b0111;

        newFile.files = null;
        if (data != null)
        {
            newFile.data = data;
        }
        return newFile;
    }
}
