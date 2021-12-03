using helper;
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
                return Run(arg.SplitSpaceQ().ToArray());
            }
            public virtual string Run(params string[] args)
            {

                Dictionary<string, string> argPairs = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++)
                {
                    AcceptedArgument argument = argumentTypes.Find(x => x.name == args[i]);
                    if (argument != null)
                    {
                        if (argument.valued)
                        {
                            if (args.Length > i + 1)
                            {
                                argPairs.Add(args[i], args[i + 1]);

                                i++;
                            }
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

    }
}