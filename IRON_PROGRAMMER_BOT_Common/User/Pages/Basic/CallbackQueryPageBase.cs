using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.User.Pages.Basic
{
    public abstract class CallbackQueryPageBase : IPage
    {
        public abstract string GetText(UserState userState);

        public abstract ButtonLinqPage[][] GetKeyboard(UserState userState);

        public virtual PageResult View(Update update, UserState userState)
        {
            var replyMarkup = GetInlineKeyboardMarkup(userState);
            var text = GetText(userState);

            if (userState.UserData.isAllTimeBooked)
                text = "На эту дату не осталось записи.";

            userState.UserData.isAllTimeBooked = false;

            userState.AddPage(this);

            return new PageResult(text, replyMarkup)
            {
                UpdatedUserState = userState
            };
        }

        public virtual PageResult Handle(Update update, UserState userState)
        {
            if (update.CallbackQuery == null)
            {
                return View(update, userState);
            }

            if(update.CallbackQuery.Data == "В Личный кабинет")
            {   
                while(userState.Pages.Peek().GetType()  != typeof(PersonalAccountPage))
                {
                    userState.Pages.Pop();
                }
            }

            var buttons = GetKeyboard(userState).SelectMany(x => x);
            var pressedButton = buttons.FirstOrDefault(x => x.Button.CallbackData == update.CallbackQuery.Data)!;

            return pressedButton.Page.View(update, userState);
        }

        protected InlineKeyboardMarkup GetInlineKeyboardMarkup(UserState userState)
        {
            return new InlineKeyboardMarkup(GetKeyboard(userState).Select(page => page.Select(x => x.Button)));
        }
    }
}
