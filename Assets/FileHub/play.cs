#if false
public class play : ExtendedShellProgram
{
    public override string GetName()
    {
        return "play";
    }

    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument>
    {
        new AcceptedArgument("working directory", true, "-wd"),
        new AcceptedArgument("note", true, "-nt"),
        new AcceptedArgument("length", true, "-l"),
        new AcceptedArgument("octave", true, "-oc"),
    };

    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

    protected override string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
    {
        string stringNote = argPairs.GetValueOrNull("-nt")?.Value;
        string stringLength = argPairs.GetValueOrNull("-l")?.Value ?? "1.0";
        string stringOctave = argPairs.GetValueOrNull("-oc")?.Value ?? "4";
        int note = 13;
        if (!int.TryParse(stringNote, out note))
        {
            note = AudioHandler.StringToNote(stringNote);
        }

        if (!float.TryParse(stringLength, out float length))
        {
        }

        Debugger.Debug($"{length} {stringLength} {stringLength == null}");
        if (!int.TryParse(stringOctave, out int octave))
        {
        }

        audioHandler.PlaySound(new Sound(note, octave, length));
        return $"Playing note {note}.";
    }
}
#endif