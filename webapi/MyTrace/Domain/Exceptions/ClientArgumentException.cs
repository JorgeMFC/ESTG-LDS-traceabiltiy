namespace MyTrace.Domain.Exceptions
{
    public class ClientArgumentException : Exception
    {
        public ClientArgumentException() { }

        public ClientArgumentException(string message)
            : base(message) { }

        public ClientArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public ClientArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
