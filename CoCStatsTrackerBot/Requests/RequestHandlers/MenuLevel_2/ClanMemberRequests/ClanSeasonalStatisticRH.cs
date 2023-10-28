using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanSeasonalStatisticRH : BaseRequestHandler
{
    public ClanSeasonalStatisticRH()
    {
        Header = "Показатели месяца";
        HandlerMenuLevel = MenuLevels.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var seasonalStatistics = GetFromDbQueryHandler.GetSeasonStatistics(parameters.LastTagMessage);

            var answer = ClanFunctions.GetSeasonClanMembersStatistcs(seasonalStatistics);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
