using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoCStatsTrackerBot;

public static class LeaderRequestHandler
{
    public static List<string> LeaderKeys { get; set; } = new List<string>();

    static LeaderRequestHandler()
    {
        LeaderKeys.AddRange(System.IO.File.ReadAllLines(@"LeaderKeys.txt"));

        Console.WriteLine("Connection winh DB in LeaderRequestHandler sucsessful");
    }

    public async static Task HandleMessageLeaderLvl2(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                text: "Тут пока пусто, но контент скоро подвезут.");
    }

    public async static Task HandleMessageLeaderLvl3(ITelegramBotClient botClient, Message message, bool justMenu = false)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                       text: "Тут пока пусто, но контент скоро подвезут.");
    }

}
