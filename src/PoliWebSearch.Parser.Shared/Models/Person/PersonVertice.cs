using System.Collections.Generic;

namespace PoliWebSearch.Parser.Shared.Models.Person
{
    /// <summary>
    /// Data representation of a person vertice
    /// </summary>
    public class PersonVertice : EdgeDataModel
    {
        /// <summary>
        /// All names which the person is related
        /// </summary>
        public List<string> Names { get; set; }
        /// <summary>
        /// Unique CPF identification
        /// </summary>
        public string Cpf { get; set; }
    }
}
