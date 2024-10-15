using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main
{
    public class StartPage(IServiceProvider services, ResourcesService resourcesService) : CallbackQueryPhotoPageBase(resourcesService)
    {
        public override byte[] GetPhoto()
        {
            return Resources.StartPagePhoto;
        }

        public override string GetText(UserState userState)
        {
            return Resources.StartPageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [
                       new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Авторизация") , services.GetRequiredService<AuthorizationPage>())
                    ],
                    [
                       new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Запросить консультацию") , services.GetRequiredService<ConsultationPage>())
                    ],
                    [
                       new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Наши специалисты") , services.GetRequiredService<SpecialistsPage>())
                    ],
                    [
                       new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Контакты") , services.GetRequiredService<ContactsPage>())
                    ],
                    [
                       new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Отзывы") , services.GetRequiredService<ReviewsPage>())
                    ],
                ];
        }
    }
}
