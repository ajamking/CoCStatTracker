using Domain.Entities;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CoCStatsTrackerBot;

public static class LeaderRequestHandler
{
    public static List<string> LeaderKeys { get; set; } = new List<string>();

    public static List<TrackedClan> TrackedClans { get; set; } = new List<TrackedClan>();

    static LeaderRequestHandler()
    {
        LeaderKeys.AddRange(System.IO.File.ReadAllLines(@"LeaderKeys.txt"));

        using var db = new AppDbContext("Data Source=CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

        db.Complete();

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
