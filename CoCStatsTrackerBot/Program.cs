using Domain.Entities;
using Storage;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// #YPPGCCY8 - тег клана
/// #2VGG92CL9 - тег игрока
/// </summary>

class Program
{
    private static TelegramBotClient client = new TelegramBotClient("6148969149:AAF_Vrsf0NZZRp3PMl30s1VGhtctj2hPU4k");
    public static List<TrackedClan> TrackedClans { get; set; }

    //static Program()
    //{
    //    var clanTag = "#YPPGCCY8";

    //    TrackedClans = new DBInit(clanTag).TrackedClans;

    //    Console.WriteLine(@$"Two versions of {TrackedClans.First(x => x.Tag == clanTag).Name} clan added to DB {DateTime.Now} ");
    //}

    async static Task Main(string[] args)
    {
        using (AppDbContext db = new AppDbContext("Data Source=CoCStatsTracker.db"))
        {
            TrackedClans = db.TrackedClans.ToList();

            db.Complete();

            Console.WriteLine("Connection winh DB sucsessful");
        }


        client.StartReceiving(DealWithUpdate, DealWithError);
        Console.ReadLine();

    }

    async static Task DealWithUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message.Text != null)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "SomeTestMessage");
        }
    }

    private static Task DealWithError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
