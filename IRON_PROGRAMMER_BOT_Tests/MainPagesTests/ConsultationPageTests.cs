using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Update = Telegram.Bot.Types.Update;

namespace IRON_PROGRAMMER_BOT_Tests.MainPagesTests
{
    internal class ConsultationPageTests
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
        public void View_ConsultationPage_CorrectTextAndKeybord()
        {
            // Arrange
            var consultationPage = services.GetRequiredService<ConsultationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), consultationPage]);
            var userState = new UserState(pages, new UserData());

            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var result = consultationPage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(consultationPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));

            Assert.That(result.Text, Is.EqualTo(Resources.ConsultationPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_StartPage()
        {
            // Arrange
            var consultationPage = services.GetRequiredService<ConsultationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), consultationPage]);
            var userState = new UserState(pages, new UserData());

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = consultationPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));

            Assert.IsInstanceOf<StartPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
        }

        [Test]
        public void Handle_CorrectUserInput_CallHandlerPage()
        {
            // Arrange
            var consultationPage = services.GetRequiredService<ConsultationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), consultationPage]);
            var userState = new UserState(pages, new UserData());
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "7988-987-76-54" } };

            // Act
            var result = consultationPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<CallHandlerPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.EqualTo(Resources.SuccessCallRequestText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }



        [Test]
        public void Handle_UnknowMessage_ConsultationPageView()
        {
            // Arrange
            var consultationPage = services.GetRequiredService<ConsultationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), consultationPage]);
            var userState = new UserState(pages, new UserData());
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("НАЗАД")]
            };

            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "notnumbers92929920" } };

            // Act
            var result = consultationPage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(services.GetRequiredService<CallHandlerPage>()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));

            Assert.That(result.Text, Is.EqualTo(Resources.IncorrectUserNumberText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }
    }
}
