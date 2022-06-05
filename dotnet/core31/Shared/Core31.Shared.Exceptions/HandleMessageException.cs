using System;
using System.Runtime.Serialization;

namespace Core31.Shared.Exceptions
{
    public class HandleMessageException : Exception
    {
        public HandleMessageException()
            : base()
        {
        }

#nullable enable
        public HandleMessageException(string? message)
            : base(message)
        {
        }

#nullable enable
        public HandleMessageException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        public HandleMessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
