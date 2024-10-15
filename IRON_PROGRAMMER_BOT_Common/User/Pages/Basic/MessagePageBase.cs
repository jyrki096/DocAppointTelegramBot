using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Basic
{
    public abstract class MessagePageBase : CallbackQueryPageBase
    {
        public abstract UserState ProcessMessage(Telegram.Bot.Types.Message message, UserState userState);

        public abstract IPage GetNextPage();

        public override PageResult Handle(Update update, UserState userState)
        {
            if (update.Message == null)
            {
                return base.Handle(update, userState);
            }

            var updatedUserState = ProcessMessage(update.Message, userState);
            var nextPage = GetNextPage();

            return nextPage.View(update, updatedUserState);
        }
    }
}
