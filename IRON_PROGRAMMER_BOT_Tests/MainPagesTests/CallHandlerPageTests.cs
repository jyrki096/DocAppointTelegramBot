using IRON_PROGRAMMER_BOT_Common;
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
    internal class CallHandlerPageTests
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
        public void View_CallHandlerPage_CorrectTextAndKeybord()
        {
            // Arrange
            var callHandlerPage = services.GetRequiredService<CallHandlerPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<ConsultationPage>(), callHandlerPage]);
            var ind = new Random().Next(0, 1);
            var messages = new string[2] { "randominput", "7985-665-44-34" };
            var userState = new UserState(pages, new UserData() { SentMessage = messages[ind] });

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };


            var result = callHandlerPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(callHandlerPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.Text, Is.AnyOf([Resources.IncorrectUserNumberText, Resources.SuccessCallRequestText]));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_CallHandlerPage()
        {
            // Arrange
            var callHandlerPage = services.GetRequiredService<CallHandlerPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<ConsultationPage>(), callHandlerPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = callHandlerPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<ConsultationPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));

            Assert.That(result.Text, Is.EqualTo(Resources.ConsultationPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
        }



        [Test]
        public void Handle_UnknowMessage_CallHandlerPageView()
        {
            // Arrange
            var callHandlerPage = services.GetRequiredService<CallHandlerPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), services.GetRequiredService<ConsultationPage>(), callHandlerPage]);
            var userState = new UserState(pages, new UserData() { SentMessage = "7985-223-33-22" });
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "randominput" } };

            // Act
            var result = callHandlerPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<CallHandlerPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.AnyOf([Resources.IncorrectUserNumberText, Resources.SuccessCallRequestText]));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
