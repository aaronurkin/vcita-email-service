using Confluent.Kafka;
using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Core31.Shared.Services.ConfluentKafka.Deserializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core31.Consumers.CreateEmail
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configuration =>
                {
                    configuration.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var confluentKafkaConsumerConfig = new ConsumerConfig
                    {
                        AllowAutoCreateTopics = true,
                        GroupId = context.Configuration["EmailsConsumerGroup"],
                        BootstrapServers = context.Configuration["KafkaServers"]
                    };

                    services
                        .AddSingleton(new CreateEmailRequestHandlerOptions
                        {
                            Topics = new[] { context.Configuration["KafkaTopic"]  }
                        });

                    services
                        .AddTransient<IConsumer<Ignore, CreateEmailRequest>>(provider =>
                            new ConsumerBuilder<Ignore, CreateEmailRequest>(confluentKafkaConsumerConfig)
                                .SetValueDeserializer(new JsonDeserializer<CreateEmailRequest>())
                                .Build());

                    services
                        .AddTransient<ISubscriber<CreateEmailRequest>, ConfluentKafkaDefaultSubscriber<CreateEmailRequest>>();

                    services.AddHostedService<CreateEmailRequestHandler>();
                });
    }
}
