using IRON_PROGRAMMER_BOT_Common.User.Pages.Main.Authorization;
using IRON_PROGRAMMER_BOT_Common.User.Pages.Main;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount.Appoint;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PersonalAccount;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using IRON_PROGRAMMER_BOT_Common.Storage;

namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests.AppointPagesTests
{
    internal class AppointRegisterTimePageTests
    {
        private IServiceProvider services;
        private AppointStorage storage;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
            storage = services.GetRequiredService<AppointStorage>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            storage.RemoveLast();

            if (services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public void View_AppointRegisterTimePage_CorrectTextAndKeyboard()
        {
            // Arrange
            var appointTime = services.GetRequiredService<AppointRegisterTimePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    appointTime
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = 99,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Parse(DateTime.Now.ToString("u").Split().First())
            };

            var text = Resources.AppointRegisterTimePageText;

            // Act
            var result = appointTime.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointTime));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(8));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
        }

        [Test]
        public void Handle_PrevPageCallback_AppointRegisterDatePage()
        {
            var appointTime = services.GetRequiredService<AppointRegisterTimePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    appointTime
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = appointTime.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<AppointRegisterDatePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(7));
        }

        [Test]
        public void Handle_SelectedTimeCallback_AppointRegisteredPage()
        {
            // Arrange
            var appointTime = services.GetRequiredService<AppointRegisterTimePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    appointTime
                ]);


            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = 1,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Parse(DateTime.Now.ToString("u").Split().First())
            };

            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = $"time_{DateTime.Now.AddHours(1).Hour}:00" } };

            // Act
            var result = appointTime.Handle(update, userState);
            var lastReview = storage.GetAppoints(1).Last();

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));
            Assert.That(userState.UserData.AppointRegistration, Is.EqualTo(lastReview), "Классы Appoint не равны");
            Assert.IsInstanceOf<AppointRegisteredPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(9), result.UpdatedUserState.CurrentPage.ToString());
        }

        [Test]
        public void Handle_UnknowMessage_AppointRegisterTimePageView()
        {
            // Arrange
            var appointTime = services.GetRequiredService<AppointRegisterTimePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    services.GetRequiredService<DoctorsNamePage>(),
                    services.GetRequiredService<AppointRegisterDatePage>(),
                    appointTime
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт", selectedDocName = "Иванов И.И." });

            userState.UserData.AppointRegistration = new IRON_PROGRAMMER_BOT_Common.Models.Appoint()
            {
                UserId = 1,
                Name = userState.UserData.PhoneNumber,
                DocName = userState.UserData.selectedDocName,
                DocType = userState.UserData.selectedDocType,
                Date = DateTime.Parse(DateTime.Now.ToString("u").Split().First())
            };

            var text = Resources.AppointRegisterTimePageText;
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "ededef34343" } };

            // Act
            var result = appointTime.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(appointTime));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(8));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
        }
    }
}
