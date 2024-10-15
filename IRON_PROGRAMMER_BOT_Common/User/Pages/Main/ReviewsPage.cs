using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Main
{
    public class ReviewsPage(ReviewStorage reviewStorage, IServiceProvider services) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            var text = Resources.ReviewsPageText;

            var reviews = reviewStorage.GetReviews();

            if (reviews is not null && reviews.Count > 0)
            {
                var data = new List<string>();

                foreach (var review in reviews)
                {
                    data.Add($"<b>{review.User.Name} {review.Date.ToString("dd.MM.yyyy")}</b>\n<i>{review.Text}</i>");
                }

                text = string.Join("\n\n", data);
            }

            return text;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            return [
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД") , services.GetRequiredService<BackwardDummyPage>())
                    ]
                   ];
        }
    }
}