namespace MyTrace.Domain.Exceptions
{
    public class ColorSizesException: Exception
    {
        public ColorSizesException() { }

        public ColorSizesException(string message)
            : base(message) { }

        public ColorSizesException(string message, Exception inner)
            : base(message, inner) { }

        public ColorSizesException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
