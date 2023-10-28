using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentRaidStatisticsRH : BaseRequestHandler
{
    public CurrentRaidStatisticsRH()
    {
        Header = "Показатели";
        HandlerMenuLevel = MenuLevels.CurrentRaidInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            parameters.EntriesCount = 1;

            var allRaids = GetFromDbQueryHandler.GetAllRaids(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn).ToList();

            var answer = ClanFunctions.GetRaidsHistory(allRaids, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
