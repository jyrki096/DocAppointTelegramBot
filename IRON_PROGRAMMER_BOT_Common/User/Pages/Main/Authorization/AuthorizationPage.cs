using IRON_PROGRAMMER_BOT_Common.Helpers;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization
{
    public class AuthorizationPage(UserStorage userStorage, IServiceProvider services) : MessagePageBase
    {
        private bool isCorrectUser = false;
        public override UserState ProcessMessage(Telegram.Bot.Types.Message message, UserState userState)
        {
            ValidateUser(message.Text!, userState);
            return userState;           
        }

        public override IPage GetNextPage()
        {
            if (isCorrectUser)
                return services.GetRequiredService<PersonalAccountPage>();

            return services.GetRequiredService<ErrorAuthorizationPage>();
        }

        public override string GetText(UserState userState)
        {
            return Resources.AuthorizationPageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                     [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("Зарегистрироваться") , services.GetRequiredService<RegisterPage>())
                     ],
                     [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                     ]
                   ];
        }

        public bool ValidateUser(string userData, UserState userState)
        {
            var logPass = userData.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (logPass.Length != 2)
            {
                userState.UserData.SentMessage = Resources.IncorrectInputFormatText;
                return false;   
            }

            var user = userStorage.GetUser(logPass[0], logPass[1]);

            if (user is null)
            {
                userState.UserData.SentMessage = Resources.IncorrectUserAuthorizationDataText;
                return false;
            }

            isCorrectUser = true;
            Mapper.AuthorizeUser(userState, user);

            return true;
        }
    }
}