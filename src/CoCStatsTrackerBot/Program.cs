using CoCStatsTracker;
using CoCStatsTrackerBot.Requests;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary> 
/// Тег клана:	"#YPPGCCY8", "#UQQGYJJP", "#VUJCUQ9Y"
/// private static readonly string _botClientToken = System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/BotClientToken.txt");
/// Тег игрока: AJAMKING: #G8P9Q299R Зануда051: #LRPLYJ9U2
/// </summary>

class Program
{
#if DEBUG
    private static readonly string _botClientToken = System.IO.File.ReadAllText(@"./CustomSolutionElements/BotClientTokenTEST.txt");
#else
    private static readonly string _botClientToken = System.IO.File.ReadAllText(@"./CustomSolutionElements/BotClientToken.txt");
#endif

    public static TelegramBotClient BotClient { get; } = new(token: _botClientToken);

    public static string BanListPath { get; } = @"./CustomSolutionElements/BannedUsers.txt";

    public static string ExceptionLogsPath { get; } = @"./CustomSolutionElements/ErrorLogs.txt";

    public static string AdminsChatId { get; } = "6621123435";

    async static Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        BotBackgroundManager.StartAstync(BotClient);

        BotClient.StartReceiving(HandleUpdateAsync, HandleError);

        Console.WriteLine("Bot started");

        var host = new HostBuilder()
            .ConfigureHostConfiguration(h => { })
            .UseConsoleLifetime()
            .Build();

        host.Run();
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //Console.WriteLine(Environment.CurrentManagedThreadId);

        try
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                if (IsBannedUserCheck(botClient, update).Result)
                {
                    return;
                }

                if (update.Message.Chat.Type is ChatType.Channel or ChatType.Group or ChatType.Supergroup)
                {
                    if (IsInvalidGroupMessageCheck(update).Result || IsUnknownGroupIdCheck(botClient, update).Result)
                    {
                        return;
                    }
                }

                Console.Write($"{DateTime.Now}: Принято сообщение: \"{update.Message.Text}\" от ");

                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine($"@{update.Message.Chat.Username}");

                Console.ResetColor();

                await Task.Run(() => Navigation.Execute(botClient, update.Message));

                return;
            }
        }
        catch (Exception e)
        {
            e.LogException(update.Message.Chat.Username, update.Message.Chat.Id, update.Message.Text, "Проигнорировали исключение в HandleUpdateAsync");

            Console.WriteLine("Проигнорировали исключение " + e.Message + "в чате " + update.Message);
        }
    }

    static Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Прилетело исключение {exception.Message}");

        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $"Ошбика телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        return Task.CompletedTask;
    }

    async static Task<bool> IsBannedUserCheck(ITelegramBotClient botClient, Update update)
    {
        if (System.IO.File.ReadAllLines(BanListPath).Contains(update.Message.Chat.Id.ToString()))
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                text: StylingHelper.MakeItStyled("Вы были заблокированы за неподобающее поведение и " +
                "больше не можете пользоваться ботом.", UiTextStyle.Default),
                parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    async static Task<bool> IsInvalidGroupMessageCheck(Update update)
    {
        var validGroupMessage = true;

        foreach (var key in Navigation.BotSlashFunctions.Keys)
        {
            if (update.Message.Text.Contains(key))
            {
                validGroupMessage = false;
            }
        }

        return validGroupMessage;
    }

    async static Task<bool> IsUnknownGroupIdCheck(ITelegramBotClient botClient, Update update)
    {
        if (update.Message.Text.Contains("/group_get_chat_id"))
        {
            return false;
        }

        if (!GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == update.Message.Chat.Id.ToString() && x.IsInBlackList == false).Any())
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
              text: StylingHelper.MakeItStyled("Доступ к функциям бота закрыт. Возможные причины:" +
              "\n1. Ваш клан не подключен к боту;" +
              "\n2. Глава клана не связал бота с группой;" +
              "\n3. Глава сменил привязанную к клану ТГ группу;" +
              "\n4. Истекла подписка на услуги бота.", UiTextStyle.Default),
              parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    private static void CreateNewTestDb(params string[] ClanTags)
    {
        AddToDbCommandHandler.ResetDb();

        foreach (var clanTag in ClanTags)
        {
            if (clanTag == "#VUJCUQ9Y")
            {
                AddToDbCommandHandler.AddTrackedClan(clanTag);

                UpdateDbCommandHandler.ResetClanChatId(clanTag, "-1002146710907");

                UpdateDbCommandHandler.ResetClanAdminKey(clanTag, "$Vikand0707");
            }
            else
            {
                AddToDbCommandHandler.AddTrackedClan(clanTag);

                UpdateDbCommandHandler.ResetClanAdminKey(clanTag, "$KEFamily0707");
            }

            AddToDbCommandHandler.AddClanMembers(clanTag);

            AddToDbCommandHandler.AddCurrentRaidToClan(clanTag);

            try
            {
                AddToDbCommandHandler.AddCurrentClanWarToClan(clanTag);
            }
            catch
            {
                AddToDbCommandHandler.AddCurrentCwlClanWarsToClan(clanTag);
            }

            Console.WriteLine($"Clan {clanTag} aded");
        }
    }
}