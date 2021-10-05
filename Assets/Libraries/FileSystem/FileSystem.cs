using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Libraries.system.filesystem
{
    public class FileSystem : BaseLibrary
    {
        public static File GetFileByPath(string path)
        {
            return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
            {
                returnValue = FileSystemInternal.instance.GetFileByPath(path);
            }, sync);//should be always sync
        }
        public static File MakeFile(string path, string name, string extension = "", byte[] data = default(byte[]))
        {
            return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
            {
                returnValue = FileSystemInternal.instance.GetFileByPath(path);
            }, sync);//should be always sync
        }
        public static File MakeFolder(string path,string name)
        {
            return MakeFile(path, name, "DIR");
        }
        public static File RemoveFile(string path)
        {
            return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
            {
                returnValue = FileSystemInternal.instance.GetFileByPath(path);
            }, sync);//should be always sync
        }

        public static File Make(string path)
        {
            return ScriptManager.AddDelegateToStack((ref bool done, ref File returnValue) =>
            {
                returnValue = FileSystemInternal.instance.GetFileByPath(path);
            }, sync);//should be always sync
        }
    }
}