using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoliWebSearch.Parser.ConsoleApp.Arguments
{
    public class ProgramArguments
    {
        public string EnvFolder { get; set; } = string.Empty;
        public List<string> Commands { get; set; } = new List<string>();

        public bool HasEnvFolder => !EnvFolder.Equals(string.Empty);
        public bool HasCommands => Commands.Any();

        public ProgramArguments(string[] args)
        {
            if(args.Length > 0) {
                EnvFolder = args[0];
            }

            for (int i = 1; i < args.Length; i++) {
                Commands.Add(args[i]);
            }
        }
    }
}
