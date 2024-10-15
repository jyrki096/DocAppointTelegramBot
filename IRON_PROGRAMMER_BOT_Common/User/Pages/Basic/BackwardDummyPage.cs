using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Telegram.Bot.Types;


namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Basic
{
    public class BackwardDummyPage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override PageResult View(Update update, UserState userState)
        {
            userState.Pages.Pop();
            return userState.CurrentPage.View(update, userState);
        }

        public override string GetText(UserState userState)
        {
            throw new NotImplementedException();
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            throw new NotImplementedException();
        }
    }
}
