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
    }
}