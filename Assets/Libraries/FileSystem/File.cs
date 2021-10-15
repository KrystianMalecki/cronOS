using NaughtyAttributes;
using System;
using System.Collections;

using System.Collections.Concurrent;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
namespace Libraries.system.filesystem
{

    [System.Serializable]
    public class File
    {
        public string name;
        [SerializeField]
        public FilePermissions permissions = new FilePermissions(0b0111);

        [AllowNesting]
        [HideInInspector]
        public byte[] data;

        [SerializeField]
        public ThreadSafeList<File> files;

        [System.NonSerialized]
        public File parent;

        public void AddChild(File file)
        {
            files.Add(file);
            file.parent = this;
        }
        public void RemoveFile(File file)
        {
            files.Remove(file);
            file.parent = null;
        }

        public string GetFullPath()
        {
            return string.Concat(parent.GetFullPath(), "/", name);
        }
        public void MoveFileTo(File desitination)
        {
            parent.RemoveFile(this);
            desitination.AddChild(this);
        }
        //todo 9 find better method
        public string ReturnDataAsString()
        {

            return ProcessorManager.mainEncoding.GetString(data);
        }
        public File GetChildByName(string name)
        {
            lock (files)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].name == name)
                    {
                        return files[i];
                    }
                }
            }
            return null;
        }

        public override string ToString()
        {
            return GetFullPath();
        }


        public bool isFolder()
        {
            return permissions.isFolder;
        }
    }

}
[Serializable]
public struct FilePermissions
{
    public bool isFolder;
    public bool read;
    public bool wrtite;
    public bool execute;

    public FilePermissions(byte halfByte)
    {
        execute = halfByte % 2 == 1;
        wrtite = halfByte % 4 == 1;
        read = halfByte % 8 == 1;
        isFolder = halfByte % 16 == 1;

    }
}