﻿using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Core31.Shared.Services
{
    public class ConfluentKafkaDefaultPublisher<TMessageValue> : IPublisher<TMessageValue>
        where TMessageValue : class
    {
        private readonly ILogger logger;
        private readonly IProducer<Null, TMessageValue> producer;

        public ConfluentKafkaDefaultPublisher(
            IProducer<Null, TMessageValue> producer,
            ILogger<ConfluentKafkaDefaultPublisher<TMessageValue>> logger
        )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public async Task Publish(TMessageValue value, string topic)
        {
            try
            {
                await this.producer.ProduceAsync(topic, new Message<Null, TMessageValue> { Value = value });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "FAILED publishing a message: {0}", JsonConvert.SerializeObject(value));
            }
        }

        public void Dispose()
        {
            if (this.producer != null)
            {
                this.logger.LogDebug("Disposing publisher...");
                this.producer.Dispose(); ;
            }
        }
    }
}