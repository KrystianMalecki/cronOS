#if false
//imported: /sys/libs/commands.lib
/*using System.Collections.Generic;
using System.Linq;*/
public struct AcceptedArgument
{
    public string[] aliases;
    public string description;
    public bool valued;
    public AcceptedArgument(string description, bool valued, params string[] aliases)
    {
        this.aliases = aliases;
        this.description = description;
        this.valued = valued;
    }
    public bool HasAlias(string alias)
    {
        return aliases.Contains(alias);
    }
}
/*namespace Extensions
{
    public static class AcceptedArgumentDictionaryExtensions
    {*/
        public static KeyValuePair<AcceptedArgument, string>? GetValueOrNull(
            this Dictionary<AcceptedArgument, string> argPairs,
            string key)
        {
            var pair = argPairs.FirstOrDefault(x => x.Key.aliases.Contains(key));
            if (pair.Equals(default(KeyValuePair<AcceptedArgument, string>)))
            {
                return null;
            }
            return pair;
        }
        public static bool ContainsAlias(this Dictionary<AcceptedArgument, string> argPairs,
            string key)
        {
            return argPairs.Any(keyValuePair => keyValuePair.Key.aliases.Contains(key));
        }
        public static string[] SplitArgumentsToArray(string arg)
        {
            return arg.SplitSpaceQ().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
        public static Dictionary<AcceptedArgument, string> SplitArgumentsToDictionary(List<AcceptedArgument> argumentTypes, params string[] args)
        {
            Dictionary<AcceptedArgument, string> argPairs = new Dictionary<AcceptedArgument, string>();
            for (int i = 0; i < args.Length; i++)
            {
                int index = argumentTypes.FindIndex(x => x.HasAlias(args[i]));
                if (index != -1)
                {
                    AcceptedArgument argument = argumentTypes[index];
                    if (argument.valued)
                    {
                        if (args.Length > i + 1)
                        {
                            argPairs.Add(argument, args[i + 1]);
                            i++;
                        }
                    }
                    else
                    {
                        argPairs.Add(argument, null);
                    }
                }
            }
            return argPairs;
        }
   /* }
}*/
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
        Debugger.Debug(argPairs.ToFormattedString())
        File workingDirectory = FileSystem.GetFileByPath(wdPath);
        string path = argPairs.GetValueOrNull("-p")?.Value ?? "./";
        File f = FileSystem.GetFileByPath(path, workingDirectory);
        Debugger.Debug($"{f} {path}");
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