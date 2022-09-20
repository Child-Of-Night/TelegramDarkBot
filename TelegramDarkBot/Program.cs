using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Newtonsoft.Json.Linq;


namespace Telegram_Bot
//Name: DarkBot
//User name: darken_bot
//
//Available commands:
//help - list of available commands
//keyboard - show demo the demo keyboard
//inline_keyboard - show demo the demo inline keyboard
{

    class Program
    {
        private const string TokenVariable = "DarkBotToken";
        static async Task Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable(TokenVariable);
            if(token == null)
            {
                return;
            }
            var client = new TelegramBotClient(token);

            CancellationTokenSource cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            client.StartReceiving(HandleUpdateAsync,
                                  HandlePollingErrorAsync,
                                  receiverOptions,
                                  cts.Token);

            var me = await client.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();

            static string ChitChater(string message)
            {
                string result = "";
                switch (message)
                {
                    case "Помощь" or "помощь":
                        result = "Поздаровайся хотя бы, а то чё как лох\n" +
                        "Используй: привет, хай, прив, здарова, Hello, Hi и вся ху*ня";
                        break;
                    case "Hi" or "hi":
                        result = "Hi! Wazzup?";
                        break;
                    case "И вся ху*ня" or "и вся ху*ня" or "И вся хуйня" or "и вся хуйня" or "Вся ху*ня" or "вся ху*ня"
                    or "Вся хуйня" or "вся хуйня" or "ху*ня" or "Ху*ня" or "хуйня" or "Хуйня":
                        result = "Ой ты гля, хитрая колбаса, мамкин взломщик, папкин тестировщик...\n" +
                        "Перездоровывайся давай, не выйобживайся!";
                        break;
                    case "Хай" or "хай":
                        result = "Хэй хо!";
                        break;
                    case "Хой" or "хой":
                        result = "Челюсть до-лОЙ!";
                        break;
                    case "Hello" or "hello":
                        result = "Hello motherfuck'a!";
                        break;
                    case "Прив" or "прив":
                        result = "хуив, ну то есть приветствую";
                        break;
                    case "Привет" or "привет":
                        result = "И тебе привет";
                        break;
                    case "Здарова" or "Здарово" or "Здорова" or "здарова" or "здарово" or "здорова" or "Доров" or "доров"
                    or "Дарова" or "дарова" or "Дорова" or "дорова":
                        result = "Здарова заебал!";
                        break;
                    case "/help":
                        result = "Список команд пока пуст";
                        break;
                    default:
                        break;
                }
                return result;
            }

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                Message sentMessage;

                // Send generated text by ChitChater
                sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: ChitChater(messageText),
                        cancellationToken: cancellationToken);
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}