using Common.Contracts;
using DataAccess;
using DataAccess.Models;

using Confluent.Kafka;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace ServicePublisher
{
    internal sealed class PublishWorker
    {
        public PublishWorker(
            IProducer<string, string> producer,
            MyDbContext context,
            ILogger<PublishWorker> logger)
        {
            _producer = producer;
            _context = context;
            _logger = logger;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_context.Accounts.Any()) 
                {
                    _logger.LogInformation("No items in the database");
                    return;
                }

                foreach (var account in _context.Accounts.ToList())
                {
                    try
                    {
                        await _producer.ProduceAsync(KafkaSettings.TopicName, CreateMessage(account), cancellationToken).ConfigureAwait(false);
                    }
                    catch
                    {
                        _logger.LogError($"There was an error while sending: {account.Id}");
                    }
                    
                    await Task.Delay(WaitTimeWorker.WaitTime);
                }
            }
        }

        private Message<string, string> CreateMessage(Account account)
        {
            var json = JsonConvert.SerializeObject(account);

            return new Message<string, string>()
            {
                Key = account.Id.ToString(),
                Value = json
            };
        }

        private readonly MyDbContext _context;
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<PublishWorker> _logger;
    }
}
