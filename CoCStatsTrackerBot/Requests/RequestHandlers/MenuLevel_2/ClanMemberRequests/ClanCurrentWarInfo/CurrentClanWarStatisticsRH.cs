using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarStatisticsRH : BaseRequestHandler
{
    public CurrentClanWarStatisticsRH()
    {
        Header = "Показатели войны";
        HandlerMenuLevel = MenuLevel.CurrentWarInfo2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            parameters.EntriesCount = 1;

            var lastClanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetClanWarHistory(new List<ClanWarUi>() { lastClanWarUi }, parameters.EntriesCount, MessageSplitToken);

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