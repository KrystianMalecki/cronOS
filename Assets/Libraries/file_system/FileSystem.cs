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
            /*  return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
              {
                  returnValue = FileSystemInternal.instance.GetFileByPath(path, parent);
              }, sync);//should be always sync\*/
            return FileSystemInternal.instance.GetFileByPath(path, parent);
        }
        public static File MakeFile(string path, string name, byte[] data = null)
        {
            /*  return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
              {
                  returnValue = FileSystemInternal.instance.MakeFile(name, data);
                  File parent = FileSystemInternal.instance.GetFileByPath(path);
                  parent.AddChild(returnValue);
              }, sync);//should be always sync*/
            File file = FileSystemInternal.instance.MakeFile(name, data);
            File parent = FileSystemInternal.instance.GetFileByPath(path);
            parent.AddChild(file);
            return file;


        }
        public static File MakeFile(string rawPath)
        {
            /* return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
             {
                 Path path = new Path(rawPath);
                 File parent = path.fileparts[path.fileparts.Count - 1];
                 for (int i = path.fileparts.Count; i < path.parts.Count - 1; i++)
                 {
                     File file = FileSystemInternal.instance.MakeFolder(path.parts[i]);
                     parent.AddChild(file);
                     parent = file;
                 }
                 returnValue = FileSystemInternal.instance.MakeFile(path.parts[path.parts.Count - 1]);

                 parent.AddChild(returnValue);
             }, sync);//should be always sync*/
            Path path = new Path(rawPath);
            File parent = path.fileparts[path.fileparts.Count - 1];
            for (int i = path.fileparts.Count; i < path.parts.Length - 1; i++)
            {
                File file = FileSystemInternal.instance.MakeFolder(path.parts[i]);
                parent.AddChild(file);
                parent = file;
            }
            File output = FileSystemInternal.instance.MakeFile(path.parts[path.parts.Length - 1]);

            parent.AddChild(output);
            return output;
        }
        public static File MakeFolder(string path, string name)
        {
            /* return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
             {
                 returnValue = FileSystemInternal.instance.MakeFolder(name);
                 File parent = FileSystemInternal.instance.GetFileByPath(path);
                 parent.AddChild(returnValue);
             }, sync);//should be always sync*/
            File file = FileSystemInternal.instance.MakeFolder(name);
            File parent = FileSystemInternal.instance.GetFileByPath(path);
            parent.AddChild(file);
            return file;
        }
        public static bool RemoveFile(string path)
        {
           /* return ScriptManager.AddDelegateToStack((ref bool done, ref bool returnValue) =>
            {
                returnValue = FileSystemInternal.instance.RemoveFile(path);
            }, sync);//should be always sync*/
            return FileSystemInternal.instance.RemoveFile(path);

        }
      /*  public static string MakeAbsolutePath(string path, File currentFile = null)
        {
            /*  return ScriptManager.AddDelegateToStack((ref bool done, ref string returnValue) =>
              {
                  returnValue = FileSystemInternal.instance.MakeAbsolutePath(path, currentFile);
              }, sync);//should be always sync*/
          /* return FileSystemInternal.instance.MakeAbsolutePath(path, currentFile);

        }*/

    }
}