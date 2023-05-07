using CoCApiDealer;
using Domain.Entities;
using Microsoft.Extensions.Primitives;
using Storage;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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

    //public static void InitializeDb()
    //{
    //    var clanTag = "#YPPGCCY8";

    //    var TrackedClans = new DBInit(clanTag).TrackedClans;

    //    Console.WriteLine(@$"Two versions of {TrackedClans.First(x => x.Tag == clanTag).Name} clan added to DB {DateTime.Now} ");
    //}

    public static void AddActivity()
    {
        //using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        //TrackedClans = db.TrackedClans.ToList();

        //foreach (var clan in TrackedClans)
        //{
        //    if (clan.IsCurrent == true)
        //    {
        //        clan.ClanMembers.FirstOrDefault(x => x.Tag == "#2VGG92CL9")
        //        .Carma
        //        .PlayerActivities.Add(
        //        new CustomActivity
        //        {
        //            Name = "TestActivity123123",
        //            EarnedPoints = 100,
        //            UpdatedOn = DateTime.Now,
        //            Description = "Тестирую как в кайф"
        //        });

        //        clan.ClanMembers.FirstOrDefault(x => x.Tag == "#G8P9Q299R")
        //           .Carma
        //           .PlayerActivities.Add(
        //           new CustomActivity
        //           {
        //               Name = "TestActivity123123",
        //               EarnedPoints = 20,
        //               UpdatedOn = DateTime.Now,
        //               Description = "Тестирую как в кайф"
        //           });

        //        clan.ClanMembers.FirstOrDefault(x => x.Tag == "#RV9JP9Y")
        //           .Carma
        //           .PlayerActivities.Add(
        //           new CustomActivity
        //           {
        //               Name = "TestActivity123123",
        //               EarnedPoints = 15,
        //               UpdatedOn = DateTime.Now,
        //               Description = "Тестирую как в кайф"
        //           });
        //    }

        //    foreach (var member in clan.ClanMembers)
        //    {
        //        foreach (var activity in member.Carma.PlayerActivities)
        //        {
        //            member.Carma.TotalCarma += activity.EarnedPoints;
        //        }
        //    }
        //}
      

        //db.Complete();
    }

    //public static void RecalculateDrawScores()
    //{
    //    using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

    //    TrackedClans = db.TrackedClans.ToList();

    //    var first = TrackedClans.FirstOrDefault(x => x.IsCurrent == true);
    //    var second = TrackedClans.FirstOrDefault(x => x.IsCurrent == true);
    //    second.PrizeDraws.First().Members = DrawDealer.RecalculatePrizeDrawScores(first, second, second.PrizeDraws.First().Members);

    //    var Q12 = first.ClanMembers.FirstOrDefault(x => x.Tag == "#2VGG92CL9");

    //    db.Complete();
    //}

    async static Task Main(string[] args)
    {
        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

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
                await Navigator.HandleMessage(botClient, update.Message);

                return;
            }
        }
        catch (Exception e)
        {

            Console.WriteLine("ignored exception" + e.Message + "in chat " + update.Message.Chat.Id);
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
