using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Basic;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount
{
    public class AppointRegisterTimePage(IServiceProvider services, AppointStorage appointStorage) : CallbackQueryPageBase
    {
        public override string GetText(UserState userState)
        {
            return Resources.AppointRegisterTimePageText;
        }

        public override ButtonLinqPage[][] GetKeyboard(UserState userState)
        {
            var buttons = new List<ButtonLinqPage[]>();
            var userDate = userState.UserData.AppointRegistration!.Date;
            var currentDate = DateTime.Now;
            var startHour = currentDate.Date == userDate.Date && currentDate.Hour >= 9? currentDate.Hour + 1 : 9;
            var endHour = 18;
            var bookedHours = appointStorage.GetAppoints(userState.UserData.Id)
                                            .Where(x => x.DocType == userState.UserData.selectedDocType && x.Date.Date == userDate.Date && x.DocName == userState.UserData.selectedDocName)
                                            .Select(x => x.Date.Hour);
                                                                                          
            for (int hour = startHour; hour <= endHour; hour++)
            {
                if (!bookedHours.Contains(hour))
                    buttons.Add(
                        [
                            new ButtonLinqPage(InlineKeyboardButton.WithCallbackData($"{hour:00}:00", $"time_{hour:00}:00"), services.GetRequiredService<AppointRegisteredPage>())
                        ]);
            }

            buttons.Add([new ButtonLinqPage(InlineKeyboardButton.WithCallbackData("НАЗАД"), services.GetRequiredService<BackwardDummyPage>())]);

            if (buttons.Count() == 1)
                userState.UserData.isAllTimeBooked = true; 

            return buttons.ToArray();
        }

        public override PageResult Handle(Update update, UserState userState)
        {
            if (update.CallbackQuery?.Data == null)
            {
                return View(update, userState);
            }

            if (update.CallbackQuery.Data == "НАЗАД")
            {
                userState.UserData.AppointRegistration = null;
                return services.GetRequiredService<BackwardDummyPage>().View(update, userState);
            }


            if (update.CallbackQuery.Data.StartsWith("time_"))
            {
                var selectedTime = update.CallbackQuery.Data.Split('_')[1];
                var time = TimeSpan.Parse(selectedTime);

                userState.UserData.AppointRegistration!.SetTime(time);
                appointStorage.AddAppoint(userState.UserData.AppointRegistration!);
                return services.GetRequiredService<AppointRegisteredPage>().View(update, userState);
            }    
            return base.Handle(update, userState);
        }
    }
}