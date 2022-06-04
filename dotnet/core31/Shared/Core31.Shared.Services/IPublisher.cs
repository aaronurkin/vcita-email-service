using System;

namespace Core31.Shared.Services
{
    public interface IPublisher<TMessageValue> : IDisposable
        where TMessageValue : class
    {
    }
}
