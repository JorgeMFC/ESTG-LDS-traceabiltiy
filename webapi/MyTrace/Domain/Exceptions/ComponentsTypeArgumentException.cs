namespace MyTrace.Domain.Exceptions
{
    public class ComponentsTypeArgumentException : Exception
    {
        public ComponentsTypeArgumentException() { }

        public ComponentsTypeArgumentException(string message)
            : base(message) { }

        public ComponentsTypeArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public ComponentsTypeArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
