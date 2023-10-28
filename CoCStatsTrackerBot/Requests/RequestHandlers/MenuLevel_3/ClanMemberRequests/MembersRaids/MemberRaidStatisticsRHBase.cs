using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;   

public class MemberRaidStatisticsRHBase : BaseRequestHandler
{
    public MemberRaidStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.PlayerRaidStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var raidMembershipsUi = GetFromDbQueryHandler.GetAllMemberRaidMemberships(parameters.LastTagMessage);

            var answer = PlayerFunctions.GetRaidStatistics(raidMembershipsUi, parameters.EntriesCount, MessageSplitToken);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
