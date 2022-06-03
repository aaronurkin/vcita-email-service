using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Core31.Consumers.CreateEmail
{
    internal class CreateEmailRequestHandler : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
