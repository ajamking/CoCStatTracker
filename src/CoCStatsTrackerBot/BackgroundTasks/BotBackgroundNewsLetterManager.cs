using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Requests;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoCStatsTrackerBot;

public static class BotBackgroundNewsLetterManager
{
    private static int _timeAfterNoNeedToSendAMessage = 2;

    private static List<ClanNewsLetterState> _clanNewsLetterStates = new();

    public static async Task StartAstync(ITelegramBotClient botClient)
    {
        _clanNewsLetterStates.ChangeNewsLetterStatesForAllClans();

        Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Начинаю рассылку в группы...");

        foreach (var clanNewsLetter in _clanNewsLetterStates)
        {
            await clanNewsLetter.SendGroupRaidMessages(botClient);

            await clanNewsLetter.SendGroupWarMessages(botClient);

            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> [{clanNewsLetter.Tag}] - {clanNewsLetter.Name} - успешно.");
        }

        Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Сообщения разосланы.\n");
    }

    private async static Task<ClanNewsLetterState> SendGroupRaidMessages(this ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        try
        {
            var lastRaidUi = GetFromDbQueryHandler.GetLastRaidUi(clanNewsLetterState.Tag);

            clanNewsLetterState.TryResetRaidNewsLetterState(lastRaidUi.StartedOn);

            if (clanNewsLetterState.RaidIsStartMessageSent == false && clanNewsLetterState.RaidStartNewsLetterOn)
            {
                clanNewsLetterState.RaidIsStartMessageSent = await SendRaidsStartMessage(lastRaidUi, clanNewsLetterState, botClient);
            }
            if (clanNewsLetterState.RaidIsCustomTimeMessageSent == false && clanNewsLetterState.RaidTimeToMessageBeforeEnd != 0)
            {
                clanNewsLetterState.RaidIsCustomTimeMessageSent = await SendRaidsCustomTimeMessage(lastRaidUi, clanNewsLetterState, botClient);
            }
            if (clanNewsLetterState.RaidIsEndMessageSent == false && clanNewsLetterState.RaidEndNewsLetterOn)
            {
                clanNewsLetterState.RaidIsEndMessageSent = await SendRaidsEndMessage(lastRaidUi, clanNewsLetterState, botClient);
            }

            return clanNewsLetterState;
        }
        catch (Exception e)
        {
            if (e.Message == "Bad Request: chat not found")
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> [{clanNewsLetterState.Tag}] - {clanNewsLetterState.Name} - невозможно отправиль сообщение, бот не подключен к чату клана.");
            }
            else
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> [{clanNewsLetterState.Tag}] - {clanNewsLetterState.Name} - нет последнего рейда.");
            }

