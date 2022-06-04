using Confluent.Kafka;
using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core31.Consumers.CreateEmail
{
    internal class CreateEmailRequestHandler : IHostedService
    {
        private readonly ILogger logger;
        private readonly CreateEmailRequestHandlerOptions options;
        private readonly ISubscriber<CreateEmailRequest> subscriber;

        public CreateEmailRequestHandler(
            CreateEmailRequestHandlerOptions options,
            ILogger<CreateEmailRequestHandler> logger,
            ISubscriber<CreateEmailRequest> subscriber
        )
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.options = options ?? throw new System.ArgumentNullException(nameof(options));
            this.subscriber = subscriber ?? throw new System.ArgumentNullException(nameof(subscriber));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.subscriber.Subscribe(this.options.Topics.ToArray());

                foreach (var request in this.subscriber.GetMessages(cancellationToken))
                {
                    this.logger.LogInformation("Received email: {0}", request.Email);
                }
            }
            catch (System.OperationCanceledException)
            {
                this.logger.LogWarning("Operation canceled");
                await this.StopAsync(cancellationToken);
            }
            catch (System.Exception exception)
            {
                this.logger.LogError(exception, "FAILED {0}", nameof(CreateEmailRequestHandler));
                await this.StopAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (subscriber != null)
            {
                this.logger.LogInformation("Closing the {0} consumer...", nameof(CreateEmailRequest));
                this.subscriber.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}
