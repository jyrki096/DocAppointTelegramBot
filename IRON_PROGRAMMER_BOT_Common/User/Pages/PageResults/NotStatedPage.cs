using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults
{
    public class NotStatedPage(IServiceProvider services) : IPage
    {
        public PageResult Handle(Update update, UserState userState)
        {
            return new StartPage(services, services.GetRequiredService<ResourcesService>()).View(update, userState);
        }

        public PageResult View(Update update, UserState userState)
        {
            return null;
        }
    }
}
