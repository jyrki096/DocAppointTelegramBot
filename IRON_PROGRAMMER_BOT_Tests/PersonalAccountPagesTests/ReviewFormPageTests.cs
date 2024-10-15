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
using IRON_PROGRAMMER_BOT_Common.Storage;

namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests
{
    internal class ReviewFormPageTests
    {
        private IServiceProvider services;
        private UserStorage userStorage;
        private IRON_PROGRAMMER_BOT_Common.Models.User user;
        private ReviewStorage reviewStorage;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
            userStorage = services.GetRequiredService<UserStorage>();
            reviewStorage = services.GetRequiredService<ReviewStorage>();
            

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
        public void View_ReviewFormPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var reviewFormPage = services.GetRequiredService<ReviewFormPage>();

            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    reviewFormPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var text = Resources.ReviewFormPageText;

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            // Act
            var result = reviewFormPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(reviewFormPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_ReviewMessage_SuccessReviewFormPage()
        {
            // Arrange
            var reviewFormPage = services.GetRequiredService<ReviewFormPage>();

            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    reviewFormPage
                ]);

            var userState = new UserState(pages, new UserData() { Id = user.Id, PhoneNumber = user.PhoneNumber, Name = user.Name });
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "Good clinic" } };

            // Act
            var result = reviewFormPage.Handle(update, userState);
            var review = reviewStorage.GetReviews().Last();
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(update.Message.Text, Is.EqualTo(review.Text));
            Assert.IsInstanceOf<SuccessReviewFormPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(6));
        }

        [Test]
        public void Handle_PrevPageCallback_PersonalAccountPage()
        {
            // Arrange
            var reviewFormPage = services.GetRequiredService<ReviewFormPage>();

            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    reviewFormPage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = reviewFormPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<PersonalAccountPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }
    }
}
