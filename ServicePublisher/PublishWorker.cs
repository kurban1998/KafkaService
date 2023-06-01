using Common.Contracts;
using DataAccess;
using DataAccess.Models;

using Confluent.Kafka;
using Newtonsoft.Json;

namespace ServicePublisher
{
    internal sealed class PublishWorker
    {
        public PublishWorker(
            IProducer<string, string> producer,
            MyDbContext context)
        {
            _producer = producer;
            _context = context;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: добавить логирование
                if (!_context.Accounts.Any()) return;

                foreach (var account in _context.Accounts.ToList())
                {
                    await _producer.ProduceAsync(KafkaTopicNames.TopicName, CreateMessage(account), cancellationToken).ConfigureAwait(false);
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
    }
}
