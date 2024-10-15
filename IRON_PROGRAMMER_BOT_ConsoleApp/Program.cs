using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
        {
            ContainerConfigurator.Configure(context.Configuration, services);

            services.AddHostedService<LongPoolingConfigurator>();

        }).Build();

        await host.RunAsync();
    }
}
