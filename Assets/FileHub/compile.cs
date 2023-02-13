#if true


using Libraries.system.debug;
using Libraries.system.file_system;
using System.Collections.Generic;
using Arguments;
public class compile
{
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
            Dictionary<AcceptedArgument, string> argPairs = AcceptedArgumentDictionaryExtensions.SplitArgumentsToDictionary(argumentTypes, args);
            string wdPath = argPairs.GetValueOrNull("-wd")?.Value ?? Hardware.currentFile.GetFullPath();
            Debugger.Debug("p1" + argPairs.ToFormattedString());
            Debugger.Debug("p2" + argPairs.GetValueOrNull("-wd"));
            Debugger.Debug("p3" + argPairs.GetValueOrNull("-wd")?.Value);

            File workingDirectory = FileSystem.GetFileByPath(wdPath);
            string path = argPairs.GetValueOrNull("-p")?.Value ?? "./";


            File f = FileSystem.GetFileByPath(path, workingDirectory);



        }
        catch (Exception e)
        {
            Debugger.Debug(e);
        }
    }


    public static void Compile()
    {


        if (parts.Length > 1)
        {
            File file = FileSystem.GetFileByPath(parts[1], currentFile);
            if (file != null)
            {
                File compileFile = Runtime.Compile(file);
                if (compileFile != null)
                {
                    compileFile = file.Parent.SetChild(compileFile);
                    return $"Compiled '{file.GetFullPath()}' to '{compileFile.GetFullPath()}'";
                }
                else
                {
                    return $"Failed to compile '{file.GetFullPath()}'";
                }
            }
            else
            {
                return $"File '{parts[1]}' not found";
            }
        }


    }

}


#endif