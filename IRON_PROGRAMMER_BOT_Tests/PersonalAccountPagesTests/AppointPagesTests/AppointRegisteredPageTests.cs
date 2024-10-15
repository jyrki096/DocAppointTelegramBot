using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;


namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests.AppointPagesTests
{
    internal class AppointRegisteredPageTests
    {
        private IServiceProvider services;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public void View_AppointRegisteredPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var appointRegisteredPage = services.GetRequiredService<AppointRegisteredPage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    services.GetRequiredService<AppointRegisterTimePage>(),
                    appointRegisteredPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = 1,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Parse(DateTime.Now.ToString("u").Split().First()),
            };
            userState.UserData.AppointRegistration.SetTime(TimeSpan.Parse($"{DateTime.Now.AddHours(1).Hour}:00"));

            var text = $"Спасибо. Вы записаны к {userState.UserData.AppointRegistration!}";
            var expectedButtons = new InlineKeyboardButton[][]
            {
                    [InlineKeyboardButton.WithCallbackData("В Личный кабинет")],
            };
            // Act
            var result = appointRegisteredPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointRegisteredPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(9));
            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PersonalAccountCallback_PersonalAccountPage()
        {
            var appointRegisteredPage = services.GetRequiredService<AppointRegisteredPage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    services.GetRequiredService<AppointRegisterTimePage>(),
                    appointRegisteredPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "В Личный кабинет" } };

            // Act
            var result = appointRegisteredPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<PersonalAccountPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }

        [Test]
        public void Handle_UnknowMessage_AppointRegisterTimePageView()
        {
            // Arrange
            var appointRegisteredPage = services.GetRequiredService<AppointRegisteredPage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    services.GetRequiredService<AppointRegisterTimePage>(),
                    appointRegisteredPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = 1,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Parse(DateTime.Now.ToString("u").Split().First()),
            };
            userState.UserData.AppointRegistration.SetTime(TimeSpan.Parse($"{DateTime.Now.AddHours(1).Hour}:00"));

            var text = $"Спасибо. Вы записаны к {userState.UserData.AppointRegistration!}";

            var expectedButtons = new InlineKeyboardButton[][]
            {
                    [InlineKeyboardButton.WithCallbackData("В Личный кабинет")],
            };

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "ededef34343" } };

            // Act
            var result = appointRegisteredPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointRegisteredPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(9));
            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
