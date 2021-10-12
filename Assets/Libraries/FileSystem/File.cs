using NaughtyAttributes;
using System;
using System.Collections;

using System.Collections.Concurrent;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
//todo 5 add checks is files are folders
//todo 7 add checks for names and data
namespace Libraries.system.filesystem
{

    [System.Serializable]
    public class File
    {
        public string name;


        [AllowNesting]
        [HideInInspector]
        // [NaughtyAttributes.OnValueChanged("_setText")]
        public byte[] data;

        public string extension;
        [SerializeField]

        public ThreadSafeList<File> files;
        [System.NonSerialized]
        public File parent;


        [AllowNesting]
        [System.NonSerialized]
        public bool changing = false;




        public void AddFile(File file)
        {
            files.Add(file);
            file.parent = this;
        }
        public void RemoveFile(File file)
        {
            files.Remove(file);
            file.parent = null;
        }
        public string GetFullName()
        {
            return name + ((!string.IsNullOrEmpty(extension)) && (extension != "DIR") ? ("." + extension) : "");
        }
        public string GetFullPath()
        {
            return string.Concat(parent.GetFullPath(), "/", GetFullName());
        }
        public void MoveFileTo(File desitination)
        {
            parent.RemoveFile(this);
            desitination.AddFile(this);
        }
        //todo 9 fix 0
        public string ReturnDataAsString()
        {
            return new string(Array.ConvertAll(data, x => (char)x));
        }
        public File GetFileByName(string name)
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
        public File GetFileByFullName(string fullName)
        {
            lock (files)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].GetFullName() == fullName)
                    {
                        return files[i];
                    }
                }
            }
            return null;
        }

        /*   public System.IO.MemoryStream Open()
           {

               return new System.IO.MemoryStream(data);
           }*/
        public override string ToString()
        {
            return GetFullName();
        }
    }
}