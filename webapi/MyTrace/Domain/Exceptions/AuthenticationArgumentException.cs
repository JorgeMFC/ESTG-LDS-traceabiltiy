namespace MyTrace.Domain.Exceptions
{
    public class AuthenticationArgumentException : Exception
    {
        public AuthenticationArgumentException() { }

        public AuthenticationArgumentException(string message)
            : base(message) { }

        public AuthenticationArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public AuthenticationArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
