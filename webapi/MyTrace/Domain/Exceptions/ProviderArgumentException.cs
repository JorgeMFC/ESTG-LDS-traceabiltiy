namespace MyTrace.Domain.Exceptions
{
    public class ProviderArgumentException : Exception
    {
        public ProviderArgumentException() { }

        public ProviderArgumentException(string message)
            : base(message) { }

        public ProviderArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public ProviderArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
