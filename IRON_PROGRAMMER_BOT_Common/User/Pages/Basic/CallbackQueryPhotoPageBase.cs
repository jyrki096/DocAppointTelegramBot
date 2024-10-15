using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_ConsoleApp.Services;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Basic
{
    public abstract class CallbackQueryPhotoPageBase(ResourcesService resourcesService) : CallbackQueryPageBase
    {
        public abstract byte[] GetPhoto();

        public override PageResult View(Update update, UserState userState)
        {
            var text = GetText(userState);
            var keyboard = GetInlineKeyboardMarkup(userState);
            var photo = resourcesService.GetResource(GetPhoto());

            userState.AddPage(this);
            return new PhotoPageResult(photo, text, keyboard)
            {
                UpdatedUserState = userState
            };
        }
    }
}
