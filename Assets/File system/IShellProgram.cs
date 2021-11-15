using Libraries.system;
using Libraries.system.file_system;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Libraries.system
{
    namespace shell
    {
        public interface IShellProgram
        {
            public string Run(params string[] args);
        }
        public class ExtendedShellProgram : IShellProgram
        {
            protected virtual List<AcceptedArgument> argumentTypes => null;
            public virtual string Run(string arg)
            {
                return Run(StaticHelper.SplitString2Q(arg).ToArray());
            }
            public virtual string Run(params string[] args)
            {
                Debug.Log(args.ToArray().GetValuesToString());
                Dictionary<string, string> argPairs = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++)
                {
                    AcceptedArgument argument = argumentTypes.Find(x => x.name == args[i]);
                    if (argument != null)
                    {
                        if (argument.valued)
                        {
                            argPairs.Add(args[i], args[i + 1]);
                            i++;
                        }
                        else
                        {
                            argPairs.Add(args[i], "");
                        }
                    }
                }
                return InternalRun(argPairs);
            }
            protected virtual string InternalRun(Dictionary<string, string> argPairs)
            {
                return "";
            }
            public class AcceptedArgument
            {
                public string name;
                public string description;
                public bool valued;

                public AcceptedArgument(string name, string description, bool valued)
                {
                    this.name = name;
                    this.description = description;
                    this.valued = valued;
                }
            }
        }
        public class ls : ExtendedShellProgram
        {
            private static ls _instance;
            public static ls instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new ls();
                    }
                    return _instance;
                }
            }
            private static readonly List<AcceptedArgument> _argumentTypes = new List<AcceptedArgument> {
        new AcceptedArgument("-wd", "working directory", true),
        new AcceptedArgument("-r", "recursive", false),
        new AcceptedArgument("-jn", "just names", false),
                new AcceptedArgument("-sz", "show size", false),
                new AcceptedArgument("-fp", "full paths instead of names", false),
    };

            protected override List<AcceptedArgument> argumentTypes => _argumentTypes;

            protected override string InternalRun(Dictionary<string, string> argPairs)
            {
                string path = "/";
                if (argPairs.TryGetValue("-wd", out path))
                {
                }
                File f = FileSystem.GetFileByPath(path);

                return GetChildren(f, 0, "", argPairs.ContainsKey("-r"), argPairs.ContainsKey("-sz"), argPairs.ContainsKey("-jn"), argPairs.ContainsKey("-fp"));
            }
            string GetChildren(File file, int indent, string prefix, bool recursive, bool showSize, bool onlyNames, bool fullPaths)
            {
                string str = string.Format(
                   onlyNames ? "{2}" : "{0," + indent + "}{1} {2}:{3}\n"
                    , "", prefix, fullPaths ? file.GetFullPath() : file.name, file.GetByteSize());
                if (recursive)
                {
                    for (int i = 0; i < file.children.Count; i++)
                    {
                        File child = file.children[i];
                        str += GetChildren(child, indent + (onlyNames ? 0 : 1), $"{((i + 1) == file.children.Count ? Runtime.ByteToChar(192) : Runtime.ByteToChar(195))}", recursive, showSize, onlyNames, fullPaths);

                    }
                }
                return str;
            }


        }
    }
}