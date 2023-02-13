using Helpers;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
namespace Arguments
{
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

    public static class AcceptedArgumentDictionaryExtensions
    {
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
    }
}