using CoCApiDealer;
using CoCApiDealer.ApiRequests;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Storage;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// CЕрилог добавить ВЕЗДЕ.
/// НЕ ЗАБЫТЬ ОБЫГРАТЬ РЕКВЕСТ ЦВЛ ГРУППЫ
/// Тег клана:	#YPPGCCY8   #UQQGYJJP
/// 
/// Тег игрока: #2VGG92CL9  #LRPLYJ9U2 #G8P9Q299R
/// </summary>

class Program
{
    private static TelegramBotClient client = new TelegramBotClient(token: System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/TelegramBotClientToken.txt"));

    public static List<TrackedClan> TrackedClans { get; set; } = new List<TrackedClan>();

    async static Task Main(string[] args)
    {
        //var dbinit = new DBInit("#YPPGCCY8", "#UQQGYJJP");

        //var asf = new CwlGroupRequest();

        //var answ = asf.CallApi("#YPPGCCY8");

        using var db = new AppDbContext("Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

        foreach (var activeClan in TrackedClans.Where(x => x.IsCurrent == true))
        {
            //TempFunctions.GetNonAttackersRaids(activeClan.Tag);
            //TempFunctions.GetNonAttackersCw(activeClan.Tag);

            var testDaddyBuilder = new DaddyBuilder(activeClan);

            testDaddyBuilder.UpdateCurrentRaid();

            testDaddyBuilder.UpdateCurrentClanWar();
        }

        var abs = db.ChangeTracker.Entries<RaidDefense>();

        db.Complete();


        Console.WriteLine("Connection winh DB in MemberRequestHandler sucsessful");

        client.StartReceiving(HandleUpdateAsync, HandleError);

        Console.WriteLine("Bot started");

        Console.ReadLine();

        await Task.CompletedTask;
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                Console.WriteLine(update.Message.Chat.Username + "  Отправил сообщение " + update.Message.Text + " " + DateTime.Now);

                await Navigator.HandleMessage(botClient, update.Message);

                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ignored exception" + e.Message + "in chat " + update.Message);
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
}
