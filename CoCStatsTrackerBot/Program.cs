using Domain.Entities;
using Microsoft.Extensions.Primitives;
using Storage;
using System.IO;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

/// <summary>
/// #YPPGCCY8 - тег клана
/// #2VGG92CL9 - тег игрока
/// </summary>

class Program
{
    private static TelegramBotClient client = new TelegramBotClient(token: "6148969149:AAF_Vrsf0NZZRp3PMl30s1VGhtctj2hPU4k");

    public static List<TrackedClan> TrackedClans { get; set; } = new List<TrackedClan>();

    public static List<string> LeaderKeys { get; set; } = new List<string>();

    public static Dictionary<string, ReplyKeyboardMarkup> MKeyboards { get; set; } = MemberKeyboards.CustomKeyboards;

    public static List<string> MemberFirstLevelButtonMessages { get; set; } = new List<string>()
    {
        "Игрок",
        "Клан",
        "Текущая война",
        "Текущий рейд",
        "Розыгрыш",
        "Выбор интерфейса",
        "Член клана",
        "В главное меню",
    };

    public static List<string> MemberSecondLevelButtonMessages { get; set; } = new List<string>()
    {
       "Все об игроке", "Главное об игроке", "Показатели войн", "Показатели рейдов", "Показатели розыгрыша", "Войска", "История кармы",
       "Главное о клане", "Члены клана","История войн", "История рейдов","История розыгрышей", "Активные супер юниты",
       "Главное о войне", "Все о войне",
       "Главное о рейде", "Все о рейде",
       "Главное о розыгрыше", "Все о розыгрыше",
    };

    public static List<string> LeaderFirstLevelAllowdeMessages { get; set; } = new List<string>()
    {
        "Глава/Соруководитель",
        ""
    };

    public static List<string> LeaderSecondLevelAllowdeMessages { get; set; } = new List<string>()
    {
        "",
        ""
    };

    public static Regex PlayerRegex { get; set; } = new Regex(@"^#(\w{9})$");

    public static Regex ClanRegex { get; set; } = new Regex(@"^#(\w{8})$");

    //Сюда мы записываем ID чата и тег игрока/клана, который он вводил последним
    public static Dictionary<long, string> LastUserTags { get; set; } = new Dictionary<long, string>();

    //static Program()
    //{
    //    var clanTag = "#YPPGCCY8";

    //    TrackedClans = new DBInit(clanTag).TrackedClans;

    //    Console.WriteLine(@$"Two versions of {TrackedClans.First(x => x.Tag == clanTag).Name} clan added to DB {DateTime.Now} ");
    //}

