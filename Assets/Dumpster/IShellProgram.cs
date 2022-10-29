using helper;
using Libraries.system;
using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Libraries.system.shell;
using UnityEngine;

namespace Libraries.system
{
    namespace shell
    {
        //todo 1 think if this is needed. Normal thing to do is to make it only require params string[] args Main or not even Main and if it is not found that do nothing.
        
        public interface IShellProgram
        {
            public string GetName();
            public string Run(params string[] args);
            public string Run(string arg);
        }

        public class ExtendedShellProgram : IShellProgram
        {
            protected virtual List<AcceptedArgument> argumentTypes => null;

            public virtual string GetName()
            {
                return "";
            }

            public virtual string Run(string arg)
            {
                return Run(arg.SplitSpaceQ().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            }

            public virtual string Run(params string[] args)
            {
                Dictionary<AcceptedArgument, string> argPairs = new Dictionary<AcceptedArgument, string>();
                for (int i = 0; i < args.Length; i++)
                {
                    AcceptedArgument argument = argumentTypes.Find(x => x.aliases.Contains(args[i]));
                    if (argument != null)
                    {
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
                            argPairs.Add(argument, "");
                        }
                    }
                }

                return InternalRun(argPairs);
            }

            protected virtual string InternalRun(Dictionary<AcceptedArgument, string> argPairs)
            {
                return "";
            }

            public class AcceptedArgument
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
            }
        }
    }

    public static class ArgDictionaryExtensions
    {
        public static KeyValuePair<ExtendedShellProgram.AcceptedArgument, string>? GetValueOrNull(
            this Dictionary<ExtendedShellProgram.AcceptedArgument, string> argPairs,
            string key)
        {
            KeyValuePair<ExtendedShellProgram.AcceptedArgument, string> pair =
                argPairs.FirstOrDefault(x => x.Key.aliases.Contains(key));
            if (!pair.Equals(default(KeyValuePair<ExtendedShellProgram.AcceptedArgument, string>)))

            {
                return pair;
            }
            else
            {
                return null;
            }
        }

        public static bool ContainsKey(this Dictionary<ExtendedShellProgram.AcceptedArgument, string> argPairs,
            string key)
        {
            return argPairs.Any(keyValuePair => keyValuePair.Key.aliases.Contains(key));
        }
    }
}