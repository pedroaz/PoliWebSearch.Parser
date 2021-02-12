using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp.Commands
{
    /// <summary>
    /// Commands Manager interface. Handles the user commands input
    /// </summary>
    public interface ICommandsManager
    {
        /// <summary>
        /// Main Console loop
        /// Ask the use for commands and process them
        /// </summary>
        /// <returns></returns>
        Task Loop();

        /// <summary>
        /// Execute a single command. If a second parameter is passsed on the console app this method will be executed
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Returns true if the application should end</returns>
        Task ExecuteListOfCommands(List<string> commands);

        /// <summary>
        /// Execute a single command. If a second parameter is passsed on the console app this method will be executed
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Returns false if the application should end</returns>
        Task<bool> ExecuteCommand(string commands);
    }
}
