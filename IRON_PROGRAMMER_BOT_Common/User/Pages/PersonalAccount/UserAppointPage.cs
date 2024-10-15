using Telegram.Bot.Types.ReplyMarkups;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using IRON_PROGRAMMER_BOT_Common.Storage;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount
{
    public class UserAppointPage(IServiceProvider services, AppointStorage appointStorage) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            var appoints = appointStorage.GetAppoints(userState.UserData.Id);

            if (appoints is not null && appoints.Count() > 0)
                return string.Join("\n\n", appoints);

            return "Записи на приём отсутствуют";
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                    ]
                   ];
        }
    }
}