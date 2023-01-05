#if false
//#include "/sys/libs/commands.lib"






public class ls
{
    private const string shortFormat = "{2} ";
    private const string longFormat = "{1}{2}:{3}\n";
    private static readonly List<AcceptedArgument> argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("path", true, "-p"),
        new AcceptedArgument("recursive", false, "-r"),
        new AcceptedArgument("just names", false, "-jn"),
        new AcceptedArgument("show size", false, "-sz"),
        new AcceptedArgument("full paths instead of names", false, "-fp"),
    };

    public static void Main(params string[] args)
    { 
    try
        {
        Dictionary<AcceptedArgument, string> argPairs = SplitArgumentsToDictionary(argumentTypes, args);
        string wdPath = argPairs.GetValueOrNull("-wd")?.Value ?? currentFile.GetFullPath();
        Debugger.Debug("p1"+argPairs.ToFormattedString());
                Debugger.Debug("p2"+argPairs.GetValueOrNull("-wd"));
                                Debugger.Debug("p3"+argPairs.GetValueOrNull("-wd")?.Value);

        File workingDirectory = FileSystem.GetFileByPath(wdPath);
        string path = argPairs.GetValueOrNull("-p")?.Value ?? "./";


        File f = FileSystem.GetFileByPath(path, workingDirectory);
        Debugger.Debug("p4"+workingDirectory);
        Debugger.Debug($"0{f} {path}");
        Debugger.Debug($"r {argPairs.ContainsAlias("-r")} sz {argPairs.ContainsAlias("-sz")} jn{argPairs.ContainsAlias("-jn")} fp {argPairs.ContainsAlias("-fp")}");

        string output = GetChildren(
            f,
            0,
            "",
           recursive: argPairs.ContainsAlias("-r"),
           showSize: argPairs.ContainsAlias("-sz"),
           justNames: argPairs.ContainsAlias("-jn"),
           fullPaths: argPairs.ContainsAlias("-fp"));
        Debugger.Debug("1" + output);
        //  Debugger.Debug(Hardware);
       
            Debugger.Debug("2" + Hardware.Statics.Shell.balls);
            Debugger.Debug("3" + Hardware.Statics.Shell.thisShell);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(Hardware.Statics.Shell))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(Hardware.Statics.Shell);
                Debugger.Debug($"{name}={value}");
            }
            Debugger.Debug("4" + Hardware.Statics);
            Debugger.Debug("5" + Hardware.Statics.Shell.GetType());
            Hardware.Statics.Shell.WriteToConsole(output);
            Debugger.Debug("6" + output);
        }
        catch (Exception e)
        {
            Debugger.Debug(e);
        }
    }
    static string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool justNames,
         bool fullPaths)
    {
        string str = string.Format(
            justNames ?
            shortFormat :
            "{0," + indent + "}" + longFormat,
                "",
                prefix,
                fullPaths ? file.GetFullPath() : file.name,
                file.GetByteSize()
            );
        if (recursive)
        {
            if (file.children != null || true)
            {
                for (int i = 0; i < file.children?.Count; i++)
                {
                    char prefixCharacter = (((i + 1) == file.children?.Count) ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195));
                    File child = file.children[i];
                    str +=
                        GetChildren(
                            child,
                            indent + (justNames ? 0 : 1),
                            prefixCharacter.ToString(),
                            recursive,
                            showSize,
                            justNames,
                            fullPaths);
                }
            }
        }

        return str;
    }
}

#endif