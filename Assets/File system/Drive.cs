using Libraries.system.file_system;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Concurrent;


[Serializable]
public class Drive
{
    [NonSerialized] public File root = null;
    [NonSerialized] public bool cached = false;

    [SerializeField] public ThreadSafeList<File> files = new ThreadSafeList<File>();
    [SerializeField] public ThreadSafeList<int> freeSpaces = new ThreadSafeList<int>();


    public File GetRoot()
    {
        //  root ??= GetFileByID(1);
        //  root ??= GetFileByPath("");
        return root;
    }


    public void GenerateCacheData()
    {
        cached = true;
        Debug.Log("cashing");
        for (int i = 0; i < files.Count; i++)
        {
            if (files[i].children != null)
            {
                files[i].children.Clear();
            }
        }

        for (int i = 0; i < files.Count; i++)
        {
            File file = files[i];
            file.FileID = i;
            file.SetDrive(this);
            if (file.FileID == file.ParentID)
            {
                Debug.Log("FUCK UNITY");
            }

            File parent = GetFileByID(file.ParentID);
            file.Parent = parent;
            parent?.AddChild(file);
        }

        root ??= GetFileByID(1);
        root ??= GetFileByPath("");
    }

    public File GetFileByID(int id)
    {
        try
        {
            if (id == 0)
            {
                return null;
            }

            if (id < 0)
            {
                return null;
            }
            else if (id >= files.Count)
            {
                return null;
            }

            return files[id];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public File GetFileByPath(string rawPath, File parent = null)
    {
        return GetPath(rawPath, parent).GetFile();
    }

    public Path GetPath(string rawPath, File parent = null)
    {
        return new Path(rawPath, GetRoot(), parent);
    }

    public bool RemoveFile(string path)
    {
        //todo-future add errors
        File file = GetFileByPath(path);

        return RemoveFile(file);
    }

    public bool RemoveFile(File file)
    {
        //todo-future add errors
        file.Parent.RemoveChild(file);
        return true;
    }

    /**
     <summary> 
    This doesn't add <see cref="File"/> of type folder to <see cref="DriveSO"/>
    </summary>
       <returns>New sample folder</returns>
     **/
    public static File MakeFolder(string name)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = (FilePermission)0b1111;

        return newFile;
    }

    /**
     <summary> 
    This doesn't add <see cref="File"/> to <see cref="DriveSO"/>
    </summary>
       <returns>New sample file</returns>
     **/
    public static File MakeFile(string name, byte[] data = null)
    {
        File newFile = new File();
        newFile.name = name;
        newFile.permissions = (FilePermission)0b0111;

        newFile.data = data;

        return newFile;
    }

    public int GetFreeID()
    {
        if (freeSpaces.Count > 0)
        {
            int result = freeSpaces[0];

            Debug.Log(freeSpaces.ToFormattedString());
            freeSpaces.RemoveAt(0);
            return result;
        }

        return files.Count;
    }

    private int SetAt(int pos, File file)
    {
        if (files.Count <= pos)
        {
            pos = files.Count;
            file.FileID = (pos);
            files.Add(file);
        }
        else
        {
            files[pos] = file;
        }

        return pos;
    }

    public void AddFileToDrive(File file)
    {
        file.SetDrive(this);
        file.FileID = GetFreeID();
        SetAt(file.FileID, file);
    }

    public void RemoveFileFromDrive(File file)
    {
        //  files.RemoveAt(file.FileID); //why? cuz now you can still retrive it
        freeSpaces.Add(file.FileID);
        if (file.children != null)
        {
            for (int i = 0; i < file.children.Count; i++)
            {
                RemoveFileFromDrive(file.children[i]);
            }
        }

        Debug.Log(freeSpaces.ToFormattedString());
    }
}