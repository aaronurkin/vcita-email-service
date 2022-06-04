using Confluent.Kafka;
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
        private readonly IConsumer<Ignore, TMessageValue> consumer;

        public ConfluentKafkaDefaultSubscriber(
            IConsumer<Ignore, TMessageValue> consumer,
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
                var consumeResult = default(ConsumeResult<Ignore, TMessageValue>);

                try
                {
                    consumeResult = this.consumer.Consume(cancellationToken);

                    if (consumeResult.Message?.Value == null)
                    {
                        this.consumer.Commit(consumeResult);
                        continue;
                    }
                }
                catch (ConsumeException exception)
                {
                    this.logger.LogWarning(exception, "FAILED consuming a message: {0}", exception.Error.Reason);
                }
                catch (OperationCanceledException exception)
                {
                    this.logger.LogError(exception, "Operation has been canceled");
                    this.Dispose();
                }

                yield return consumeResult.Message?.Value;
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
    }
}
