namespace MyTrace.Domain.Exceptions
{
    public class UserArgumentException : Exception
    { 
        public UserArgumentException() { }

        public UserArgumentException(string message)
            : base(message) { }

        public UserArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public UserArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
