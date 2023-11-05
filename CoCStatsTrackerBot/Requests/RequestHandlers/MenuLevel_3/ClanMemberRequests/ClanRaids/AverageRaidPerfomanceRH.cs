using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class AverageRaidPerfomanceRH : BaseRequestHandler
{
    public AverageRaidPerfomanceRH()
    {
        Header = "Средние показатели игроков";
        HandlerMenuLevel = MenuLevel.ClanRaidsHistory3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var averagePergomances = GetFromDbQueryHandler.GetAllClanMembersAverageRaidPerfomance(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetMembersAverageRaidsPerfomance(averagePergomances);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
