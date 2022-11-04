#if false
//todo -1 compile this and as binary
public class ls : ExtendedShellProgram
{
    public override string GetName()
    {
        return "ls";
    }

    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("path", true, "-p"),
        new AcceptedArgument("recursive", false, "-r"),
        new AcceptedArgument("just names", false, "-jn"),
        new AcceptedArgument("show size", false, "-sz"),
        new AcceptedArgument("full paths instead of names", false, "-fp"),
    };

    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;
    public static void Main(string[] args)
    {
        
    }
    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
        string wdPath = argPairs.GetValueOrNull("-wd")?.Value ?? currentFile.GetFullPath();


        File workingDirectory = FileSystem.GetFileByPath(wdPath);
        string path = argPairs.GetValueOrNull("-p")?.Value ?? "";


        File f = FileSystem.GetFileByPath(path, workingDirectory);
        Debugger.Debug($"{f} {path}");
        return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"),
            argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
    }

    string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames,
        bool fullPaths)
    {
        string str = string.Format(
            onlyNames ? "{2}" : "{0," + indent + "}{1}{2}:{3}\n"
            , "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize());
        if (recursive)
        {
            if (file.children != null || true)
            {
                for (int i = 0; i < file.children?.Count; i++)
                {
                    bool last = i + 1 == file.children.Count;
                    File child = file.children[i];
                    str +=
                        GetChildren(child, indent + (onlyNames ? 0 : 1),
                            $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize,
                            onlyNames, fullPaths);
                }
            }
        }

        return str;
    }
}
public class ls2 
{
}
#endif