using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot.Polling;
using Telegram.Bot;

public class LongPoolingConfigurator(ITelegramBotClient botClient, IUpdateHandler updateHandler) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var user = await botClient.GetMeAsync();
        Console.WriteLine($"Начали слушать апдейты с {user.Username}");

        await botClient.ReceiveAsync(updateHandler: updateHandler);
    }
}