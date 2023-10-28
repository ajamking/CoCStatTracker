using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class CurrentClanWarStatisticsRH : BaseRequestHandler
{
    public CurrentClanWarStatisticsRH()
    {
        Header = "Показатели";
        HandlerMenuLevel = MenuLevels.CurrentWarInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            parameters.EntriesCount = 1;

            var allClanWars = GetFromDbQueryHandler.GetAllClanWars(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn).ToList();

            var answer = ClanFunctions.GetClanWarHistory(allClanWars, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
