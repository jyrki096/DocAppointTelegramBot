using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests
{
    internal class UserAppointPageTests
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
        public void View_UserAppointPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var userAppointPage = services.GetRequiredService<UserAppointPage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(), 
                    services.GetRequiredService<StartPage>(), 
                    services.GetRequiredService<AuthorizationPage>(), 
                    services.GetRequiredService<PersonalAccountPage>(),
                    userAppointPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = Resources.UserAppointPageText;

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            // Act
            var result = userAppointPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(userAppointPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_PersonalAccountPage()
        {
            // Arrange
            var userAppointPage = services.GetRequiredService<UserAppointPage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    userAppointPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = userAppointPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<PersonalAccountPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }


        [Test]
        public void Handle_UnknowMessage_UserAppointPageView()
        {
            // Arrange
            var userAppointPage = services.GetRequiredService<UserAppointPage>();

            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    userAppointPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = Resources.UserAppointPageText;
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"ededef34343" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            // Act
            var result = userAppointPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(userAppointPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
