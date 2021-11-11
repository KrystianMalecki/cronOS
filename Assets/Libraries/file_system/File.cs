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
        public class File
        {
            public string name;
            [SerializeField]
            [EnumMask]
            public FilePermission permissions = (FilePermission)0b0111;

            [AllowNesting]
            [HideInInspector]
            public byte[] data;

            [SerializeField]
            public ThreadSafeList<File> files;

            [SerializeReference]
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
                return (permissions & FilePermission.isFolder) == FilePermission.isFolder;
            }

            public void OnValidate()
            {
                Validate(false);
            }
            public void Validate(bool recursive)
            {
                foreach (var child in files)
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

    }
}
[Serializable]
[Flags]
public enum FilePermission
{
    isFolder = 0b1000,
    read = 0b0100,
    write = 0b0010,
    execute = 0b0001


}