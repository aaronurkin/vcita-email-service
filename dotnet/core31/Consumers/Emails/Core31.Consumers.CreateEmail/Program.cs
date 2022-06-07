﻿using Confluent.Kafka;
using Core31.Consumers.CreateEmail.Data;
using Core31.Consumers.CreateEmail.Data.EntityFramework;
using Core31.EventSubscribers.Emails.Data;
using Core31.EventSubscribers.Emails.Data.EntityFramework;
using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Core31.Shared.Services.ConfluentKafka.Deserializers;
using Microsoft.EntityFrameworkCore;
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
                        .AddTransient<IConsumer<Null, CreateEmailRequest>>(provider =>
                            new ConsumerBuilder<Null, CreateEmailRequest>(confluentKafkaConsumerConfig)
                                .SetValueDeserializer(new JsonDeserializer<CreateEmailRequest>())
                                .Build());

                    services
                        .AddTransient<ISubscriber<CreateEmailRequest>, ConfluentKafkaDefaultSubscriber<CreateEmailRequest>>();

                    services.AddDbContext<EmailsDbContext>(config =>
                        config.UseNpgsql(context.Configuration.GetConnectionString("EmailsMs")));

                    services
                        .AddTransient<IEmailsRepository, EntityFrameworkEmailsRepository>();

                    services.AddHostedService<CreateEmailRequestHandler>();
                });
    }
}
