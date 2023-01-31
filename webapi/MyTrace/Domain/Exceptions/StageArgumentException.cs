namespace MyTrace.Domain.Exceptions
{
    public class StageArgumentException : Exception
    {
        public StageArgumentException() { }

        public StageArgumentException(string message)
            : base(message) { }

        public StageArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public StageArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
