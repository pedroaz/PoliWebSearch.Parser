using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.ConsoleApp.Arguments
{
    public class ProgramArguments
    {
        public string EnvFolder { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;

        public bool HasEnvFolder => !EnvFolder.Equals(string.Empty);
        public bool HasCommand => !Command.Equals(string.Empty);

        public ProgramArguments(string[] args)
        {
            if(args.Length > 0) {
                EnvFolder = args[0];
            }

            if(args.Length > 1) {
                Command = args[1];
            }
        }
    }
}
