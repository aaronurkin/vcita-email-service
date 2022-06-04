using System.Collections.Generic;

namespace Core31.Consumers.CreateEmail
{
    public class CreateEmailRequestHandlerOptions
    {
        public IEnumerable<string> Topics { get; internal set; }
    }
}