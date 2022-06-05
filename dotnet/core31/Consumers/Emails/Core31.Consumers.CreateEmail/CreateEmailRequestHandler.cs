using Core31.Consumers.CreateEmail.Data;
using Core31.EventSubscribers.Emails.Data;
using Core31.Shared.Exceptions;
using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core31.Consumers.CreateEmail
{
    internal class CreateEmailRequestHandler : IHostedService
    {
        private readonly ILogger logger;
        private readonly IEmailsRepository repository;
        private readonly CreateEmailRequestHandlerOptions options;
        private readonly ISubscriber<CreateEmailRequest> subscriber;

        public CreateEmailRequestHandler(
            IEmailsRepository repository,
            CreateEmailRequestHandlerOptions options,
            ILogger<CreateEmailRequestHandler> logger,
            ISubscriber<CreateEmailRequest> subscriber
        )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.subscriber.Subscribe(this.options.Topics.ToArray());
                this.subscriber.HandleMessages(cancellationToken, (CreateEmailRequest request) =>
                {
                    try
                    {
                        this.repository.Insert(new Email
                        {
                            Address = request.Email
                        });
                    }
                    catch (Npgsql.PostgresException exception)
                    {
                        // TODO: Notify user if the insert failed with 'duplicate key value violates unique constraint' error
                        this.ThrowHandleMessageException(exception, request.Email);
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException exception)
                    {
                        this.ThrowHandleMessageException(exception, request.Email);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                this.logger.LogWarning("Operation canceled");
                await this.StopAsync(cancellationToken);
            }
            catch (Exception exception)
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

        private void ThrowHandleMessageException(Exception innerException, string email)
        {
            throw new HandleMessageException($"FAILED inserting email '{email}' to the database", innerException);
        }
    }
}
