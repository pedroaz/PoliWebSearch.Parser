namespace PoliWebSearch.Parser.Domain.Vertices
{
    /// <summary>
    /// Occupation which a person can have
    /// </summary>
    public class ProfessionalOccupationVertice
    {

        /// <summary>
        /// The code of the professions
        /// </summary>
        public string OccupationCode { get; set; }

        /// <summary>
        /// Name of the profession
        /// </summary>
        public string ProfessionName { get; set; }
    }
}
