namespace MyTrace.Domain.Exceptions
{
    public class StagesModelArgumentException : Exception
    {
        public StagesModelArgumentException() { }

        public StagesModelArgumentException(string message)
            : base(message) { }

        public StagesModelArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public StagesModelArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
