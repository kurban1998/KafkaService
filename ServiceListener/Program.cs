using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ServiceListener;
using ServiceListener.Registers;

namespace KafkaListener
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IHostBuilder host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) =>
                {
                    var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(context.Configuration)
                    .CreateLogger();

                    services.AddLogging(l => l.AddSerilog(logger));
                    services.AddConsumer(context.Configuration);
                    services.AddSingleton<WorkerListener>();
                });
            
            var app = host.Build();
            await app.Services.GetRequiredService<WorkerListener>().Run(CancellationToken.None).ConfigureAwait(false);
        }
    }
}