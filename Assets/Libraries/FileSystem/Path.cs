using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//todo 8 rethink if Makers should be sync or not
namespace Libraries.system.filesystem
{
    public class Path
    {
        public List<string> parts = new List<string>();
        public Drive drive;
        public List<File> fileparts = new List<File>();

        public Path(string rawPath)
        {
            parts = rawPath.Split(FileSystemInternal.catalogSymbol).ToList();
            drive = FileSystemInternal.instance.GetDrive(parts[0]);
            //todo 8 check d
            File currentFile = drive.driveFile;
            fileparts.Add(currentFile);
            for (int i = 1; i < parts.Count; i++)
            {
                if (string.IsNullOrEmpty(parts[i]))
                {
                    break;
                }
                currentFile = currentFile.GetChildByName(parts[i]);
                if (currentFile == null)
                {
                    //todo 9 throw error
                    break;
                }
                else
                {
                    fileparts.Add(currentFile);
                }
            }
        }
    }
}