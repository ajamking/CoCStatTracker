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

public static class BotBackgroundUpdateManager
{
    private static string _hashOfErrorLogFile = null;

    static BotBackgroundUpdateManager()
    {
        _hashOfErrorLogFile = GetHashOfErrorLogsFile();
    }

    public static async Task StartAstync(ITelegramBotClient botClient)
    {
        var clanNewsLetterStates = new List<ClanNewsLetterState>();

        while (CheckInternetConnection())
        {
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Обновляю кланы...");

            var allTrackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

            var tasks = allTrackedClans
           .Select(x => Task.Run(() => UpdateAllProperties(x)))
           .ToList();

            await Task.WhenAll(tasks);

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Все кланы обновлены.\n");

            var isLogFileSent = await SendLogFileToAdmin();

            if (isLogFileSent)
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Обновленный файл ErrorLogs.txt послан админу.\n");
            }

            var isAnySeasonalStatisticChanged = await TryChangeSeasonalStatistics(allTrackedClans);

            foreach (var update in isAnySeasonalStatisticChanged.Where(x => x.Value == true))
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Сезонная статистика обновлена для {update.Key}.\n");
            }

            clanNewsLetterStates.AddNewsletterStateForNewClans();

            await BotBackgroundNewsLetterManager.StartAstync(botClient, clanNewsLetterStates);

            await Task.Delay(TimeSpan.FromHours(1));
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
            return true;
        }
        else
        {
            Console.WriteLine("Нет подключения к Internet");

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

    private static async Task<Dictionary<string, bool>> TryChangeSeasonalStatistics(List<TrackedClan> trackedClans)
    {
        var isSeasonalStatisticChanged = new Dictionary<string, bool>();

        foreach (var clan in trackedClans)
        {
            var seasonalStatisticsUi = GetFromDbQueryHandler.GetSeasonStatisticsUi(clan.Tag);

            if ((DateTime.Now - seasonalStatisticsUi.First().InitializedOn).TotalDays > 30)
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

    private static void AddNewsletterStateForNewClans(this List<ClanNewsLetterState> clanNewsLetterStates)
    {
        var properClans = GetFromDbQueryHandler.GetAllTrackedClans()
              .Where(x => x.IsInBlackList == false && x.RegularNewsLetterOn == true && !string.IsNullOrEmpty(x.ClansTelegramChatId))
              .ToList();

        foreach (var clan in properClans)
        {
            if (clanNewsLetterStates.FirstOrDefault(x => x.Tag == clan.Tag) == null)
            {
                clanNewsLetterStates.Add(
                    new ClanNewsLetterState(clan.Tag, clan.Name, clan.ClansTelegramChatId, clan.RaidTimeToMessageBeforeEnd, clan.WarTimeToMessageBeforeEnd)
                    {
                        WarStartNewsLetterOn = clan.WarStartMessageOn,
                        WarEndNewsLetterOn = clan.WarEndMessageOn,
                        RaidStartNewsLetterOn = clan.RaidStartMessageOn,
                        RaidEndNewsLetterOn = clan.RaidEndMessageOn,
                    });
            }
            else
            {
                var clanNewsLetter = clanNewsLetterStates.First(x => x.Tag == clan.Tag);

                clanNewsLetter.WarTimeToMessageBeforeEnd = clan.WarTimeToMessageBeforeEnd;
                clanNewsLetter.WarStartNewsLetterOn = clan.WarStartMessageOn;
                clanNewsLetter.WarEndNewsLetterOn = clan.WarEndMessageOn;

                clanNewsLetter.RaidTimeToMessageBeforeEnd = clan.RaidTimeToMessageBeforeEnd;
                clanNewsLetter.RaidStartNewsLetterOn = clan.RaidStartMessageOn;
                clanNewsLetter.RaidEndNewsLetterOn = clan.RaidEndMessageOn;



            }
        }
    }
}