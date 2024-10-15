using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;

namespace IRON_PROGRAMMER_BOT_Tests.AuthorizationPagesTests
{
    internal class AuthorizationPageTests
    {
        private IServiceProvider services;
        private UserStorage userStorage;
        private IRON_PROGRAMMER_BOT_Common.Models.User user;

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

            userStorage.SaveUser(user);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            userStorage.DeleteUser(user.PhoneNumber);

            if (services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public void View_AuthorizationPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());

            var expectedButtons = new InlineKeyboardButton[][]
            {
                    [InlineKeyboardButton.WithCallbackData("Зарегистрироваться")],
                    [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var result = authPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(authPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));

            Assert.That(result.Text, Is.EqualTo(Resources.AuthorizationPageText));

            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_StartPage()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = authPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));
           
            Assert.IsInstanceOf<StartPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
        }
        [Test]
        public void Handle_IncorrectAuthData_AuthorizationPage()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"79658882255 ddd343rfffd" } };

            // Act
            var result = authPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ErrorAuthorizationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(Resources.IncorrectUserAuthorizationDataText));
        }

        [Test]
        public void Handle_IncorrectDataFormat_AuthorizationPage()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"79875665558" } };

            // Act
            var result = authPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ErrorAuthorizationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(Resources.IncorrectInputFormatText));
        }

        [Test]
        public void Handle_RegisterCallbackQuery_RegisterPage()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Зарегистрироваться" } };
            // Act
            var result = authPage.Handle(update, userState);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<RegisterPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(Resources.RegisterPageText));
        }

        [Test]
        public void Handle_UserCorrectInput_PersonalAccountPage()
        {
            // Arrange
            var authPage = services.GetRequiredService<AuthorizationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), authPage]);
            var userState = new UserState(pages, new UserData());
            var personalAccountPageText = $"Добро пожаловать в личный кабинет {user.Name}!!!\nЖелаешь записаться на приём или что-то другое?\nВыбери кнопкой одно из действий";
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"{user.PhoneNumber} {user.Password}" } }; 

            // Act
            var result = authPage.Handle(update, userState);      
            // Assert
            
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)), $"{result.UpdatedUserState.UserData.SentMessage}");
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<PersonalAccountPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(personalAccountPageText));
        }
    }
}
