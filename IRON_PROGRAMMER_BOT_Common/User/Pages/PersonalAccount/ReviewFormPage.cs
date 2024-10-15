using IRON_PROGRAMMER_BOT_Common.Models;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount
{
    public class ReviewFormPage(ReviewStorage reviewStorage, IServiceProvider services) : MessagePageBase
    {
        public override UserState ProcessMessage(Telegram.Bot.Types.Message message, UserState userState)
        {
            var review = new Review()
            {
                UserId = userState.UserData.Id,
                Text = message.Text,
                Date = DateTime.UtcNow
            };

            reviewStorage.SaveReview(review);
            return userState;
        }

        public override IPage GetNextPage()
        {
            return services.GetRequiredService<SuccessReviewFormPage>();
        }

        public override string GetText(UserState userState)
        {
            return Resources.ReviewFormPageText;
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