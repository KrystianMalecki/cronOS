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

            public Path(string rawPath, File parent = null, File root = null)
            {
                rawPath ??= "";
                parent ??= root == null ? FileSystemInternal.instance.mainDrive.GetRoot() : root;

                rawPath = rawPath.StartsWith("./") ? (parent.GetFullPath() + rawPath.Substring(1)) : rawPath;
                parts = rawPath.Split(FileSystemInternal.catalogSymbol).ToList();

                File currentFile = root == null ? FileSystemInternal.instance.mainDrive.GetRoot() : root;
                fileparts.Add(currentFile);
                for (int i = 1; i < parts.Count; i++)
                {
                    if (currentFile == null)
                    {
                        //todo-future throw error

                        parts = null;
                        fileparts = null;
                        break;
                    }
                    string name = parts[i];
                    if (string.IsNullOrEmpty(name))
                    {
                        //todo-future throw error
                        break;
                    }
                    if (name == "..")
                    {
                        fileparts.RemoveAt(fileparts.Count - 1);
                        fileparts.RemoveAt(fileparts.Count - 1);

                        currentFile = currentFile?.Parent;
                        //  continue;
                    }
                    else
                    {
                        currentFile = currentFile.GetChildByName(name);
                    }
                    if (currentFile == null)
                    {
                        //todo-future throw error

                        parts = null;
                        fileparts = null;
                        break;
                    }
                    fileparts.Add(currentFile);
                }
            }
            public override string ToString()
            {
                string path = parts == null ? "" : string.Join(/*FileSystemInternal.catalogSymbol.ToString()*/"-", parts) + "\n";
                fileparts?.ForEach(x => path += x.name + "-");
                return path;

            }
            public File GetFile()
            {

                return fileparts?[fileparts.Count - 1];
            }
        }
    }
}