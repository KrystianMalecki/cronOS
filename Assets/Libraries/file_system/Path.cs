using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Libraries.system
{
    namespace file_system
    {
        public class Path
        {
            public List<string> parts = new List<string>();
            public List<File> fileparts = new List<File>();

            public Path(string rawPath)
            {
                parts = rawPath.Split(FileSystemInternal.catalogSymbol).ToList();
                File currentFile = FileSystemInternal.instance.root;
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