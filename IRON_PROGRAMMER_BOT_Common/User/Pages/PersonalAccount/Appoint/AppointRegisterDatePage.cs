using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint
{
    public class AppointRegisterDatePage(IServiceProvider services) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            return Resources.AppointRegisterDatePageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            if (userState.UserData.AppointRegistration is null)
            {
                userState.UserData.AppointRegistration = new Models.Appoint()
                {
                    UserId = userState.UserData.Id,
                    Name = userState.UserData.PhoneNumber,
                    DocName = userState.UserData.selectedDocName,
                    DocType = userState.UserData.selectedDocType,
                    Date = DateTime.Now
                };
            }
            

            var currentDate = userState.UserData.AppointRegistration.Date;
            var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            var monthFirstDay = currentDate.Month == DateTime.Now.Month ? currentDate.Day : 1;

            var buttons = new List<ButtonLinqPage[]>();

            if (currentDate.Month == DateTime.Now.Month)
            {
                buttons.Add(
                [
                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData(currentDate.ToString("Y", CultureInfo.GetCultureInfo("ru-RU")), "no_action"), this),
                    new ButtonLinqPage(InlineKeyboardButton.WithCallbackData(">"), this)
                ]);
            }
            else
            {
                buttons.Add(
                    [
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("<"), this),
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData(currentDate.ToString("Y", CultureInfo.GetCultureInfo("ru-RU")), "no_action"), this),
                        new ButtonLinqPage(InlineKeyboardButton.WithCallbackData(">"), this)
                    ]);
            }

            var week = new List<ButtonLinqPage>();

            for (int day = monthFirstDay; day <= daysInMonth; day++)
            {
                var date = new DateTime(currentDate.Year, currentDate.Month, day);

                week.Add(new ButtonLinqPage(InlineKeyboardButton.WithCallbackData(day.ToString(), $"date_{date:yyyy-MM-dd}"), this));

                if (week.Count == 7 || day == daysInMonth)
                {
                    buttons.Add(week.ToArray());
                    week = new List<ButtonLinqPage>();
                }
            }

            buttons.Add([new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())]);

            return buttons.ToArray();
        }

        public override PageResult Handle(Update update, UserState userState)
        {
            if (update.CallbackQuery == null || update.CallbackQuery.Data == "no_action")
            {
                return View(update, userState);
            }

            switch (update.CallbackQuery.Data)
            {
                case ">":
                    {
                        userState.UserData.AppointRegistration!.NextMonth();
                        return View(update, userState);
                    }
                case "<":
                    {
                        userState.UserData.AppointRegistration!.PreviousMonth();
                        return View(update, userState);
                    }
                case string { Length: > 0 } s when s.StartsWith("date_"):
                    {
                        var selectedDate = update.CallbackQuery.Data.Split('_')[1];
                        userState.UserData.AppointRegistration!.Date = DateTime.Parse(selectedDate);
                        return services.GetRequiredService<PersonalAccount.AppointRegisterTimePage>().View(update, userState);
                    }
            }

            return base.Handle(update, userState);
        }

    }
}
