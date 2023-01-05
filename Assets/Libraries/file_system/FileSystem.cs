namespace Libraries.system.file_system
{
    public class FileSystem : BaseLibrary
    {
        public const char catalogSymbol = '/';

        public static File GetFileByPath(string path, File parent = null)
        {
            //  Debug.Log($"{Hardware.currentThreadInstance}-{Hardware.currentThreadInstance.hardwareInternal}-{Hardware.currentThreadInstance.hardwareInternal.mainDrive}-{Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive}");
            return Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.GetFileByPath(path, parent);
        }

        public static File MakeFile(string path, string name, FilePermission filePermission, byte[] data = null)
        {
            File file = Drive.MakeFile(name, data);
            file.permissions = filePermission;
            File parent = Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.GetFileByPath(path);
            parent.SetChild(file);
            return file;
        }

        public static File MakeFile(string rawPath)
        {
            string[] path = rawPath.Split(catalogSymbol);

            File currentFile = Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.GetRoot();
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
            File parent = Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.GetFileByPath(path);
            parent.SetChild(file);
            return file;
        }

        public static bool RemoveFile(string path)
        {
            return Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.RemoveFile(path);
        }

        public static bool HasFile(string path)
        {
            return Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.HasFile(path);
        }

        public static bool TryGetFile(string path, out File file)
        {
            return Hardware.currentThreadInstance.hardwareInternal.mainDrive.drive.TryGetFile(path, out file);
        }
    }
}