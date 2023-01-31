namespace MyTrace.Models
{
    public class SwaggerHeaderAttribute : Attribute
    {
        public string HeaderName { get; }
        public string Description { get; }
        public string? DefaultValue { get; }
        public bool IsRequired { get; }

        public SwaggerHeaderAttribute(string headerName, string description, string? defaultValue, bool isRequired)
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }

        public SwaggerHeaderAttribute(bool isRequired = false)
        {
            HeaderName = "Authorization";
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"";
            DefaultValue = null;
            IsRequired = isRequired;
        }
    }
}
