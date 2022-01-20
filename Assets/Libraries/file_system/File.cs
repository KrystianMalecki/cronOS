using helper;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            public File()
            {
                fileID = -1;
                parentID = -1;
                name = "";
            }

            [SerializeField] protected int fileID = -1;
            [SerializeField] protected int parentID = -1;


            [SerializeField] public string name;


            [SerializeField] [EnumFlags] public FilePermission permissions = (FilePermission)0b0111;


            [HideInInspector] [SerializeField] public byte[] data;

            [NonSerialized] public ThreadSafeList<File> children;

            [NonSerialized] private Drive drive;

            [NonSerialized] private File _parent;

            //  [NonSerialized]
            public File Parent
            {
                get { return _parent; }
                set
                {
                    _parent = value;
                    parentID = (_parent == null ? -1 : _parent.fileID);
                }
            }

            internal int FileID
            {
                get { return fileID; }
                set { fileID = value; }
            }

            internal int ParentID
            {
                get { return parentID; }
                set { parentID = value; }
            }


            public Drive GetDrive()
            {
                return drive;
            }

            internal void SetDrive(Drive drive)
            {
                this.drive = drive;
            }

            public File AddChild(File file)
            {
                if (children == null)
                {
                    children = new ThreadSafeList<File>();
                }

                children.Add(file);
                if (file.fileID == -1 || file.fileID == 0)
                {
                    drive.AddFileToDrive(file);
                }

                file.Parent = (this);
                return file;
            }

            public void RemoveChild(File file)
            {
                children?.Remove(file);
                file.Deparent();
                drive.RemoveFileFromDrive(file);
            }

            public void Deparent()
            {
                Parent = (null);
                _parent = null;
            }

            public string GetFullPath()
            {
                if (Parent == null)
                {
                    return name;
                }

                return string.Concat(Parent.GetFullPath(), "/", name);
            }

            public Path GetPathClass(File root = null)
            {
                return new Path(GetFullPath(), root == null ? drive?.GetRoot() : root);
            }

            public void MoveFileTo(File desitination)
            {
                Parent.RemoveChild(this);
                desitination.AddChild(this);
            }

            public string ReturnDataAsString()
            {
                return HardwareInternal.mainEncoding.GetString(data);
            }

            public File GetChildByName(string name)
            {
                return children?.Find(x => x.name == name);
            }

            public override string ToString()
            {
                return GetFullPath();
            }


            public bool isFolder()
            {
                return (permissions & FilePermission.isFolder) == FilePermission.isFolder;
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