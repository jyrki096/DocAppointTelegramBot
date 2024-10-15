using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;
using IRON_PROGRAMMER_BOT_ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount
{
    public class PersonalAccountPage(IServiceProvider services, ResourcesService resourcesService) : CallbackQueryPhotoPageBase(resourcesService)
    {
        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [

                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Записаться на приём"), services.GetRequiredService<DoctorsTypePage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Мои записи"), services.GetRequiredService<UserAppointPage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Оставить отзыв"), services.GetRequiredService<ReviewFormPage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                    ]
                ];
        }

        public override byte[] GetPhoto()
        {
            return Resources.PersonalAccountPagePhoto;
        }

        public override string GetText(UserState userState)
        {
            return $"Добро пожаловать в личный кабинет {userState.UserData.Name}!!!\nЖелаешь записаться на приём или что-то другое?\nВыбери кнопкой одно из действий";
        }
    }
}