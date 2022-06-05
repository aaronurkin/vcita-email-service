using System;
using System.Collections.Generic;
using System.Threading;

namespace Core31.Shared.Services
{
    public interface ISubscriber<TMessageValue> : IDisposable
        where TMessageValue : class
    {
        void Subscribe(params string[] topics);
        IEnumerable<TMessageValue> GetMessages(CancellationToken cancellationToken);
        void HandleMessages(CancellationToken cancellationToken, Action<TMessageValue> handleMessage = null);
    }
}
