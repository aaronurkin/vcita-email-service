using System;
using System.Threading.Tasks;

namespace Core31.Shared.Services
{
    public interface IPublisher<TMessageValue> : IDisposable
        where TMessageValue : class
    {
        Task Publish(TMessageValue value, string topic);
    }
}
