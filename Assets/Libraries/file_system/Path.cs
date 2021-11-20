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
            public string[] parts;
            public List<File> fileparts = null;

            public Path(string rawPath, File parent = null)
            {
                if (rawPath == null)
                {
                    rawPath = "";
                }
                //ls ./home
                //ls -wd 
                rawPath = rawPath.StartsWith("./") ? (parent.GetFullPath() + rawPath.Substring(1)) : rawPath;
                parts = rawPath.Split(FileSystemInternal.catalogSymbol);
                File currentFile = FileSystemInternal.instance.drive.root;

                try
                {
                    while (rawPath.Contains("../"))
                    {

                        int upStartIndex = rawPath.IndexOf("../");

                        int ealierCatalogIndex = rawPath.LastIndexOf("/", upStartIndex - 2);
                        string parentPart = rawPath.Substring(ealierCatalogIndex, (upStartIndex + 2) - ealierCatalogIndex);
                        int ealierParentCatalogIndex = rawPath.LastIndexOf("/", ealierCatalogIndex);

                        string parentParentPart = rawPath.Substring(ealierParentCatalogIndex, ealierCatalogIndex - ealierParentCatalogIndex);
                        string pre = rawPath.Substring(0, ealierParentCatalogIndex);
                        string after = rawPath.Substring((upStartIndex + 2));
                        rawPath = pre + after;

                    }
                }
                catch (Exception e)
                {

                }
                fileparts = new List<File>();
                fileparts.Add(currentFile);//root
                for (int i = 1; i < parts.Length; i++)
                {
                    string name = parts[i];
                    if (string.IsNullOrEmpty(name))
                    {
                        //todo-future throw error
                        break;
                    }
                    if (name == "..")
                    {
                        currentFile = currentFile.parent;
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
                //  return parts == null ? "" : string.Join(/*FileSystemInternal.catalogSymbol.ToString()*/"-", parts);
                string path = "";
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