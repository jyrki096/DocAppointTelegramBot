using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main
{
    public class CallHandlerPage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            var isCorrectNumber = Regex.IsMatch(userState.UserData.SentMessage, @"^[78]\d{3}-\d{3}-\d{2}-\d{2}$");

            if (isCorrectNumber)
                return Resources.SuccessCallRequestText;

            return Resources.IncorrectUserNumberText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                    ]
                   ];
        }
    }
}
