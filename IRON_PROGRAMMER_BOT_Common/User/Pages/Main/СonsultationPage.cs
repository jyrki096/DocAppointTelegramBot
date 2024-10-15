using Telegram.Bot.Types.ReplyMarkups;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main
{
    public class ConsultationPage(IServiceProvider services) : MessagePageBase
    {
        public override string GetText(UserState userState)
        {
            return Resources.ConsultationPageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                    ]
                   ];
        }
        public override UserState ProcessMessage(Telegram.Bot.Types.Message message, UserState userState)
        {
            userState.UserData.SentMessage = message.Text;
            return userState;
        }

        public override IPage GetNextPage()
        {
            return services.GetRequiredService<CallHandlerPage>();
        }
    }
}