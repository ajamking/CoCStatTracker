using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class AverageRaidPerfomanceRH : BaseRequestHandler
{
    public AverageRaidPerfomanceRH()
    {
        Header = "Средние показатели игроков";
        HandlerMenuLevel = MenuLevels.ClanRaidsHistory3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var averagePergomances = GetFromDbQueryHandler.GetAllClanMembersAverageRaidPerfomance(parameters.LastTagMessage);

            var answer = ClanFunctions.GetMembersAverageRaidsPerfomance(averagePergomances);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
