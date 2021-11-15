using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Libraries.system
{
    namespace file_system
    {
        //todo 0 think if it should be removed
        public class Path
        {
            public List<string> parts = new List<string>();
            public List<File> fileparts = new List<File>();

            public Path(string rawPath, File root = null)
            {
                parts = rawPath.Split(FileSystemInternal.catalogSymbol).ToList();
                File currentFile = root == null ? FileSystemInternal.instance.drive.root : root;
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
                        //todo-future throw error
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
}