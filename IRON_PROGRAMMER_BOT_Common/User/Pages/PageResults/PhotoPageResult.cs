using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults
{
    public class PhotoPageResult : PageResult
    {
        public InputFile Photo { get; set; }
        public PhotoPageResult(InputFile photo, string text, IReplyMarkup replyMarkupBase) : base(text, replyMarkupBase)
        {
            Photo = photo;
        }
    }

}