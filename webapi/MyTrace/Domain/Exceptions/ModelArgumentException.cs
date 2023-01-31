namespace MyTrace.Domain.Exceptions
{
    public class ModelArgumentException : Exception
    {
        public ModelArgumentException() { }

        public ModelArgumentException(string message)
            : base(message) { }

        public ModelArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public ModelArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