            return clanNewsLetterState;
        }
    }

    private async static Task<ClanNewsLetterState> SendGroupWarMessages(this ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        try
        {
            var allClanWars = GetFromDbQueryHandler.GetAllClanWarsUi(clanNewsLetterState.Tag).OrderByDescending(x => x.StartedOn).ToList();

            var lastWarUi = allClanWars.First();

            if (allClanWars.Count>1 && allClanWars[1].IsCwl)
            {
                lastWarUi = allClanWars[1];
            }

            clanNewsLetterState.TryResetClanWarNewsLetterState(lastWarUi.StartedOn);

            if (clanNewsLetterState.WarIsStartMessageSent == false && clanNewsLetterState.WarStartNewsLetterOn)
            {
                clanNewsLetterState.WarIsStartMessageSent = await SendWarStartMessage(lastWarUi, clanNewsLetterState, botClient);
            }
            if (clanNewsLetterState.WarIsCustomTimeMessageSent == false && clanNewsLetterState.WarTimeToMessageBeforeEnd != 0)
            {
                clanNewsLetterState.WarIsCustomTimeMessageSent = await SendWarCustomTimeMessage(lastWarUi, clanNewsLetterState, botClient);
            }
            if (clanNewsLetterState.WarIsEndMessageSent == false && clanNewsLetterState.WarEndNewsLetterOn)
            {
                clanNewsLetterState.WarIsEndMessageSent = await SendWarEndMessage(lastWarUi, clanNewsLetterState, botClient);
            }

            return clanNewsLetterState;
        }
        catch (Exception e)
        {
            if (e.Message == "Bad Request: chat not found")
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> [{clanNewsLetterState.Tag}] - {clanNewsLetterState.Name} - невозможно отправиль сообщение, бот не подключен к чату клана.");
            }
            else
            {
                Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> [{clanNewsLetterState.Tag}] - {clanNewsLetterState.Name} - нет последней войны.");
            }

            return clanNewsLetterState;
        }
    }


    private async static Task<bool> SendRaidsStartMessage(CapitalRaidUi lastRaidUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (lastRaidUi == null) return false;

        if (DateTime.Now.Subtract(lastRaidUi.StartedOn).TotalHours > 0 && DateTime.Now.Subtract(lastRaidUi.StartedOn).TotalHours < _timeAfterNoNeedToSendAMessage)
        {
            var newAnswer = NewsLetterFunctions.GetRaidStartShortMessage(lastRaidUi);

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                   text: newAnswer,
                   parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    private async static Task<bool> SendRaidsCustomTimeMessage(CapitalRaidUi lastRaidUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (lastRaidUi == null) return false;

        var raidTimeLeft = lastRaidUi.EndedOn.Subtract(DateTime.Now).TotalHours;

        if (raidTimeLeft >= clanNewsLetterState.RaidTimeToMessageBeforeEnd - 0.5 && raidTimeLeft <= clanNewsLetterState.RaidTimeToMessageBeforeEnd + 0.5)
        {
            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(lastRaidUi);

            var newAnswer = answer.Insert(0, StylingHelper.MakeItStyled("Дни рейдов близятся к концу! Не забудьте провести атаки!", UiTextStyle.Header) + " \n\n");

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                 text: newAnswer,
                 parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    private async static Task<bool> SendRaidsEndMessage(CapitalRaidUi lastRaidUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (lastRaidUi == null) return false;

        if (DateTime.Now.Subtract(lastRaidUi.EndedOn).TotalHours > 0 && DateTime.Now.Subtract(lastRaidUi.EndedOn).TotalHours < _timeAfterNoNeedToSendAMessage)
        {
            var newAnswer = NewsLetterFunctions.GetRaidEndShortMessage(lastRaidUi);

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                 text: newAnswer,
                 parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }


    private async static Task<bool> SendWarStartMessage(ClanWarUi clanWarUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (clanWarUi == null) return false;

        if (DateTime.Now.Subtract(clanWarUi.StartedOn).TotalHours > 0 && DateTime.Now.Subtract(clanWarUi.StartedOn).TotalHours < _timeAfterNoNeedToSendAMessage)
        {
            var newAnswer = NewsLetterFunctions.GetClanWarStartShortMessage(clanWarUi);

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                 text: newAnswer,
                 parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    private async static Task<bool> SendWarCustomTimeMessage(ClanWarUi clanWarUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (clanWarUi == null) return false;

        var warTimeLeft = clanWarUi.EndedOn.Subtract(DateTime.Now).TotalHours;

        if (warTimeLeft >= clanNewsLetterState.WarTimeToMessageBeforeEnd - 0.5 && warTimeLeft <= clanNewsLetterState.WarTimeToMessageBeforeEnd + 0.5)
        {
            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(clanWarUi);

            var newAnswer = answer.Insert(0, StylingHelper.MakeItStyled("Война почти завершилась! Не забудьте провести атаки!", UiTextStyle.Header) + " \n\n");

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                 text: newAnswer,
                 parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }

    private async static Task<bool> SendWarEndMessage(ClanWarUi clanWarUi, ClanNewsLetterState clanNewsLetterState, ITelegramBotClient botClient)
    {
        if (clanWarUi == null) return false;

        if (DateTime.Now.Subtract(clanWarUi.EndedOn).TotalHours > 0 && DateTime.Now.Subtract(clanWarUi.EndedOn).TotalHours < _timeAfterNoNeedToSendAMessage)
        {
            var newAnswer = NewsLetterFunctions.GetClanWarEndShortMessage(clanWarUi);

            await botClient.SendTextMessageAsync(clanNewsLetterState.TelegramsChatId,
                 text: newAnswer,
                 parseMode: ParseMode.MarkdownV2);

            return true;
        }

        return false;
    }


    private static void TryResetRaidNewsLetterState(this ClanNewsLetterState clanNewsLetterState, DateTime raidStartedOn)
    {
        if (clanNewsLetterState.RaidStartedOn == DateTime.MinValue || clanNewsLetterState.RaidStartedOn != raidStartedOn)
        {
            clanNewsLetterState.RaidStartedOn = raidStartedOn;
            clanNewsLetterState.RaidIsStartMessageSent = false;
            clanNewsLetterState.RaidIsEndMessageSent = false;
            clanNewsLetterState.RaidIsCustomTimeMessageSent = false;
        }
    }

    private static void TryResetClanWarNewsLetterState(this ClanNewsLetterState clanNewsLetterState, DateTime warStartedOn)
    {
        if (clanNewsLetterState.WarStartedOn == DateTime.MinValue || clanNewsLetterState.WarStartedOn != warStartedOn)
        {
            clanNewsLetterState.WarStartedOn = warStartedOn;
            clanNewsLetterState.WarIsStartMessageSent = false;
            clanNewsLetterState.WarIsCustomTimeMessageSent = false;
            clanNewsLetterState.WarIsEndMessageSent = false;
        }
    }

    private static void ChangeNewsLetterStatesForAllClans(this List<ClanNewsLetterState> clanNewsLetterStates)
    {
        var properClans = GetFromDbQueryHandler.GetAllTrackedClans()
              .Where(x => x.IsInBlackList == false && !string.IsNullOrEmpty(x.ClansTelegramChatId))
              .ToList();

        foreach (var clan in properClans)
        {
            var clanNewsLetter = clanNewsLetterStates.FirstOrDefault(x => x.Tag == clan.Tag);

            if (clan.RegularNewsLetterOn == false)
            {
                clanNewsLetterStates.Remove(clanNewsLetter);

                continue;
            }

            if (clanNewsLetter == null)
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
                clanNewsLetter.WarTimeToMessageBeforeEnd = clan.WarTimeToMessageBeforeEnd;
                clanNewsLetter.WarStartNewsLetterOn = clan.WarStartMessageOn;
                clanNewsLetter.WarEndNewsLetterOn = clan.WarEndMessageOn;

                clanNewsLetter.RaidTimeToMessageBeforeEnd = clan.RaidTimeToMessageBeforeEnd;
                clanNewsLetter.RaidStartNewsLetterOn = clan.RaidStartMessageOn;
                clanNewsLetter.RaidEndNewsLetterOn = clan.RaidEndMessageOn;

                clanNewsLetter.TelegramsChatId = clan.ClansTelegramChatId;
            }
        }
    }
}