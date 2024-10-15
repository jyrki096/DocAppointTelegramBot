using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults
{
    public class PageResult
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
        public bool IsMedia {  get; set; }
        public UserState UpdatedUserState { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Html;

        public PageResult(string text, IReplyMarkup replyMarkup)
        {
            Text = text;
            ReplyMarkup = replyMarkup;
        }
    }

}