using CoCStatsTracker;

namespace CoCStatsTrackerBot.Requests.RequestHandlers;

public static class AllSlashFunctionHandler
{
    public static void Handle(BotUserRequestParameters parameters, BotSlashFunction slashFunc)
    {
        try
        {
            switch (slashFunc)
            {
                case BotSlashFunction.GroupGetChatId:
                    HandleGroupGetChatId(parameters);
                    break;
                case BotSlashFunction.GroupGetRaidShortInfo:
                    HandleGroupGetRaidShortInfo(parameters);
                    break;
                case BotSlashFunction.GroupGetWarShortInfo:
                    HandleGroupGetWarShortInfo(parameters);
                    break;
                case BotSlashFunction.GroupGetWarMap:
                    HandleGroupGetWarMap(parameters);
                    break;
                default:
                    break;
            }
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, BaseRequestHandler.DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }

    private static void HandleGroupGetChatId(BotUserRequestParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true,
            StylingHelper.MakeItStyled($"{parameters.Message.Chat.Id}", UiTextStyle.Default));
    }

    private static void HandleGroupGetRaidShortInfo(BotUserRequestParameters parameters)
    {
        var clans = GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == parameters.Message.Chat.Id.ToString());

        foreach (var clan in clans)
        {
            var lastCapitalRaidUi = GetFromDbQueryHandler.GetLastRaidUi(clan.Tag);

            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(lastCapitalRaidUi);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }

    private static void HandleGroupGetWarShortInfo(BotUserRequestParameters parameters)
    {
        var clans = GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == parameters.Message.Chat.Id.ToString());

        foreach (var clan in clans)
        {
            var lastClanWar = GetFromDbQueryHandler.GetLastClanWarUi(clan.Tag);

            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(lastClanWar);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }

    private static void HandleGroupGetWarMap(BotUserRequestParameters parameters)
    {
        var clans = GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == parameters.Message.Chat.Id.ToString());

        foreach (var clan in clans)
        {
            var lastClanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(clan.Tag);

            var answer = CurrentStatisticsFunctions.GetCurrentWarMap(lastClanWarUi.WarMap);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }

    private static string[] SplitAnswer(string answer) => answer.Split(new[] { BaseRequestHandler.MessageSplitToken }, StringSplitOptions.RemoveEmptyEntries).ToArray();
}