using Libraries.system.output.graphics.system_colorspace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace helper
{
    public static class GlobalHelper
    {
        public static List<string> SplitString2Q(string input)
        {
            bool qSearch = false;
            List<string> output = new List<string>();
            string currentPart = "";
            bool lastCharIsEscape = false;
            for (int i = 0; i < input.Length; i++)
            {

                char c = input[i];
                if (c == '"' && !lastCharIsEscape)
                {
                    if (qSearch)
                    {
                        qSearch = false;
                        output.Add(currentPart);
                        currentPart = "";
                    }
                    else
                    {
                        qSearch = true;
                    }
                }
                else if (c == ' ' && !qSearch)
                {
                    output.Add(currentPart);
                    currentPart = "";
                }
                else
                {
                    if (c == '\\')
                    {
                        lastCharIsEscape = true;
                    }
                    else
                    {
                        lastCharIsEscape = false;
                        currentPart += c;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(currentPart) || !string.IsNullOrEmpty(currentPart))
            {
                output.Add(currentPart);
            }
            return output;
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
    }
}