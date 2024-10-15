using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests.AppointPagesTests
{
    internal class AppointRegisterDateTests
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
        public void View_AppointRegisterDate_CorrectTextAndKeyboard()
        {
            // Arrange
            var appointDate = services.GetRequiredService<AppointRegisterDatePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    appointDate
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт" , selectedDocName = "Иванов И.И."});
            var text = Resources.AppointRegisterDatePageText;

            // Act
            var result = appointDate.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointDate));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(7));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_DoctorsNamePage()
        {
            var appointDate = services.GetRequiredService<AppointRegisterDatePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    appointDate
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = appointDate.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<DoctorsNamePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(6));
        }

        [Test]  
        public void Handle_SelectedDateCallback_AppointTimePage()
        {
            // Arrange
            var appointDate = services.GetRequiredService<AppointRegisterDatePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    appointDate
                ]);


            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = userState.UserData.Id,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Now
            };

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "date_" + DateTime.Now.ToString("u").Split(" ").First() } };

            // Act
            var result = appointDate.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<AppointRegisterTimePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(8));
        }

        [Test]
        public void Handle_UnknowMessage_AppointRegisterDatePageView()
        {
            // Arrange
            var appointDate = services.GetRequiredService<AppointRegisterDatePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    appointDate
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });
            var text = Resources.AppointRegisterDatePageText;
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "ededef34343" } };

            // Act
            var result = appointDate.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointDate));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(7));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
        }


    }
}
