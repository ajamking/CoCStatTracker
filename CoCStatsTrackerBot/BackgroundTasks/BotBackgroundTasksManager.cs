using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.Requests;
using Domain.Entities;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

public static class BotBackgroundTasksManager
{
    private static string _hashOfErrorLogFile = null;

    static BotBackgroundTasksManager()
    {
        _hashOfErrorLogFile = GetHashOfErrorLogsFile();
    }

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

            await SendLogFileToAdmin();

            await TryChangeSeasonalStatistics();

            await Task.Delay(TimeSpan.FromHours(1));
        }
    }

    private static void UpdateAllProperties(TrackedClan clan)
    {
        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanBaseProperties(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanClanMembers(clan.Tag), $"[{clan.Tag}] - {clan.Name}");


        ExecuteUpdate(() => AddToDbCommandHandler.AddCurrentRaidToClan(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateClanCurrentRaid(clan.Tag), $"[{clan.Tag}] - {clan.Name}");


        ExecuteUpdate(() => AddToDbCommandHandler.AddCurrentCwlClanWarsToClan(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateCurrentCwlClanWars(clan.Tag), $"[{clan.Tag}] - {clan.Name}");


        ExecuteUpdate(() => AddToDbCommandHandler.AddCurrentClanWarToClan(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateCurrentClanWar(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        Console.WriteLine($"[{clan.Tag}] - {clan.Name} - Клан полностью обновлен.");
    }

    private static void ExecuteUpdate(Action action, string clanTagAndName)
    {
        try
        {
            action.Invoke();
        }
        catch (AlreadyExistsException)
        {
            return;
        }
        catch (FailedPullFromApiException)
        {
            return;
        }
        catch (Exception ex)
        {
            ex.LogException("BaackGroundTask", 1234567890, action.Method.Name, "Обновление частично не прошло, ошибка");

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

            if (raidTimeLeft > 11 && raidTimeLeft < 12)
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

            if (warTimeLeft > 2 && warTimeLeft < 1)
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
        //var process = new Process();
        //process.StartInfo.FileName = "ping";
        //process.StartInfo.Arguments = "-c 5 google.com";
        //process.StartInfo.RedirectStandardOutput = true;

        //process.Start();
        //process.WaitForExit();

        ////Console.WriteLine(process.StandardOutput.ReadToEnd());

        //return true;

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

    private static async Task SendLogFileToAdmin()
    {
        var newHash = GetHashOfErrorLogsFile();

        if (_hashOfErrorLogFile != newHash)
        {
            await Program.BotClient.SendDocumentAsync(Program.AdminsChatId,
                      document: Program.ExceptionLogsPath);

            _hashOfErrorLogFile = newHash;
        }
    }

    private static async Task TryChangeSeasonalStatistics()
    {
        var trackedClansUi = GetFromDbQueryHandler.GetAllTrackedClansUi();

        foreach (var clan in trackedClansUi)
        {
            var seasonalStatisticsUi = GetFromDbQueryHandler.GetSeasonStatisticsUi(clan.Tag);

            if ((DateTime.Now - seasonalStatisticsUi.First().InitializedOn).TotalDays > 30)
            {
                UpdateDbCommandHandler.ResetLastClanMembersStaticstics(clan.Tag);
            }
        }
    }

    private static string GetHashOfErrorLogsFile()
    {
        using var md5 = MD5.Create();

        using var stream = File.OpenRead(Program.ExceptionLogsPath);

        var hash = md5.ComputeHash(stream);

        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}