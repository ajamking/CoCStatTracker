using CoCStatsTracker;
using Domain.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// Тег клана:	#YPPGCCY8   #UQQGYJJP
/// 
/// Тег игрока: AJAMKING: #G8P9Q299R Зануда051: #LRPLYJ9U2
/// </summary>

class Program
{
    private static TelegramBotClient _client = new TelegramBotClient(token: System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/TelegramBotClientToken.txt"));

    async static Task Main(string[] args)
    {
        //CreateNewTestDb("#YPPGCCY8", "#UQQGYJJP", "#VUJCUQ9Y");
        //AddToDbCommandHandler.AddCurrentRaidToClan("#YPPGCCY8");

        _client.StartReceiving(HandleUpdateAsync, HandleError);

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
                Console.WriteLine($"{DateTime.Now}: Принято сообщение: \"{update.Message.Text}\" от {update.Message.Chat.Username}");

                Navigation.Execute(botClient, update.Message);

                return;
            }
        }
        catch (Exception e)
        {
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
                AddToDbCommandHandler.AddTrackedClan(clanTag, adminsKey: "@VIKAND");
            }
            else
            {
                AddToDbCommandHandler.AddTrackedClan(clanTag, adminsKey: "@KEFamily0707");
            }

            AddToDbCommandHandler.AddClanMembers(clanTag);

            AddToDbCommandHandler.AddCurrentRaidToClan(clanTag);

            //AddToDbCommandHandler.AddCurrentClanWarToClan(clanTag);
        }
    }
}
