using CoCStatsTracker;
using CoCStatsTrackerBot.Requests;
using Domain.Entities;
using System.Net.NetworkInformation;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

public static class BotBackgroundTasksManager
{
    public static async Task StartAstync(ITelegramBotClient botClient)
    {
        while (CheckInternetConnection())
        {
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Обновляю кланы...");

            var allTrackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

            var tasks = allTrackedClans
           .Select(x => Task.Run(() => UpdateAllProperties(x)))
           .ToList();

            await Task.WhenAll(tasks);

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Все кланы обновлены.\n");

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Начинаю рассылку...");

            SendDailyMessages(allTrackedClans, botClient);

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Сообщения разосланы.\n");

            await Task.Delay(TimeSpan.FromHours(1));
        }
    }

    private static void UpdateAllProperties(TrackedClan clan)
    {
        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanBaseProperties(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanClanMembers(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateClanCurrentRaid(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        try
        {
            UpdateDbCommandHandler.UpdateCurrentCwlClanWars(clan.Tag);
        }
        catch
        {
            ExecuteUpdate(() => UpdateDbCommandHandler.UpdateCurrentClanWar(clan.Tag), $"[{clan.Tag}] - {clan.Name}");
        }

        Console.WriteLine($"[{clan.Tag}] - {clan.Name} - Клан полностью обновлен.");
    }

    private static void ExecuteUpdate(Action action, string clanTagAndName)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{clanTagAndName} - Обновление частично не прошло, ошибка: {ex.Message}");
        }
    }

    private static void SendDailyMessages(List<TrackedClan> trackedClans, ITelegramBotClient botClient)
    {
        var properClans = trackedClans.Where(x => x.IsInBlackList == false && x.RegularNewsLetterOn == true && !string.IsNullOrEmpty(x.ClansTelegramChatId));

        foreach (var clan in properClans)
        {
            SendRaidsMessage(clan, botClient);

            SendWarMessage(clan, botClient);
        }

    }

    private static void SendRaidsMessage(TrackedClan clan, ITelegramBotClient botClient)
    {
        var lastRaidUi = GetFromDbQueryHandler.GetLastRaidUi(clan.Tag);

        if (lastRaidUi != null)
        {
            var raidTimeLeft = Math.Round(lastRaidUi.EndedOn.Subtract(DateTime.Now).TotalHours, 2);

            if (raidTimeLeft > 15 && raidTimeLeft < 16)
            {
                var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(lastRaidUi);

                botClient.SendTextMessageAsync(clan.ClansTelegramChatId,
                    text: answer,
                    parseMode: ParseMode.MarkdownV2);
            }
        }
    }

    private static void SendWarMessage(TrackedClan clan, ITelegramBotClient botClient)
    {
        var clanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(clan.Tag);

        if (clanWarUi != null)
        {
            var warTimeLeft = Math.Round(clanWarUi.EndedOn.Subtract(DateTime.Now).TotalHours, 2);

            if (warTimeLeft > 0 && warTimeLeft < 3)
            {
                var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(clanWarUi);

                botClient.SendTextMessageAsync(clan.ClansTelegramChatId,
                    text: answer,
                    parseMode: ParseMode.MarkdownV2);
            }
        }
    }

    private static bool CheckInternetConnection()
    {
        var myPing = new Ping();

        PingReply reply = myPing.Send("google.com", 10, new byte[32], new PingOptions());

        if (reply.Status == IPStatus.Success)
        {
            return true;
        }
        else
        {
            Console.WriteLine("Нет подключения к Internet");

            return false;
        }
    }
}