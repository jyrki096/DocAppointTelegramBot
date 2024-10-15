using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using IRON_PROGRAMMER_BOT_Common.User.Pages.PageResults;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common
{
    public class UpdateHandler(UserStateStorage storage, IServiceProvider services) : IUpdateHandler
    {
        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message && update.Type != Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                return;

            long telegramUserId;

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                telegramUserId = update!.Message!.From!.Id;
            else
                telegramUserId = update!.CallbackQuery!.From.Id;


            Console.WriteLine($"update_id={update.Id}, telegramUserId={telegramUserId}");

            var userState = storage.TryGet(telegramUserId);

            if (userState == null)
            {
                userState = new UserState(new Stack<IPage>([services.GetRequiredService<NotStatedPage>()]), new UserData());
            }

            Console.WriteLine($"update_id={update.Id}, CURRENT_userState={userState}");

            var result = userState!.CurrentPage.Handle(update, userState);
            Console.WriteLine($"update_id={update.Id}, send_text={result.Text}, UPDATED_UserState={result.UpdatedUserState}");

            var lastMessage = await SendResultAsync(client, update, telegramUserId, result);

            result.UpdatedUserState.UserData.LastMessage = new User.Pages.Message(lastMessage.MessageId, result.IsMedia);
            storage.AddOrUpdate(telegramUserId, result.UpdatedUserState);
        }

        private async Task<Telegram.Bot.Types.Message> SendResultAsync(ITelegramBotClient client, Update update, long telegramUserId, PageResult result)
        {
            switch (result)
            {
                case PhotoPageResult photoPageResult:
                    return await SendPhotoAsync(client, update, telegramUserId, photoPageResult);
                default:
                    return await SendTextAsync(client, update, telegramUserId, result);
            }
        }

        private async Task<Telegram.Bot.Types.Message> SendTextAsync(ITelegramBotClient client, Update update, long telegramUserId, PageResult result)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageTextAsync(
                    chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                    text: result.Text,
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup,
                    parseMode: result.ParseMode);
            }

            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(chatId: telegramUserId,
                                                messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }

            return await client.SendTextMessageAsync(chatId: telegramUserId,
                                                text: result.Text,
                                                replyMarkup: result.ReplyMarkup,
                                                parseMode: result.ParseMode);
        }

        private async Task<Telegram.Bot.Types.Message> SendPhotoAsync(ITelegramBotClient client, Update update, long telegramUserId, PhotoPageResult result)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageMediaAsync(chatId: telegramUserId,
                    messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                    media: new InputMediaPhoto(result.Photo)
                    {
                        Caption = result.Text,
                        ParseMode = result.ParseMode
                    },
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup);
            }

            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(chatId: telegramUserId,
                                                messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }

            return await client.SendPhotoAsync(
                            chatId: telegramUserId,
                            photo: result.Photo,
                            caption: result.Text,
                            replyMarkup: result.ReplyMarkup);
        }
    }
}
