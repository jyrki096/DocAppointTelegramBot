using System.Text.RegularExpressions;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.Helpers;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization
{
    public class RegisterPage(UserStorage userStorage, IServiceProvider services) : MessagePageBase
    {
        private bool isCorrectUserData = false;
        public override UserState ProcessMessage(Telegram.Bot.Types.Message message, UserState userState)
        {
            isCorrectUserData = ValidateRegistration(message.Text, userState);
            return userState;
        }

        public override IPage GetNextPage()
        {
            if (isCorrectUserData)
                return services.GetRequiredService<PersonalAccountPage>();  

            return services.GetRequiredService<ErrorAuthorizationPage>();
        }

        public override string GetText(UserState userState)
        {
            return Resources.RegisterPageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                     [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                     ]
                   ];
        }

        public bool ValidateRegistration(string message, UserState userState)
        {
            var userData = message.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (userData.Length < 3)
            {
                userState.UserData.SentMessage = Resources.IncorrectInputFormatText;
                return false;
            }
              
            var isCorrectNumber = Regex.IsMatch(userData[0], @"^[78]\d{10}$");

            if (isCorrectNumber)
            {

                var isExists = userStorage.Exists(userData[0]);

                if (isExists)
                {
                    userState.UserData.SentMessage = Resources.UserExistErrorText;
                    return false;
                }

                var user = new Models.User()
                {
                    Name = userData[1],
                    PhoneNumber = userData[0],
                    Password = userData[2]
                };

                userStorage.SaveUser(user);
                var userId = userStorage.GetUser(userData[0], userData[2])!.Id;
                user.Id = userId;
                Mapper.AuthorizeUser(userState, user);

                return true;
            }
            userState.UserData.SentMessage = Resources.IncorrectUserNumberText;

            return false;
        }
    }
}
