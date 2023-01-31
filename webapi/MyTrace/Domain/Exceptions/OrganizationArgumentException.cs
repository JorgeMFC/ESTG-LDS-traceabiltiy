namespace MyTrace.Domain.Exceptions
{
    public class OrganizationArgumentException : Exception
    {
        public OrganizationArgumentException() { }

        public OrganizationArgumentException(string message)
            : base(message) { }

        public OrganizationArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public OrganizationArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
