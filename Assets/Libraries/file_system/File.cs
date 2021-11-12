using NaughtyAttributes;
using System;
using System.Collections;

using System.Collections.Concurrent;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
namespace Libraries.system
{
    namespace file_system
    {
        [System.Serializable]
        public unsafe class File
        {
            public string name;
            [SerializeField]
            [EnumMask]
            public FilePermission permissions = (FilePermission)0b0111;

            [AllowNesting]
            [HideInInspector]
            public byte[] data;

            [SerializeField]
            public ThreadSafeList<File> children;

            [NonSerialized]
            public File parent;

            public void AddChild(File file)
            {
                children.Add(file);
                file.parent = this;
            }
            public void RemoveFile(File file)
            {
                children.Remove(file);
                file.parent = null;
            }

            public string GetFullPath()
            {
                if (parent == null)
                {
                    return name;
                }
                return string.Concat(parent.GetFullPath(), "/", name);
            }
            public Path GetPathClass()
            {
                string rawPath = "";
                if (parent == null)
                {
                    rawPath = name;
                }
                else
                {
                    rawPath = string.Concat(parent.GetFullPath(), "/", name);
                }
                return new Path(rawPath);
            }
            public void MoveFileTo(File desitination)
            {
                parent.RemoveFile(this);
                desitination.AddChild(this);
            }
            public string ReturnDataAsString()
            {
                return ProcessorManager.mainEncoding.GetString(data);
            }
            public File GetChildByName(string name)
            {
                lock (children)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i].name == name)
                        {
                            return children[i];
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
                return (permissions & FilePermission.isFolder) == FilePermission.isFolder;
            }

            public void OnValidate()
            {
                Validate(false);
            }
            public void Validate(bool recursive)
            {
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        child.parent = this;
                        // Debug.Log("Validating " + child);
                        if (recursive)
                        {
                            child.Validate(recursive);
                        }
                    }
                }
            }
            public int GetDataArraySize()
            {
                if (data == null)
                {
                    return 0;
                }
                return data.Length;
            }
            public string GetByteSize()
            {
                int sizeScale = 0;
                int currentSize = GetDataArraySize() + name.Length * 8 + 8 /*+ children.Count * 8 * 2*/;
                while (currentSize > 1024)
                {
                    currentSize = currentSize / 1024;
                    sizeScale++;
                }
                return $"{currentSize}{SIZES[sizeScale]}B";
            }
            private readonly string[] SIZES = { "k", "M", "G", "T" };
        }

    }
}
[Serializable]
[Flags]
public enum FilePermission
{
    //hasExtension =  0b01000000
    //hidden =        0b00100000,
    //isLink =        0b00010000,


    isFolder = 0b00001000,
    read = 0b00000100,
    write = 0b00000010,
    execute = 0b00000001


}