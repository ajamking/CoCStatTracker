using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class CurrentRaidStatisticsRH : BaseRequestHandler
{
    public CurrentRaidStatisticsRH()
    {
        Header = "Показатели рейда";
        HandlerMenuLevel = MenuLevel.CurrentRaidInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            parameters.EntriesCount = 1;

            var lastRaidUi = GetFromDbQueryHandler.GetLastRaidUi(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetRaidsHistory(new List<CapitalRaidUi>() { lastRaidUi }, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}