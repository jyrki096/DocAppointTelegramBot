using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Tests.MainPagesTests
{
    internal class ReviewsPageTests
    {
        private IServiceProvider services;
        private ReviewStorage reviewStorage;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
            reviewStorage = services.GetRequiredService<ReviewStorage>();

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
        public void View_ReviewsPage_CorrectTextAndKeyboard()
        {
            // Arrange
            var reviewsPage = services.GetRequiredService<ReviewsPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), reviewsPage]);
            var userState = new UserState(pages, new UserData());
            var reviews = reviewStorage.GetReviews();

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var result = reviewsPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(reviewsPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));

            if (reviews.Count == 0)
                Assert.That(result.Text, Is.EqualTo(Resources.ReviewsPageText));

            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_StartPage()
        {
            // Arrange
            var reviewsPage = services.GetRequiredService<ReviewsPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), reviewsPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = reviewsPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<StartPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
        }

        [Test]
        public void Handle_UnknowMessage_ReviewsPageView()
        {
            // Arrange
            var reviewsPage = services.GetRequiredService<ReviewsPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), reviewsPage]);
            var userState = new UserState(pages, new UserData());
            var reviews = reviewStorage.GetReviews();

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };


            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "incorrect text" } };

            // Act
            var result = reviewsPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(reviewsPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));

            if (reviews.Count == 0)
                Assert.That(result.Text, Is.EqualTo(Resources.ReviewsPageText));

            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
