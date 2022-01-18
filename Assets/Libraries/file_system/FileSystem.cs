using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libraries.system.file_system
{
    public class FileSystem : BaseLibrary
    {
        public const char catalogSymbol = '/';

        public File GetFileByPath(string path, File parent = null)
        {
            return hardware.hardwareInternal.mainDrive.drive.GetFileByPath(path, parent);
        }

        public File MakeFile(string path, string name, FilePermission filePermission, byte[] data = null)
        {
            File file = Drive.MakeFile(name, data);
            file.permissions = filePermission;
            File parent = hardware.hardwareInternal.mainDrive.drive.GetFileByPath(path);
            parent.AddChild(file);
            return file;
        }

        public File MakeFile(string rawPath)
        {
            string[] path = rawPath.Split(catalogSymbol);

            File currentFile = hardware.hardwareInternal.mainDrive.drive.GetRoot();
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

        public File MakeFolder(string path, string name)
        {
            File file = Drive.MakeFolder(name);
            File parent = hardware.hardwareInternal.mainDrive.drive.GetFileByPath(path);
            parent.AddChild(file);
            return file;
        }

        public bool RemoveFile(string path)
        {
            return hardware.hardwareInternal.mainDrive.drive.RemoveFile(path);
        }
    }
}