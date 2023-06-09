using Common.Contracts;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ServiceListener.Registers
{
    internal static class ConsumerRegister
    {
        public static void AddConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var server = configuration.GetSection("KafkaServer").Value;

            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = server,
                GroupId = KafkaSettings.GroupId
            };

            var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig)
                .SetErrorHandler((consumer, er) =>
                {
                    Console.WriteLine($"Failed producer {consumer.Name}: Error: {er.Reason}");
                });
        
            var consumer = consumerBuilder.Build();

            services.AddSingleton(consumer);
        }
    }
}