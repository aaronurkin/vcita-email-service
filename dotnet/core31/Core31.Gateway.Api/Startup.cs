using Confluent.Kafka;
using Core31.Shared.Models.Requests;
using Core31.Shared.Services;
using Core31.Shared.Services.ConfluentKafka.Serializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Core31.Gateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var confluentKafkaProducerConfig = new ProducerConfig
            {
                ClientId = Dns.GetHostName(),
                BootstrapServers = this.Configuration["KafkaServers"]
            };

            services
                .AddSingleton(new EmailServiceOptions
                {
                    CreateEmailTopic = this.Configuration["KafkaTopics:CreateEmail"]
                });

            services
                .AddTransient<IProducer<Null, CreateEmailRequest>>(provider =>
                    new ProducerBuilder<Null, CreateEmailRequest>(confluentKafkaProducerConfig)
                        .SetValueSerializer(new JsonSerializer<CreateEmailRequest>())
                        .Build()
                )
                .AddTransient<IPublisher<CreateEmailRequest>, ConfluentKafkaDefaultPublisher<CreateEmailRequest>>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
