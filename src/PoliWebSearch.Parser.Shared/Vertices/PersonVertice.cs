using PoliWebSearch.Parser.Domain.Attributes;
using PoliWebSearch.Parser.Domain.Edges.Base;
using System;
using System.Collections.Generic;

namespace PoliWebSearch.Parser.Domain.Vertices
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

        /// <summary>
        /// Name of the Cpf property
        /// </summary>
        [IgnoreProperty]
        public static string CpfPropertyName => nameof(Cpf);

        /// <summary>
        /// Candidate possible emails
        /// </summary>
        public List<string> Emails { get; set; }

        /// <summary>
        /// Birth Date of the person
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// Genders that the person was once identified with
        /// </summary>
        public List<string> Genders { get; set; }

        /// <summary>
        /// Nacionality of the person. Can have more than one
        /// </summary>
        public List<string> Nationality { get; set; }

        /// <summary>
        /// Registred skin color of the person
        /// </summary>
        public List<string> SkinColor { get; set; }
    }
}
