using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Helpers
{
    public static class GlobalHelper
    {
        static Regex spaceQArgsRegex = new Regex(@"( +)|([\\(\\),;])|(\\\""|\""(?:\\\""|[^\""])*\""|(\\+))");
        static Regex spaceAndQRegex = new Regex(@"( +)|(\\\""|\""(?:\\\""|[^\""])*\""|(\\+))");
        public static Regex betweenSpacesAndInsideQuotes = new Regex(@"(?:(['""])(.*?)(?<!\\)(?>\\\\)*\1|([^\s]+))", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static int CountEncounters(this string input, params char[] toFind)
        {
            int counter = 0;
            foreach (char c in input)
            {
                foreach (char compareTo in toFind)
                {
                    if (compareTo == c)
                    {
                        counter++;
                        break;
                    }
                }
            }

            return counter;
        }

        public static List<string> SplitSpaceQ(this string input)
        {
            return spaceAndQRegex.Split(input).ToList();
        }

        public static List<string> SplitSpaceQCustom(this string input, string custom)
        {
            return new Regex(@"( +)|([" + custom + @"])|(\\\""|\""(?:\\\""|[^\""])*\""|(\\+))").Split(input).ToList();
        }

        public static List<string> SplitSpaceQArgs(this string input)
        {
            return spaceQArgsRegex.Split(input).ToList();
        }

        public static List<string> SplitSpaceQArgsCustom(this string input, string custom)
        {
            return new Regex(@"( +)|([\\(\\)," + custom + @"])|(\\\""|\""(?:\\\""|[^\""])*\""|(\\+))").Split(input)
                .ToList();
        }

        private static readonly string[] newLiners = new string[]
        {
            Environment.NewLine, "\n", // "\r"
        };

        public static string[] SplitNewLine(this string input)
        {
            return input.Split(newLiners, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ChangeToPrefixedValue(this int num)
        {
            int sizeScale = 0;
            int currentSize = num;
            while (currentSize > 1024)
            {
                currentSize = currentSize / 1024;
                sizeScale++;
            }

            return $"{currentSize}{SIZES[sizeScale]}";
        }


        private static readonly string[] SIZES = { "", "k", "M", "G", "T" };

        public static Rect Add(this Rect rect1, Rect rect2)
        {
            return new Rect(rect1.x + rect2.x, rect1.y + rect2.y, rect1.width + rect2.width,
                rect1.height + rect2.height);
        }
        public static string[] StringMatches(this Regex regex, string input)
        {
            var matches = regex.Matches(input);
            var result = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                result[i] = matches[i].Value;
            }
            return result;
        }
        public static string[] SplitTextBySpaces(string input, bool removeQuotes = false)
        {
            string[] parts = StringMatches(betweenSpacesAndInsideQuotes, input);
            if (removeQuotes)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Trim('"');
                }
            }
            return parts;

        }

    }
}