using helper;
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
            [SerializeField]

            public string name;
            [SerializeField]
            // [EnumMask]
            public FilePermission permissions = (FilePermission)0b0111;

            [AllowNesting]
            [HideInInspector]
            [SerializeField]

            public byte[] data;

            [NonSerialized]
            //[NonSerialized, OdinSerialize]
            public ThreadSafeList<File> children;

            [NonSerialized]
            public DriveSO drive;

            public int parentID;
            [NonSerialized]
            public File parent;

            public void AddChild(File file)
            {
                if (children == null)
                {
                    children = new ThreadSafeList<File>();
                }
                children.Add(file);
                file.parent = this;
            }
            public void RemoveChild(File file)
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
                parent.RemoveChild(this);
                desitination.AddChild(this);
            }
            public string ReturnDataAsString()
            {
                return ProcessorManager.mainEncoding.GetString(data);
            }
            public File GetChildByName(string name)
            {
                if (children != null)
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
                GenerateParentLinks(false);
            }
            public void GenerateParentLinks(bool recursive)
            {
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        Debug.Log($"{name}-{this}-{child}");
                        child.parent = this;
                        // Debug.Log("Validating " + child);
                        if (recursive)
                        {
                            child.GenerateParentLinks(recursive);
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

            public string GetByteSize(bool prefixed = true)
            {
                int size = GetDataArraySize() + name.Length * 8 + 8;
                return $"{(prefixed ? size.ChangeToPrefixedValue() : size.ToString())}B";
            }

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