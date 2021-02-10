namespace PoliWebSerach.Parser.DB.Operator
{
    /// <summary>
    /// Filter used to find a vertice
    /// </summary>
    public class VerticeFilter
    {
        /// <summary>
        /// The name of the property used by the filter
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Value which should be matched by the filter
        /// </summary>
        public string PropertyValue { get; set; }

        public VerticeFilter(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
