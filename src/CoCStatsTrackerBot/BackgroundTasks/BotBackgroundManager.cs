using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.Requests;
using Domain.Entities;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace CoCStatsTrackerBot;

public static class BotBackgroundManager
{
    private static string _hashOfErrorLogFile = null;

    static BotBackgroundManager()
    {
        _hashOfErrorLogFile = GetHashOfErrorLogsFile();
    }

    public static async Task StartAstync(ITelegramBotClient botClient)
    {
        while (true)
        {
            //Перед каждой итерацией фоновых задач проверяем подключение к интернету.
            while (true)
            {
                if (CheckInternetConnection() == true)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Обновляю подписанные кланы...");

            var allTrackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

            /*Почему-то когда вызов этих функций происходит асинхронно - что-то там где-то ломается.
              Поэтому заменили асинхронный вариант на синхронный*/
            // var tasks = allTrackedClans
            //.Select(x => Task.Run(() => UpdateAllProperties(x)))
            //.ToList();
            // await Task.WhenAll(tasks);

            foreach (var clan in allTrackedClans.Where(x => x.IsInBlackList == false))
            {
                UpdateAllProperties(clan);
            }

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Все подписанные кланы обновлены.\n");

            var isLogFileSent = await SendLogFileToAdmin();

            if (isLogFileSent)
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Обновленный файл ErrorLogs.txt послан админу.\n");
            }

            var isAnySeasonalStatisticChanged = TryChangeSeasonalStatistics(allTrackedClans);

            foreach (var update in isAnySeasonalStatisticChanged.Where(x => x.Value == true))
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Сезонная статистика обновлена для {update.Key}.\n");
            }

            await BotBackgroundNewsLetterManager.StartAstync(botClient);

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    public static bool CheckInternetConnection()
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
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Подключение к Internet стабильно");

            return true;
        }
        else
        {
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Нет подключения к Internet");

            return false;
        }
    }

    private static void UpdateAllProperties(TrackedClan clan)
    {
        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanBaseProperties(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateTrackedClanClanMembers(clan.Tag), $"[{clan.Tag}] - {clan.Name}");


        ExecuteUpdate(() => UpdateDbCommandHandler.UpdateClanCurrentRaid(clan.Tag), $"[{clan.Tag}] - {clan.Name}");

        ExecuteUpdate(() => AddToDbCommandHandler.AddCurrentRaidToClan(clan.Tag), $"[{clan.Tag}] - {clan.Name}");


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
            ex.LogException("BotBackgroundUpdateManager", 0707, action.GetMethodInfo().Name, "Обновление частично не прошло, ошибка:");

            Console.WriteLine($"{clanTagAndName} - Обновление частично не прошло, ошибка: {ex.Message}");
        }
    }

    private static async Task<bool> SendLogFileToAdmin()
    {
        var newHash = GetHashOfErrorLogsFile();

        if (_hashOfErrorLogFile != newHash)
        {
            await Program.BotClient.SendDocumentAsync(Program.AdminsChatId,
                    new InputOnlineFile(System.IO.File.OpenRead(Program.ExceptionLogsPath))
                    { FileName = $"ErrorLogs {StylingHelper.FormateToUiDateTime(DateTime.Now)} .txt" });

            _hashOfErrorLogFile = newHash;

            return true;
        }

        return false;
    }

    private static Dictionary<string, bool> TryChangeSeasonalStatistics(List<TrackedClan> trackedClans)
    {
        var isSeasonalStatisticChanged = new Dictionary<string, bool>();

        foreach (var clan in trackedClans)
        {
            var previousClanMembers = GetFromDbQueryHandler.GetClanPreviousClanMembers(clan.Tag);

            if (previousClanMembers != null && previousClanMembers.Count != 0
               && (DateTime.Now - previousClanMembers.First().UpdatedOn).TotalDays > 30)
            {
                UpdateDbCommandHandler.ResetLastClanMembersStaticstics(clan.Tag);

                isSeasonalStatisticChanged.Add(clan.Name, true);
            }
            else
            {
                isSeasonalStatisticChanged.Add(clan.Name, false);
            }
        }

        return isSeasonalStatisticChanged;
    }

    private static string GetHashOfErrorLogsFile()
    {
        var stream = File.OpenRead(Program.ExceptionLogsPath);

        var hash = MD5.Create().ComputeHash(stream);

        stream.Close();

        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}