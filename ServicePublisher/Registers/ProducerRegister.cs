using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServicePublisher.Registars
{
    internal static class ProducerRegister
    {
        public static void AddProducer(this IServiceCollection services, IConfiguration configuration)
        {
            var server = configuration.GetSection("KafkaServer").Value;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = server,
                ClientId = Guid.NewGuid().ToString()
            };

            var producerBuilder = new ProducerBuilder<string, string>(producerConfig)
            .SetErrorHandler((producer, er) =>
            {
                Console.WriteLine($"Failed producer {producer.Name}: Error: {er.Reason}");
            });

            var producer = producerBuilder.Build();

            services.AddSingleton(producer);
        }
    }
}