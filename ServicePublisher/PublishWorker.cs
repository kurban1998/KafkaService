using Common.Contracts;

using Confluent.Kafka;

namespace ServicePublisher
{
    internal sealed class PublishWorker
    {
        public PublishWorker(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(KafkaTopicNames.TopicName, CreateMessage(), cancellationToken).ConfigureAwait(false);
            await Task.Delay(WaitTimeWorker.WaitTime);
        }

        private Message<string, string> CreateMessage()
        {
            return new Message<string, string>();
        }

        private readonly IProducer<string, string> _producer;
    }
}