    async static Task Main(string[] args)
    {
        LeaderKeys.AddRange(System.IO.File.ReadAllLines(@"LeaderKeys.txt"));

        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

        db.Complete();

        Console.WriteLine("Connection winh DB sucsessful");

        client.StartReceiving(HandleUpdateAsync, HandleError);

        Console.ReadLine();
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update?.Message?.Text != null)
        {
            await HandleZeroLevelMessageAsync(botClient, update.Message);

            return;
        }

    }

    static Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Прилетело исключение {exception.Message}");

        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $"Ошбика телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        return Task.CompletedTask;
    }

    async static Task HandleZeroLevelMessageAsync(ITelegramBotClient botClient, Message message)
    {
        ReplyKeyboardMarkup authorizationKeyboard = new(new[] { new KeyboardButton[] { "Член клана", "Руководитель" }, })
        {
            ResizeKeyboard = true
        };

        switch (message.Text)
        {
            case "/start":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: "Приветствую! Выберите в качестве кого вы хотите войти", replyMarkup: authorizationKeyboard);
                return;

            case "Выбор интерфейса":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: "Выберите интерфейс", replyMarkup: authorizationKeyboard);
                return;

            case string msg when LeaderFirstLevelAllowdeMessages.Contains(msg):
                await HandleLeaderFirstLevelMessageAsync(botClient, message);
                return;

            case string msg when LeaderSecondLevelAllowdeMessages.Contains(msg):
                await HandleLeaderFirstLevelMessageAsync(botClient, message);
                return;

            case string msg when MemberFirstLevelButtonMessages.Contains(msg):
                await HandleMemberFirstLevelMessageAsync(botClient, message);
                return;

            case string msg when MemberSecondLevelButtonMessages.Contains(msg):
                await HandleMemberSecondLevelMessageAsync(botClient, message);
                return;

            case string msg when PlayerRegex.IsMatch(msg):
                await HandleMemberSecondLevelMessageAsync(botClient, message);
                return;

            case string msg when ClanRegex.IsMatch(msg):
                await HandleMemberSecondLevelMessageAsync(botClient, message);
                return;

            default:
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Вы сказали \"{message.Text}\", " +
               $"но я еще не знаю таких сложных вещей. 🥺 Выберите что-то из меню или введите корректный тег игрока/клана 😁");
                return;
        }
    }

    async static Task HandleMemberFirstLevelMessageAsync(ITelegramBotClient botClient, Message message)
    {
        switch (message.Text)
        {
            case "Член клана":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: "Что бы вы хотели узнать?", replyMarkup: MKeyboards["mainKeyboard"]);
                return;

            case "В главное меню":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: "Что бы вы хотели узнать?", replyMarkup: MKeyboards["mainKeyboard"]);
                return;

            case "Игрок":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                    text: "Введите тег игрока в формате #123456789, а затем выберите пункт из меню", replyMarkup: MKeyboards["playerKeyboard"]);
                return;

            case "Клан":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                   text: "Что бы вы хотели узнать об клане?", replyMarkup: MKeyboards["clanKeyboard"]);
                return;

            case "Текущая война":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                 text: "Что бы вы хотели узнать о текущей войне?", replyMarkup: MKeyboards["currentClanWarKeyboard"]);
                return;

            case "Текущий рейд":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Что бы вы хотели узнать о текущем рейде?", replyMarkup: MKeyboards["currentRaidKeyboard"]);
                return;

            case "Розыгрыш":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Что бы вы хотели узнать о текущем розыгрыше?", replyMarkup: MKeyboards["prizeDrawKeboard"]);
                return;

            case "Выбор интерфейса":
                await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Возвращаемся к выбору интерфейса");
                break;

            default:
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Вы сказали \"{message.Text}\", " +
               $"но я еще не знаю таких сложных вещей. 🥺 Зато вы можете выбрать что-то из меню!😁");
                return;
        }
    }

    async static Task HandleMemberSecondLevelMessageAsync(ITelegramBotClient botClient, Message message)
    {
        switch (message.Text)
        {
            case string msg when PlayerRegex.IsMatch(msg):
                {
                    if (LastUserTags.ContainsKey(message.Chat.Id))
                    {
                        LastUserTags.Remove(message.Chat.Id);
                    }
                    LastUserTags.TryAdd(message.Chat.Id, message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: "Тег игрока задан в корректной форме. Откройте меню и выберите нужный тип информации");
                    return;
                }


            case string msg when ClanRegex.IsMatch(msg):
                {
                    if (LastUserTags.ContainsKey(message.Chat.Id))
                    {
                        LastUserTags.Remove(message.Chat.Id);
                    }
                    LastUserTags.TryAdd(message.Chat.Id, message.Text);
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                      text: "Тег клана задан в корректной форме. Откройте меню и выберите нужный тип информации");
                    return;
                }


            case "Все об игроке":
                {
                    if (LastUserTags.ContainsKey(message.Chat.Id))
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: BotPlayerInfo.FullPlayerInfo(LastUserTags[message.Chat.Id], TrackedClans),
                        parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: "Тег игрока не введен");
                        return;
                    }
                }

            case "Главное об игроке":
                {
                    if (LastUserTags.ContainsKey(message.Chat.Id))
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                        text: BotPlayerInfo.ShortPlayerInfo(LastUserTags[message.Chat.Id], TrackedClans),
                        parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: "Тег игрока не введен");
                        return;
                    }
                }


            case "Показатели войн":
                {
                    if (LastUserTags.ContainsKey(message.Chat.Id))
                    {
                        await botClient.SendPhotoAsync(message.Chat.Id,
                        photo: BotPlayerInfo.ShortPlayerInfo(LastUserTags[message.Chat.Id], TrackedClans),
                        parseMode: ParseMode.MarkdownV2);
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: "Тег игрока не введен");
                        return;
                    }
                }

            case "Показатели рейдов":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Показатели розыгрыша":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Войска":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "История кармы":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Главное о клане":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Члены клана":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "История войн":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "История рейдов":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "История розыгрышей":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Активные супер юниты":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Главное о войне":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Все о войне":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Главное о рейде":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Все о рейде":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Главное о розыгрыше":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            case "Все о розыгрыше":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Я еще не полноценный и не могу вам ответить 🥺");
                return;

            default:
                await botClient.SendTextMessageAsync(message.Chat.Id, text: $"Вы сказали \"{message.Text}\", " +
               $"но я еще не знаю таких сложных вещей. 🥺 Зато вы можете выбрать что-то из меню!😁");
                return;



        }
    }

    async static Task HandleLeaderFirstLevelMessageAsync(ITelegramBotClient botClient, Message message)
    {

    }

    async static Task HandleLeaderSecondLevelMessageAsync(ITelegramBotClient botClient, Message message)
    {

    }

}
