namespace MyTrace.Domain.Exceptions
{
    public class StagesModelStageArgumentException : Exception
    {
        public StagesModelStageArgumentException() { }

        public StagesModelStageArgumentException(string message)
            : base(message) { }

        public StagesModelStageArgumentException(string message, Exception inner)
            : base(message, inner) { }

        public StagesModelStageArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
