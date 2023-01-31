namespace MyTrace.Domain.Exceptions
{
    public class ComponentArgumentException : Exception
    {
        public ComponentArgumentException() { }

        public ComponentArgumentException(string message)
            : base(message) { }

        public ComponentArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public ComponentArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
