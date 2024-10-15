using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Tests.AuthorizationPagesTests
{
    internal class RegisterPageTests
    {
        private IServiceProvider services;
        private UserStorage userStorage;
        private IRON_PROGRAMMER_BOT_Common.Models.User user;
        private string createdUserNumber;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
            userStorage = services.GetRequiredService<UserStorage>();

            user = new IRON_PROGRAMMER_BOT_Common.Models.User()
            {
                Name = "Test",
                PhoneNumber = "79995554433",
                Password = "qwerty123"
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            userStorage.DeleteUser(user.PhoneNumber);
            userStorage.DeleteUser(createdUserNumber);
            if (services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public void View_RegisterPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());

            var expectedButtons = new InlineKeyboardButton[][]
            {
                    [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var result = regPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(regPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.EqualTo(Resources.RegisterPageText));

            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_AuthorizationPage()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = regPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<AuthorizationPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void Handle_IncorrectUserNumber_RegisterPage()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"9658882255 Tester ddd343rfffd" } };

            // Act
            var result = regPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ErrorAuthorizationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(Resources.IncorrectUserNumberText));
        }

        [Test]
        public void Handle_IncorrectInputFormat_RegisterPage()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"79875665558" } };

            // Act
            var result = regPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ErrorAuthorizationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(Resources.IncorrectInputFormatText));
        }

        public void Handle_ExistedUser_RegisterPage()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"{user.PhoneNumber} {user.Password}" } };

            // Act
            var result = regPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ErrorAuthorizationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(Resources.UserExistErrorText));
        }

        [Test]
        public void Handle_SuccessRegister_PersonalAccountPage()
        {
            // Arrange
            var regPage = services.GetRequiredService<RegisterPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), regPage]);
            var userState = new UserState(pages, new UserData());
            var userName = "Tester";
            createdUserNumber = "79872223344";
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"{createdUserNumber} {userName} qwerty123" } };
            var personalAccountPageText = $"Добро пожаловать в личный кабинет {userName}!!!\nЖелаешь записаться на приём или что-то другое?\nВыбери кнопкой одно из действий";
            // Act
            var result = regPage.Handle(update, userState);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)), result.Text);
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<PersonalAccountPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(personalAccountPageText));
        }
    }
}
