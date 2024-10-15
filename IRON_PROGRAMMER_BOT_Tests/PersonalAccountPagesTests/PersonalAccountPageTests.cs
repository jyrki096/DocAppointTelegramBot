using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;


namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests
{
    internal class PersonalAccountPageTests
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
        public void View_PersonalAccountPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test"});
            var text = $"Добро пожаловать в личный кабинет {userState.UserData.Name}!!!\nЖелаешь записаться на приём или что-то другое?\nВыбери кнопкой одно из действий";
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Записаться на приём")],
                [InlineKeyboardButton.WithCallbackData("Мои записи")],
                [InlineKeyboardButton.WithCallbackData("Оставить отзыв")],
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            // Act
            var result = accountPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(accountPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_EnrollCallback_DoctorsTypePage()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Записаться на приём" } };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<DoctorsTypePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
        }

        [Test]
        public void Handle_AppointsCallback_UserAppointPage()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Мои записи" } };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<UserAppointPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
        }

        [Test]
        public void Handle_LeftReviewCallback_ReviewFormPage()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Оставить отзыв" } };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.IsInstanceOf<ReviewFormPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
        }

        [Test]
        public void Handle_PrevPageCallback_AuthorizationPage()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<AuthorizationPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }

        [Test]
        public void Handle_PrevPageCallback_RegisterPage()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), services.GetRequiredService<RegisterPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<RegisterPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }


        [Test]
        public void Handle_UnknowMessage_PersonalAccountPageView()
        {
            // Arrange
            var accountPage = services.GetRequiredService<PersonalAccountPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<AuthorizationPage>(), accountPage]);
            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = $"Добро пожаловать в личный кабинет {userState.UserData.Name}!!!\nЖелаешь записаться на приём или что-то другое?\nВыбери кнопкой одно из действий";
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = $"ededef34343" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Записаться на приём")],
                [InlineKeyboardButton.WithCallbackData("Мои записи")],
                [InlineKeyboardButton.WithCallbackData("Оставить отзыв")],
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            // Act
            var result = accountPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(accountPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
