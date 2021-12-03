using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Libraries.system.file_system
{
    public class FileSystem : BaseLibrary
    {
        public static File GetFileByPath(string path, File parent = null)
        {
            return FileSystemInternal.instance.mainDrive.GetFileByPath(path, parent);
        }
        public static File MakeFile(string path, string name, FilePermission filePermission, byte[] data = null)
        {
            File file = Drive.MakeFile(name, data);
            file.permissions = filePermission;
            File parent = FileSystemInternal.instance.mainDrive.GetFileByPath(path);
            parent.AddChild(file);
            return file;


        }
        public static File MakeFile(string rawPath)
        {
            string[] path = rawPath.Split(FileSystemInternal.catalogSymbol);

            File currentFile = FileSystemInternal.instance.mainDrive.GetRoot();
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
            File file = Drive.MakeFolder(name);
            File parent = FileSystemInternal.instance.mainDrive.GetFileByPath(path);
            parent.AddChild(file);
            return file;
        }
        public static bool RemoveFile(string path)
        {
            return FileSystemInternal.instance.mainDrive.RemoveFile(path);

        }

    }
}