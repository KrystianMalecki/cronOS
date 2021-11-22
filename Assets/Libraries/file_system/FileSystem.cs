using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo 8 rethink if Makers should be sync or not
namespace Libraries.system.file_system
{
    public class FileSystem : BaseLibrary
    {
        public static File GetFileByPath(string path, File parent = null)
        {
            return FileSystemInternal.instance.GetFileByPath(path, parent);
        }
        public static File MakeFile(string path, string name, byte[] data = null)
        {
            File file = FileSystemInternal.instance.MakeFile(name, data);
            File parent = FileSystemInternal.instance.GetFileByPath(path);
            parent.AddChild(file);
            return file;


        }
        public static File MakeFile(string rawPath)
        {
            string[] path = rawPath.Split(FileSystemInternal.catalogSymbol);
            throw new System.Exception("Not implemented");//todo 0 fix

            File currentFile = null;// FileSystemInternal.instance.drive.root;
            for (int i = 0; i < path.Length; i++)
            {
                File newFile = GetFileByPath("./" + path[i], currentFile);
                if (newFile == null)
                {
                    newFile = MakeFolder(currentFile.GetFullPath(), path[i]);
                }
                currentFile = newFile;
            }

            return currentFile;
        }
        public static File MakeFolder(string path, string name)
        {
            File file = FileSystemInternal.instance.MakeFolder(name);
            File parent = FileSystemInternal.instance.GetFileByPath(path);
            parent.AddChild(file);
            return file;
        }
        public static bool RemoveFile(string path)
        {
            return FileSystemInternal.instance.RemoveFile(path);

        }

    }
}