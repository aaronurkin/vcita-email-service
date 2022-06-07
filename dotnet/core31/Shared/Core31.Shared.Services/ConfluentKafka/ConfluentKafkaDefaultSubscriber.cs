using Confluent.Kafka;
using Core31.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Core31.Shared.Services
{
    public class ConfluentKafkaDefaultSubscriber<TMessageValue> : ISubscriber<TMessageValue>
        where TMessageValue : class
    {
        private readonly ILogger logger;
        private readonly IConsumer<Null, TMessageValue> consumer;

        public ConfluentKafkaDefaultSubscriber(
            IConsumer<Null, TMessageValue> consumer,
            ILogger<ConfluentKafkaDefaultSubscriber<TMessageValue>> logger
        )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        public void Subscribe(params string[] topics)
        {
            try
            {
                this.consumer.Subscribe(topics);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "FAILED {0}", this);
                this.Dispose();
            }
        }

        public virtual IEnumerable<TMessageValue> GetMessages(CancellationToken cancellationToken)
        {
            while (true)
            {
                var value = this.HandleMessage(cancellationToken);

                if (value == null)
                {
                    continue;
                }

                yield return value;
            }
        }

        public virtual void HandleMessages(CancellationToken cancellationToken, Action<TMessageValue> handleMessage)
        {
            while (true)
            {
                this.HandleMessage(cancellationToken, handleMessage);
            }
        }

        public void Dispose()
        {
            if (this.consumer != null)
            {
                this.logger.LogDebug("Disposing subscriber...");
                this.consumer.Close();
            }
        }

        private TMessageValue HandleMessage(CancellationToken cancellationToken, Action<TMessageValue> handleMessage = null)
        {
            var consumeResult = default(ConsumeResult<Null, TMessageValue>);

            try
            {
                consumeResult = this.consumer.Consume(cancellationToken);

                if (consumeResult.Message?.Value == null)
                {
                    this.consumer.Commit(consumeResult);
                    return default(TMessageValue);
                }

                handleMessage?.Invoke(consumeResult.Message.Value);
            }
            catch (ConsumeException exception)
            {
                this.logger.LogWarning(exception, "FAILED consuming a message: {0}", exception.Error.Reason);
            }
            catch (HandleMessageException exception)
            {
                this.logger.LogWarning(exception, "FAILED handling a message");

                if (consumeResult != null)
                {
                    this.consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException exception)
            {
                this.logger.LogError(exception, "Operation has been canceled");
                this.Dispose();
            }

            return consumeResult.Message?.Value;
        }
    }
}
