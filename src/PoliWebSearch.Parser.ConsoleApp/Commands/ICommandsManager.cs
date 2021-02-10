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
    }
}
