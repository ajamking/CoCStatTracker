using CoCApiDealer;
using CoCApiDealer.ApiRequests;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using Microsoft.Extensions.Primitives;
using Storage;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CoCStatsTrackerBot;

/// <summary>
/// Тег клана:	#YPPGCCY8   #UQQGYJJP
/// 
/// Тег игрока: #2VGG92CL9  #LRPLYJ9U2
/// </summary>

class Program
{
    private static TelegramBotClient client = new TelegramBotClient(token: "6148969149:AAF_Vrsf0NZZRp3PMl30s1VGhtctj2hPU4k");
    public static List<TrackedClan> TrackedClans { get; set; } = new List<TrackedClan>();

    async static Task Main(string[] args)
    {
        TempFunctions.GetCwMembers("#YPPGCCY8");

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
