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


namespace IRON_PROGRAMMER_BOT_Tests.PersonalAccountPagesTests.AppointPagesTests
{
    internal class DoctorsNamePageTests
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
        public void View_DoctorsNamePage_CorrectTextAndData()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт" });
            var text = Resources.DoctorsNamePageText;

            // Act
            var result = docNamePage.View(null, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(docNamePage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(6));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
        }

        [Test]
        public void View_DoctorsNamePage_CorrectTherapistKeyboard()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт" });
            var expectedButtons = GetKeyboard(userState);

            // Act
            var result = docNamePage.View(null, userState);

            // Assert

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void View_DoctorsNamePage_CorrectOculistKeyboard()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Окулист" });
            var expectedButtons = GetKeyboard(userState);

            // Act
            var result = docNamePage.View(null, userState);

            // Assert

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void View_DoctorsNamePage_CorrectSurgeonKeyboard()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Хирург" });
            var expectedButtons = GetKeyboard(userState);

            // Act
            var result = docNamePage.View(null, userState);

            // Assert

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        [Test]
        public void View_DoctorsNamePage_CorrectDentistKeyboard()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Стоматолог" });
            var expectedButtons = GetKeyboard(userState);

            // Act
            var result = docNamePage.View(null, userState);

            // Assert

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        

        [Test]
        public void Handle_SelectedNameCallback_DoctorsNamePage()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Иванов И.И." } };

            // Act
            var result = docNamePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<AppointRegisterDatePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(7));
        }


        [Test]
        public void Handle_PrevPageCallback_DoctorsTypePagePage()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test" });
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "НАЗАД" } };

            // Act
            var result = docNamePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.IsInstanceOf<DoctorsTypePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
        }


        [Test]
        public void Handle_UnknowMessage_DoctorsNamePageView()
        {
            // Arrange
            var docNamePage = services.GetRequiredService<DoctorsNamePage>();
            var pages = new Stack<IPage>(
                [
                    services.GetRequiredService<NotStatedPage>(),
                    services.GetRequiredService<StartPage>(),
                    services.GetRequiredService<AuthorizationPage>(),
                    services.GetRequiredService<PersonalAccountPage>(),
                    services.GetRequiredService<DoctorsTypePage>(),
                    docNamePage
                ]);

            var userState = new UserState(pages, new UserData() { PhoneNumber = "79998887766", Name = "Test", selectedDocType = "Терапевт" });
            var text = Resources.DoctorsNamePageText;
            var expectedButtons = GetKeyboard(userState);
            var update = new Update() { Message = new Telegram.Bot.Types.Message() { Text = "ededef34343" } };

            // Act
            var result = docNamePage.Handle(update, userState);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResult)));

            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(docNamePage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(6));

            Assert.That(result.Text, Is.EqualTo(text));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));

            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyBoardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup);
        }

        private InlineKeyboardButton[][] GetKeyboard(UserState userState)
        {
            switch (userState.UserData.selectedDocType)
            {
                case "Терапевт":
                    {
                        return
                            [
                                [

                                   InlineKeyboardButton.WithCallbackData("Иванов И.И.")
                                ],
                                [
                                    InlineKeyboardButton.WithCallbackData("НАЗАД")
                                ]
                            ];
                    }
                case "Окулист":
                    {
                        return
                            [
                                [

                                    InlineKeyboardButton.WithCallbackData("Петров В.В.")
                                ],
                                [
                                    InlineKeyboardButton.WithCallbackData("НАЗАД")
                                ]
                            ];
                    }
                case "Хирург":
                    {
                        return
                            [
                                [

                                    InlineKeyboardButton.WithCallbackData("Алексеев А.А.")
                                ],
                                [
                                    InlineKeyboardButton.WithCallbackData("НАЗАД")
                                ]
                            ];
                    }
                case "ЛОР":
                    {
                        return
                            [
                                [

                                    InlineKeyboardButton.WithCallbackData("Соколов И.И.")
                                ],
                                [
                                    InlineKeyboardButton.WithCallbackData("НАЗАД")
                                ]
                            ];
                    }
                case "Стоматолог":
                    {
                        return
                            [
                                [

                                    InlineKeyboardButton.WithCallbackData("Пронин Н.А.")
                                ],
                                [
                                    InlineKeyboardButton.WithCallbackData("НАЗАД")
                                ]
                            ];
                    }
            }

            return
                [
                    [
                        InlineKeyboardButton.WithCallbackData("НАЗАД")
                    ]
                ];
        }
    }
}
