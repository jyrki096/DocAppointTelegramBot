using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization
{
    public class ErrorAuthorizationPage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                     [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                     ]
                   ];
        }

        public override string GetText(UserState userState)
        {
            return userState.UserData.SentMessage;
        }
    }
}
