namespace PoliWebSerach.Parser.DB.Operator
{
    public class VerticeFilter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public VerticeFilter(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
