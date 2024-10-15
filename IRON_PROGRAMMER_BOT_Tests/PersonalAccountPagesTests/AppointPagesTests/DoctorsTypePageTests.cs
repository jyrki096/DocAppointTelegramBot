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
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;


namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests.AppointPagesTests
{
    internal class DoctorsTypePageTests
    {
        private IServiceProvider services;
        private InlineKeyboardButton[][] expectedButtons = 
            {

                    [

                        InlineKeyboardButton.WithCallbackData("Терапевт")
                    ],
                    [
                        InlineKeyboardButton.WithCallbackData("Окулист")
                    ],
                    [
                        InlineKeyboardButton.WithCallbackData("Хирург")
                    ],
                    [
                        InlineKeyboardButton.WithCallbackData("Стоматолог")
                    ],
                    [
                        InlineKeyboardButton.WithCallbackData("НАЗАД")
                    ]
             };

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
        public void View_DoctorsTypePage_CorrectTextAndKeyboard()
        {
            // Arrange
            var docTypePage = services.GetRequiredService<DoctorsTypePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    docTypePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = Resources.DoctorsTypePageText;

            // Act
            var result = docTypePage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(docTypePage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_SelectedTypeCallback_DoctorsNamePage()
        {
            // Arrange
            var docTypePage = services.GetRequiredService<DoctorsTypePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    docTypePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Терапевт" } };

            // Act
            var result = docTypePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<DoctorsNamePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(6));
        }


        [Test]
        public void Handle_PrevPageCallback_PersonalAccountPage()
        {
            // Arrange
            var docTypePage = services.GetRequiredService<DoctorsTypePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    docTypePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = docTypePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<PersonalAccountPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }


        [Test]
        public void Handle_UnknowMessage_DoctorsTypePageView()
        {
            // Arrange
            var docTypePage = services.GetRequiredService<DoctorsTypePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    docTypePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = Resources.DoctorsTypePageText;
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "ededef34343" } };

            // Act
            var result = docTypePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(docTypePage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
