namespace PoliWebSearch.Parser.Infra.Configurator
{
    /// <summary>
    /// Configuration data for the application
    /// </summary>
    public class AppConfigurationData
    {
        /// <summary>
        /// The env directory where the application will reference
        /// </summary>
        public string EnvDirectory { get; set; }
        /// <summary>
        /// The storage directory where all the files should be
        /// </summary>
        public string StorageDirectory { get; set; }
        /// <summary>
        /// Database name used by the database service
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Name of the graph used by the database service
        /// </summary>
        public string GraphName { get; set; }
        /// <summary>
        /// Host name of the gremlim server
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Master key of the gramlim server
        /// </summary>
        public string MasterKey { get; set; }
        /// <summary>
        /// Where the log directory is
        /// </summary>
        public string LogDirectory { get; set; }
        /// <summary>
        /// Where the results are
        /// </summary>
        public string ResultsDirectory { get; set; }

        /// <summary>
        /// Get the execution ID based on the files from log / result folder
        /// </summary>
        public string ExecutionId { get; set; }
    }
}
