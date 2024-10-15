using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Tests.AuthorizationPagesTests
{
    internal class ErrorAuthorizationPageTests
    {
        private IServiceProvider services;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();
            ContainerConfigurator.Configure(configuration, serviceCollection);            services = serviceCollection.BuildServiceProvider();

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
        public void View_ErrorAuthorizationPage_CorrectKeyboard()
        {
            // Arrange
            var errPage = services.GetRequiredService<ErrorAuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), errPage]);
            var userState = new UserState(pages, new UserData());

            var expectedButtons = new InlineKeyboardButton[][]
            {
                    [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var result = errPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(errPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_AuthorizationPage()
        {
            // Arrange
            var errPage = services.GetRequiredService<ErrorAuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), errPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = errPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<AuthorizationPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void Handle_PrevPageCallback_RegisterPage()
        {
            // Arrange
            var errPage = services.GetRequiredService<ErrorAuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<RegisterPage>(), errPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = errPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<RegisterPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void Handle_RegisterPageCallback_ErrorAuthorizationPageView()
        {
            // Arrange
            var errPage = services.GetRequiredService<ErrorAuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), services.GetRequiredService<RegisterPage>(), errPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = errPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<RegisterPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }
    }
}
