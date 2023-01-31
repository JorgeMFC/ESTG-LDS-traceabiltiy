namespace MyTrace.Domain.Exceptions
{
    public class LotArgumentException : Exception
    {
        public LotArgumentException() { }

        public LotArgumentException(string message)
            : base(message) { }

        public LotArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public LotArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
