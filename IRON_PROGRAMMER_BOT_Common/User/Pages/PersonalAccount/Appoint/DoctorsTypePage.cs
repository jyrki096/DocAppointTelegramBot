using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint
{
    public class DoctorsTypePage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            return Resources.DoctorsTypePageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [

                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Терапевт"), services.GetRequiredService<DoctorsNamePage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Окулист"), services.GetRequiredService<DoctorsNamePage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Хирург"), services.GetRequiredService<DoctorsNamePage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Стоматолог"), services.GetRequiredService<DoctorsNamePage>())
                    ],
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                    ]
                ];
        }

        public override PageResult Handle(Telegram.Bot.Types.Update update, UserState userState)
        {
            userState.UserData.AppointRegistration = null;

            if (update.CallbackQuery == null)
            {
                return View(update, userState);
            }

            userState.UserData.selectedDocType = update.CallbackQuery.Data!;
            return base.Handle(update, userState);
        }
    }
}