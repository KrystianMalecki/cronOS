//#include "/System/libraries/tools.ll"

using System.Text.RegularExpressions;
using Libraries.system;
using Libraries.system.file_system;
using Libraries.system.output;
using Libraries.system.output.graphics.mask_texture;
using Libraries.system.output.graphics.system_colorspace;
using Libraries.system.output.graphics.system_screen_buffer;

public class FontLibrary
{
    static MaskTexture fontTexture = null;
    static Regex colorTagRegex = new Regex(@"^(\s*?((color)|(c)|(col))\s*?=\s*?.+?)|(\s*?\/((color)|(c)|(col)))");

    static Regex backgroundColorTagRegex =
        new Regex(
            @"^(((\s*?)|(\/))((backgroundcolor)|(bgc)|(bgcol)|(bc))\s*?=.+?)|(\s*?\/((backgroundcolor)|(bgc)|(bgcol)|(bc)))");

    static Regex nonParseTagRegex = new Regex(@"^(\s*?\/?((raw)|(no-parsing)|(np)))");

    public void Init()
    {
        File fontFile = hardware.fileSystem.GetFileByPath("/System/defaultFontMask");
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