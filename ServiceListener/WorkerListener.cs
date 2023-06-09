using Confluent.Kafka;
using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceListener
{
    public sealed class WorkerListener
    {
        public WorkerListener(
            IConsumer<string, string> consumer,
            ILogger<WorkerListener> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var message = _consumer.Consume(cancellationToken);

                _logger.LogInformation($"Message received. Consumer: {_consumer.Name}");

                var account = JsonConvert.DeserializeObject<Account>(message.Value);

                Console.WriteLine(
                    $"Id: {account.Id}," +
                    $" Name: {account.Name}," +
                    $" Date create: {account.CreateDate}");
            }
        }

        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<WorkerListener> _logger;
    }
}