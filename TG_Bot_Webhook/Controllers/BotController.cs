using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot;
using IRON_PROGRAMMER_BOT_Common.Configuration;

namespace TG_Bot_Webhook.Controllers
{
    [ApiController]
    public class BotController(IUpdateHandler updateHandler, ITelegramBotClient botClient) : Controller
    {
        [HttpPost(BotConfiguration.UpdateRoute)]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            try
            {
                await updateHandler.HandleUpdateAsync(botClient, update, CancellationToken.None);
            }
            catch (Exception ex)
            {
                await updateHandler.HandlePollingErrorAsync(botClient, ex, CancellationToken.None);
            }

            return Ok();
        }

        [HttpGet("Ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok("Ok");
        }
    }
}
