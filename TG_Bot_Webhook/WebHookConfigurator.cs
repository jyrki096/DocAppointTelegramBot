using IRON_PROGRAMMER_BOT_Common.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace TG_Bot_Webhook
{
    public class WebHookConfigurator(ITelegramBotClient botClient, IOptions<BotConfiguration> botConfiguration) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookAddress = botConfiguration.Value.HostAddress + BotConfiguration.UpdateRoute;

            await botClient.SetWebhookAsync(url: webhookAddress,
                                            secretToken: botConfiguration.Value.SecretToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await botClient.DeleteWebhookAsync();
        }
    }
}
