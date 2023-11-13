using CoCStatsTracker;

namespace CoCStatsTrackerBot.Requests.RequestHandlers.SlashFunctionHandlers;

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
        catch (NotFoundException e)
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
            var allRaids = GetFromDbQueryHandler.GetAllRaidsUi(clan.Tag).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentRaidShortInfo(allRaids.First());

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }

    private static void HandleGroupGetWarShortInfo(BotUserRequestParameters parameters)
    {
        var clans = GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == parameters.Message.Chat.Id.ToString());

        foreach (var clan in clans)
        {
            var allClanwars = GetFromDbQueryHandler.GetAllClanWarsUi(clan.Tag).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentWarShortInfo(allClanwars.First());

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }

    private static void HandleGroupGetWarMap(BotUserRequestParameters parameters)
    {
        var clans = GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.ClansTelegramChatId == parameters.Message.Chat.Id.ToString());

        foreach (var clan in clans)
        {
            var allClanwars = GetFromDbQueryHandler.GetAllClanWarsUi(clan.Tag).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetCurrentWarMap(allClanwars.First().WarMap);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
    }


    private static string[] SplitAnswer(string answer) => answer.Split(new[] { BaseRequestHandler.MessageSplitToken }, StringSplitOptions.RemoveEmptyEntries).ToArray();
}