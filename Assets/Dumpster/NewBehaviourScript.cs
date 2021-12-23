//#include "/System/libraries/tools.ll"

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.output;
using Libraries.system.output.graphics.mask_texture;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;
using Libraries.system.shell;

#if false
//#include "/Syste/libraries/tools.ll"
//#using static FontLibrary;
public class FontLibrary
{
    static MaskTexture fontTexture = null;
    static Regex colorTagRegex = new Regex(@"^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))");

    static Regex backgroundColorTagRegex =
        new Regex(
            @"^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc)))");

    static Regex nonParseTagRegex = new Regex(@"^(\s*?\/?((raw)|(no-parsing)|(np)))");

    public static void Init()
    {
        File fontFile = fileSystem.GetFileByPath("/System/defaultFontMask");
        fontTexture = MaskTexture.FromData(fontFile.data);
    }

    static void DrawColoredCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character,
        SystemColor foreground, SystemColor background)
    {
        if (systemScreenBuffer == null)
        {
            //todo-future add error
            return;
        }

        int index = Runtime.CharToByte(character);
        int posx = index % 16;
        int posy = index / 16;

        systemScreenBuffer.DrawTexture(x, y, fontTexture.GetRect(posx * 8, (posy) * 8, 8, 8).Convert<SystemColor>
                (o => (o ? foreground : background)),
            fontTexture.transparencyFlag);
    }

    static void DrawColoredStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text,
        SystemColor foreground, SystemColor background, bool enableTags = true)
    {
        SystemColor currentForeground = foreground, currentBackground = background;
        int posX = x;
        int posY = y;
        char[] charText = text.ToCharArray();
        bool parseTags = true;
        for (int i = 0; i < text.Length; i++)
        {
            char character = charText[i];
            if (character == '\r')
            {
                posX = x;
                continue;
            }
            else if (character == '\n')
            {
                posX = x;
                posY += 8;
                continue;
            }
            else if (enableTags) //tags enabled
            {
                if (character == '<') //found tag
                {
                    if (i < 1 || charText[i - 1] != '\\') //not escaped
                    {
                        int endTagIndex = text.IndexOf('>', i);
                        if (endTagIndex != -1)
                        {
                            string tagText = text.Substring(i + 1, endTagIndex - i - 1).Trim();
                            if (nonParseTagRegex.IsMatch(tagText))
                            {
                                parseTags = (tagText.StartsWith("/"));
                                i = endTagIndex;
                                continue;
                            }

                            if (parseTags)
                            {
                                if (colorTagRegex.IsMatch(tagText))
                                {
                                    if (tagText.StartsWith("/"))
                                    {
                                        currentForeground = foreground;
                                    }
                                    else
                                    {
                                        int equalsIndex = tagText.IndexOf('=');
                                        string color = tagText.Substring(equalsIndex + 1);
                                        Console.Debug(
                                            $"match c! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                        if (color.Length == 1)
                                        {
                                            currentForeground = Runtime.HexToByte(color);
                                        }
                                    }
                                }

                                if (backgroundColorTagRegex.IsMatch(tagText))
                                {
                                    if (tagText.StartsWith("/"))
                                    {
                                        currentBackground = background;
                                    }
                                    else
                                    {
                                        int equalsIndex = tagText.IndexOf('=');
                                        string color = tagText.Substring(equalsIndex + 1);
                                        Console.Debug(
                                            $"match bckc! trimmed'{tagText}' equalsIndex'{equalsIndex}' color'{color}'.");
                                        if (color.Length == 1)
                                        {
                                            currentBackground = Runtime.HexToByte(color);
                                        }
                                    }
                                }


                                i = endTagIndex;

                                continue;
                            }
                        }
                    }
                }
            }
            else if (character == '\\')
            {
                continue;
            }

            DrawColoredCharAt(systemScreenBuffer, posX, posY, character, currentForeground, currentBackground);
            posX += 8;
        }
    }

    static void DrawCharAt(SystemScreenBuffer systemScreenBuffer, int x, int y, char character)
    {
        DrawColoredCharAt(systemScreenBuffer, x, y, character, SystemColor.white, SystemColor.black);
    }

    static void DrawStringAt(SystemScreenBuffer systemScreenBuffer, int x, int y, string text, bool enableTags = true)
    {
        DrawColoredStringAt(systemScreenBuffer, x, y, text, SystemColor.white, SystemColor.black, enableTags);
    }

    static string GetColorTag(SystemColor color)
    {
        return $"<color={Runtime.ByteToHex(color, false)}>";
    }

    static string GetBackgroundColorTag(SystemColor color)
    {
        return $"<color={Runtime.ByteToHex(color, false)}>";
    }
}

FontLibrary.Init();
#endif

#if false
    public class ls : ExtendedShellProgram
    {
        public override string GetName()
        {
            return "ls";
        }
        private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-p", "path", true),
        new AcceptedArgument("-r", "recursive", false),
        new AcceptedArgument("-jn", "just names", false),
        new AcceptedArgument("-sz", "show size", false),
        new AcceptedArgument("-fp", "full paths instead of names", false),
    };

        protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

        protected override string InternalRun(Dictionary<string, string> argPairs)
        {
            string wdPath = "/";
            if (argPairs.TryGetValue("-wd", out wdPath))
            {
            }
            File workingDirectory = fileSystem.GetFileByPath(wdPath);
            string path = "/";
            if (argPairs.TryGetValue("-p", out path))
            {
            }
            File f = fileSystem.GetFileByPath(path, workingDirectory);
            Console.Debug($"{f} {path}");
            return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
        }
        string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths)
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
 GetChildren(child, indent + (onlyNames ? 0 : 1), $"{(last ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths);

                    }
                }
            }
            return str;
        }
    }

#endif
#if false
public class mkf : ExtendedShellProgram
{
    public override string GetName()
    {
        return "mkf";
    }
    private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-p", "path", true),
        new AcceptedArgument("-d", "data", true),
        new AcceptedArgument("-n", "name", true),
        new AcceptedArgument("-fp", "file permissions", true),

    };

    protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

    protected override string InternalRun(Dictionary<string, string> argPairs)
    {
        string wdPath = argPairs.GetValueOrDefault("-wd", "/");
        File workingDirectory = fileSystem.GetFileByPath(wdPath);

        string path = argPairs.GetValueOrDefault("-p", "/");
        File f = fileSystem.GetFileByPath(path, workingDirectory);

        string name = argPairs.GetValueOrDefault("-n", "new file");
        short fpInt;
        if (!short.TryParse(argPairs.GetValueOrDefault("-fp", "0b0000111"), out fpInt))
        {
            fpInt = 0b0000111;
        }
        FilePermission fp = (FilePermission)fpInt;
        File newFile = fileSystem.MakeFile(wdPath, name, fp, null);

        return $"Created file {newFile.name} at {newFile.Parent.GetFullPath()}.";
    }

}

#endif