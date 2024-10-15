using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint
{
    public class AppointRegisteredPage(IServiceProvider services, AppointStorage appointStorage) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            return $"Спасибо. Вы записаны к {userState.UserData.AppointRegistration!}";
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {

            return [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("В Личный кабинет"), services.GetRequiredService<PersonalAccountPage>())
                    ]
                ];
        }
    }
}
