using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicePublisher;
using ServicePublisher.Registars;

namespace KafkaSender
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            IHostBuilder builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddProducer(context.Configuration);
                    services.AddSingleton<PublishWorker>();
                });

            var app = builder.Build();
            await app.Services.GetRequiredService<PublishWorker>().Run(token).ConfigureAwait(false);
        }
    }
}