using CoCStatsTracker;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

/// <summary>
/// Тег клана:	#YPPGCCY8   #UQQGYJJP
/// 
/// Тег игрока: #2VGG92CL9  #LRPLYJ9U2 #G8P9Q299R
/// </summary>
class Program
{
    private static TelegramBotClient client = new TelegramBotClient(token: System.IO.File.ReadAllText(@"./../../../../CustomSolutionElements/TelegramBotClientToken.txt"));

    async static Task Main(string[] args)
    {
        CreateNewTestDb("#YPPGCCY8", "#UQQGYJJP");

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
                Console.WriteLine($"{DateTime.Now}: Принято сообщение: \"{update.Message.Text}\" от {update.Message.Chat.Username}");

                await Navigator.HandleMessage(botClient, update.Message);

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
            AddToDbCommandHandler.AddTrackedClan(clanTag, adminsKey:"KEFamily0707");

            AddToDbCommandHandler.AddClanMembers(clanTag);

            AddToDbCommandHandler.AddLastClanMembersStaticstics(clanTag);

            AddToDbCommandHandler.AddCurrentRaidToClan(clanTag);

            AddToDbCommandHandler.AddCurrentClanWarToClan(clanTag);
        }
    }
}
