
using Helpers;
using NaughtyAttributes;

using System;
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


            [SerializeField][EnumFlags] public FilePermission permissions = (FilePermission)0b0111;


            [HideInInspector][SerializeField] public byte[] data;

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

            //todo change name to setChild
            public File SetChild(File file)
            {
                children ??= new ThreadSafeList<File>();

                int index = children.FindIndex(x => x.name == file.name);
                if (index != -1)
                {
                    children[index] = file;
                }
                else
                {
                    children.Add(file);
                }

                if (file.fileID is -1 or 0)
                {
                    drive.AddFileToDrive(file);
                }

                file.Parent = (this);
                return file;
            }

            public bool HasChild(string name)
            {
                return children.FindIndex(x => x.name == name) != -1;

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

            public void MoveFileTo(File destination)
            {
                Parent.RemoveChild(this);
                destination.SetChild(this);
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

            public override bool Equals(object obj)
            {
                if (obj is File file)
                {
                    if (file.fileID == fileID)
                    {
                        return true;
                    }
                    else
                    {
                        return file.GetFullPath() == this.GetFullPath() && file.drive == this.drive;
                    }
                }
                return base.Equals(obj);
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