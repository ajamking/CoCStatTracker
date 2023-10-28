﻿using CoCStatsTrackerBot.Requests;
using Domain.Entities;
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
        using var db = new AppDbContext("Data Source=./../../../../CustomSolutionElements/CoCStatsTracker.db");

        TrackedClans = db.TrackedClans.ToList();

        foreach (var activeClan in TrackedClans)
        {
            //  TempFunctions.GetNonAttackersRaids(activeClan.Tag);
            //TempFunctions.GetNonAttackersCw(activeClan.Tag);

            //var testDaddyBuilder = new DaddyBuilder(activeClan);

            //testDaddyBuilder.UpdateCurrentRaid();

            //testDaddyBuilder.UpdateCurrentClanWar();
        }

        var a = db.ChangeTracker.Entries<CapitalRaid>();

        db.SaveChanges();

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
}
