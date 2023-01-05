#if false
public class shout : ExtendedShellProgram
{
    public override string GetName()
    {
        return "shout";
    }

    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("file path", true, "-p"),
        new AcceptedArgument("text", true, "-txt"),
        new AcceptedArgument("enable foramting", true, "-ef"),
    };

    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
   string wdPath= argPairs.GetValueOrNull("-wd")?.Value;
    File workingDirectory = fileSystem.GetFileByPath(wdPath);
        string stringPath = argPairs.GetValueOrNull("-p")?.Value;
        string stringText = argPairs.GetValueOrNull("-txt")?.Value ;
        string stringEnableFormating = argPairs.GetValueOrNull("-ef")?.Value??"true" ;
       File f=fileSystem.GetFileByPath(stringPath,workingDirectory);
       Debugger.Debug($"wdPath{wdPath} stringPath{stringPath} stringText{stringText} stringEnableFormating{stringEnableFormating} null{f==null}");
       
        if (String.IsNullOrEmpty(stringPath)|| f == null)
        {
            return stringText;
        }
        return $"File contents of {f.name}:\n{Runtime.BytesToEncodedString(f.data)}.";
    }
}
#endif