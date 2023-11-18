using CoCStatsTracker;
using CoCStatsTrackerBot.Requests;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// Тег клана:	"#YPPGCCY8", "#UQQGYJJP", "#VUJCUQ9Y"
/// 
/// #2Q9C292Y2  #2Q8028UJL - не крутили ЛВК
/// 
/// 
/// Тег игрока: AJAMKING: #G8P9Q299R Зануда051: #LRPLYJ9U2
/// </summary>

class Program
{
    private static string _botClientToken = System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/BotClientToken.txt");

    private static string _botClientTokenTest = System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/BotClientTokenTEST.txt");

    public static string BanListPath { get; } = @"./../../../../CustomSolutionElements/BannedUsers.txt";

    public static string ExceptionLogsPath { get; } = @"./../../../../CustomSolutionElements/ErrorLogs.txt";

    public static TelegramBotClient BotClient { get; } = new(token: _botClientToken);

    public static string AdminsChatId { get; } = "6621123435";

    async static Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        //CreateNewTestDb("#YPPGCCY8", "#UQQGYJJP", "#VUJCUQ9Y");

        BotBackgroundTasksManager.StartAstync(BotClient);

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
                if (System.IO.File.ReadAllLines(BanListPath).Contains(update.Message.Chat.Id.ToString()))
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                        text: StylingHelper.MakeItStyled("Вы были заблокированы за неподобающее поведение и " +
                        "больше не можете пользоваться ботом.", UiTextStyle.Default),
                        parseMode: ParseMode.MarkdownV2);

                    return;
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

    public static void CreateNewTestDb(params string[] ClanTags)
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