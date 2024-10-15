using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Microsoft.Extensions.DependencyInjection;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint
{
    public class DoctorsNamePage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        { 
            return Resources.DoctorsNamePageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            switch(userState.UserData.selectedDocType)
            {
                case "Терапевт":
                    {
                        return 
                            [
                                [

                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Иванов И.И."), services.GetRequiredService<AppointRegisterDatePage>())
                                ],
                                [
                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                                ]
                            ];
                    }
                case "Окулист":
                    {
                        return
                            [
                                [

                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Петров В.В."), services.GetRequiredService<AppointRegisterDatePage>())
                                ],
                                [
                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                                ]
                            ];
                    }
                case "Хирург":
                    {
                        return
                            [
                                [

                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Алексеев А.А."), services.GetRequiredService<AppointRegisterDatePage>())
                                ],
                                [
                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                                ]
                            ];
                    }
                case "ЛОР":
                    {
                        return
                            [
                                [

                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Соколов И.И."), services.GetRequiredService<AppointRegisterDatePage>())
                                ],
                                [
                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                                ]
                            ];
                    }
                case "Стоматолог":
                    {
                        return
                            [
                                [

                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Пронин Н.А."), services.GetRequiredService<AppointRegisterDatePage>())
                                ],
                                [
                                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                                ]
                            ];
                    }
            }

            return
                [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())
                    ]
                ];
        }

        public override PageResult Handle(Update update, UserState userState)
        {
            if (update.CallbackQuery == null)
            {
                return View(update, userState);
            }

            userState.UserData.selectedDocName = update.CallbackQuery.Data!;
            return base.Handle(update, userState);
        }
    }
}